using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using MyReviewBook.Models;
using MyReviewBook.Useful;

namespace MyReviewBook.Services
{
    public class googleAPIBook
    {
        private string URL { get; set; }

        public googleAPIBook()
        {
            URL = "https://www.googleapis.com/books/v1/volumes";
        }

        private string tryConnectAtGoogleService(string UrlParameters)
        {
            var result = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            // List data response.
            // Blocking call! Program will wait here until a response is received or a timeout occurs.
            HttpResponseMessage response = client.GetAsync(UrlParameters).Result;
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch(HttpRequestException e)
            {
                throw new Exception("ErrorToConnect");
            }
            finally
            {
                client.Dispose();
            }
            return result;
        }

        public List<DashboardModel> getListBook(string title)
        {
            List<DashboardModel> listBooks = new List<DashboardModel>();
            string UrlParameters = $"?q={title}&printType=books&maxResults=10";
            string result = tryConnectAtGoogleService(UrlParameters);
            book obj = JsonConvert.DeserializeObject<book>(result);
            for (int i = 0; i < obj.Items.Count; i++)
            {
                DashboardModel bookModel = new DashboardModel();
                bookModel.Book.NameBook = obj.Items[i].VolumeInfo.Title;
                try
                {
                    bookModel.Book.Author = obj.Items[i].VolumeInfo.Authors[0];
                }
                catch
                {
                    bookModel.Book.Author = "";
                }
                bookModel.Book.PublishedData = Helper.ajustDate(obj.Items[i].VolumeInfo.PublishedDate);
                bookModel.Book.QuantityPage = obj.Items[i].VolumeInfo.PageCount;
                bookModel.Book.GoogleId = obj.Items[i].Id;
                listBooks.Add(bookModel);
            }
            listBooks = Helper.eliminateDuplicates(listBooks);
            return listBooks;
        }

        public BookModel getABook(string googleId)
        {
            string UrlParameters = $"?q={googleId}";
            string result = tryConnectAtGoogleService(googleId);
            book obj = JsonConvert.DeserializeObject<book>(result);
            BookModel bookModel = new BookModel();
            bookModel.NameBook = obj.Items[0].VolumeInfo.Title;
            try
            {
                bookModel.Author = obj.Items[0].VolumeInfo.Authors[0];
            }
            catch
            {
                bookModel.Author = "";
            }
            bookModel.PublishedData = Helper.ajustDate(obj.Items[0].VolumeInfo.PublishedDate);
            bookModel.QuantityPage = obj.Items[0].VolumeInfo.PageCount;
            bookModel.GoogleId = obj.Items[0].Id;
            return bookModel;
        }
    }
}
