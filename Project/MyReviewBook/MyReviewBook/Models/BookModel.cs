using Microsoft.AspNetCore.Http;
using MyReviewBook.Useful;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace MyReviewBook.Models
{
    public class BookModel
    {
        public int IdBook { get; set; }
        public string NameBook { get; set; }
        public string Author { get; set; }
        public DateTime PublishedData { get; set; }
        public int QuantityPage { get; set; }
        public int UserID { get; set; }
        public string GoogleId { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public BookModel() { }
        public BookModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<DashboardModel> getListBook(string titleBook, int userSession)
        {
            List<DashboardModel> books = new List<DashboardModel>();
            string sql = $"select b.idBook as idBook, namebook, author, PublishedDate, QuantityPage, googleID, " +
                        $"idReading, DateRead, Rating, Review, CanPublishIt " +
                        $"from books b left join UserBook us on us.idBook = b.idBook " +
                        $"where b.namebook like '%{titleBook}%'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dtBook = new DataTable();
            dtBook = connection.executeSelect(sql);

            for (int i = 0; i < dtBook.Rows.Count; i++)
            {
                DashboardModel book = new DashboardModel();
                book.Book.IdBook = int.Parse(dtBook.Rows[i]["idBook"].ToString());
                book.Book.NameBook = dtBook.Rows[i]["namebook"].ToString();
                book.Book.Author = dtBook.Rows[i]["author"].ToString();
                book.Book.PublishedData = DateTime.Parse(dtBook.Rows[i]["PublishedDate"].ToString());
                book.Book.QuantityPage = int.Parse(dtBook.Rows[i]["QuantityPage"].ToString());
                book.Book.GoogleId = dtBook.Rows[i]["googleID"].ToString();
                int idBookUser = book.UserBook.hasBookWriteByUserId(userSession, book.Book.IdBook);
                if (idBookUser == book.Book.IdBook)
                {
                    if (dtBook.Rows[i]["idReading"].ToString() == "0")
                    {
                        book.UserBook.IsReading = false;
                    }
                    else
                    {
                        book.UserBook.IsReading = true;
                    }
                    book.UserBook.DateRead = DateTime.Parse(dtBook.Rows[i]["DateRead"].ToString());
                    book.UserBook.Rating = int.Parse(dtBook.Rows[i]["Rating"].ToString());
                    book.UserBook.Review = dtBook.Rows[i]["Review"].ToString();
                    if (dtBook.Rows[i]["CanPublishIt"].ToString() == "0")
                    {
                        book.UserBook.CanPublishIt = false;
                    }
                    else
                    {
                        book.UserBook.CanPublishIt = true;
                    }
                }
                books.Add(book);
            }
            connection.closeConnection();
            return books;
        }

        public int getIdBookByGoogleId(string googleId)
        {
            string sql = $"select idBook from books where googleID = '{googleId}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            return int.Parse(dt.Rows[0]["idBook"].ToString());
        }

        public int insertBook(BookModel book)
        {
            int idBookAux = 0;
            string sql = $"INSERT INTO books(nameBook, Author, PublishedDate, QuantityPage, userId, googleID)VALUES" +
                        $"('{book.NameBook}','{book.Author}','{book.PublishedData.ToString("yyyy-MM-dd")}','{book.QuantityPage}','{book.UserID}','{book.GoogleId}')";
            ConnectionDB connection = new ConnectionDB();
            try
            {
                connection.executeComandSQL(sql);
                sql = "select idBook from books " +
                     $"where nameBook = '{book.NameBook}' and Author = '{book.Author}' and PublishedDate = '{book.PublishedData.ToString("yyyy-MM-dd")}' " +
                     $"and QuantityPage = {book.QuantityPage} and userId = {book.UserID} and googleID = '{book.GoogleId}'";
                DataTable dt = connection.executeSelect(sql);
                if(dt.Rows.Count > 0)
                {
                    idBookAux = int.Parse(dt.Rows[0]["idBook"].ToString());
                }
                else
                {
                    idBookAux = 0;
                }
                return idBookAux;
            }
            catch
            {
                return idBookAux;
            }
        }

        public bool updateBook(BookModel book)
        {
            string sql = "UPDATE books SET " +
                         $"nameBook = '{book.NameBook}', Author = '{book.Author}', PublishedDate = '{book.PublishedData.ToString("yyyy-MM-dd")}', " +
                         $"QuantityPage = {book.QuantityPage} " +
                         $"WHERE idBook  = {book.IdBook}";
            ConnectionDB connection = new ConnectionDB();
            try
            {
                connection.executeComandSQL(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
