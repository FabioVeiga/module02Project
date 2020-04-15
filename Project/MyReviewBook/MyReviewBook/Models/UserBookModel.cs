using Microsoft.AspNetCore.Http;
using MyReviewBook.Useful;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyReviewBook.Models
{
    public class UserBookModel
    {
        public int IdUserBook { get; set; }
        public int IdUser { get; set; }
        public int idBook { get; set; }
        public bool IsReading { get; set; }
        public DateTime DateRead { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public bool CanPublishIt { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public UserBookModel() { }

        public UserBookModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public int hasBookWriteByUserId(int userId, int bookId)
        {
            string sql = $"select ub.idBook as idBook from UserBook ub inner join books b on b.userId = ub.idUser " +
                         $"where ub.idUser = {userId} and ub.idBook = {bookId} " +
                         $"group by ub.idBook";
            ConnectionDB connection = new ConnectionDB();
            try
            {
                DataTable dt = connection.executeSelect(sql);
                if (dt.Rows.Count > 0)
                {
                    return int.Parse(dt.Rows[0]["idbook"].ToString());
                }
                return -1;
            }
            catch (Exception e)
            {
                connection.closeConnection();
                throw e;
            }

        }

        public bool insertUserBook(UserBookModel userBook, int idBook)
        {
            if (userBook.Review != null)
            {
                userBook.Review = MySqlHelper.EscapeString(userBook.Review);
            }
            string sql = $"insert into UserBook (idUser, idBook, idReading, DateRead, Rating, Review, CanPublishIt)values " +
                        $"('{userBook.IdUser}', '{idBook}', {userBook.IsReading}, '{userBook.DateRead.ToString("yyyy-MM-dd")}', '{userBook.Rating}', '{userBook.Review}', {userBook.CanPublishIt})";
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

        public bool updateUserBook(UserBookModel userBook, int bookId)
        {
            if (userBook.Review != null)
            {
                userBook.Review = MySqlHelper.EscapeString(userBook.Review);
            }
            string sql = "UPDATE userbook SET " +
                        $"idReading = {userBook.IsReading}, DateRead = '{userBook.DateRead.ToString("yyyy-MM-dd")}', " +
                        $"Rating = {userBook.Rating}, Review = '{userBook.Review}', CanPublishIt = {userBook.CanPublishIt} " +
                        $"where idUser = {userBook.IdUser} and idBook = {bookId}";
            try
            {
                ConnectionDB connection = new ConnectionDB();
                connection.executeComandSQL(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string[] getStatusUser(int userId)
        {
            string sql = $"select count(idUserBook)qntBook from UserBook where idUser = {userId} " +
                         $"union all select count(idUserBook) from UserBook where idUser = {userId} and idReading = 0 " +
                         $"union all select count(idUserBook) from UserBook where idUser = {userId} and idReading = 1";
            string[] result = new string[3];
            int cont = 0;
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);

            for (cont = 0; cont < dt.Rows.Count; cont++)
            {
                result[cont] = dt.Rows[cont]["qntBook"].ToString();
            }
            for (int i = cont; i < 3; i++)
            {
                result[i] = "0";
            }
            return result;
        }

        public List<DashboardModel> getListUserBook(int userId, int quantityPage, int currentPage, string search)
        {
            List<DashboardModel> books = new List<DashboardModel>();
            string sql = $"select b.idBook as idBook, namebook, author, PublishedDate, QuantityPage, googleID, " +
                         $"idReading, DateRead, Rating, Review, CanPublishIt " +
                         $"from books b left join UserBook us on us.idBook = b.idBook " +
                         $"where us.idUser = {userId} ";
            //if it has something at variable search
            if (search != "")
            {
                sql += $"and b.nameBook like '%{search}%' or b.Author like '%{search}%'";
            }
            //ajust pagagination
            int offset = 0;
            if (currentPage > 1)
            {
                offset = (quantityPage * (currentPage - 1));
            }
            sql += $" limit {quantityPage} offset {offset}";

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
                int idBookUser = book.UserBook.hasBookWriteByUserId(userId, book.Book.IdBook);
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
            return books;
        }

        public int TotalRegister(int userId, string search)
        {
            string sql = $"select count(*) as total " +
                         $"from userbook us inner join books b on b.idBook = us.idBook " +
                         $"where us.idUser = {userId} ";
            if (search != "")
            {
                sql += $"and b.nameBook like '%{search}%' or b.Author like '%{search}%'";
            }
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            return int.Parse(dt.Rows[0]["total"].ToString());
        }

        public DashboardModel getUserBook(int IdUserBook, int userId)
        {
            string sql = $"select b.idBook as idBook, namebook, author, PublishedDate, QuantityPage, googleID, " +
                       $"idReading, DateRead, Rating, Review, CanPublishIt " +
                       $"from books b left join UserBook us on us.idBook = b.idBook " +
                       $"where us.idUserBook = {IdUserBook}";

            ConnectionDB connection = new ConnectionDB();
            DataTable dtBook = connection.executeSelect(sql);
            DashboardModel book = new DashboardModel();
            book.Book.IdBook = int.Parse(dtBook.Rows[0]["idBook"].ToString());
            book.Book.NameBook = dtBook.Rows[0]["namebook"].ToString();
            book.Book.Author = dtBook.Rows[0]["author"].ToString();
            book.Book.PublishedData = DateTime.Parse(dtBook.Rows[0]["PublishedDate"].ToString());
            book.Book.QuantityPage = int.Parse(dtBook.Rows[0]["QuantityPage"].ToString());
            book.Book.GoogleId = dtBook.Rows[0]["googleID"].ToString();
            int idBookUser = book.UserBook.hasBookWriteByUserId(userId, book.Book.IdBook);
            if (idBookUser == book.Book.IdBook)
            {
                if (dtBook.Rows[0]["idReading"].ToString() == "0")
                {
                    book.UserBook.IsReading = false;
                }
                else
                {
                    book.UserBook.IsReading = true;
                }
                book.UserBook.DateRead = DateTime.Parse(dtBook.Rows[0]["DateRead"].ToString());
                book.UserBook.Rating = int.Parse(dtBook.Rows[0]["Rating"].ToString());
                book.UserBook.Review = dtBook.Rows[0]["Review"].ToString();
                if (dtBook.Rows[0]["CanPublishIt"].ToString() == "0")
                {
                    book.UserBook.CanPublishIt = false;
                }
                else
                {
                    book.UserBook.CanPublishIt = true;
                }
            }
            return book;
        }

        public List<DashboardModel> getReviewsForIndex(int limit, string search)
        {
            List<DashboardModel> books = new List<DashboardModel>();
            string sql = "select u.userName, u.picture, b.namebook, us.Review from UserBook us " +
                         "inner join books b on b.idBook = us.idBook inner join user u on u.userId = us.idUser " +
                         $"where us.canPublishIt = 1 and b.namebook like '%{search}%' " +
                         "order by us.idUserBook desc " +
                         $"LIMIT {limit}";
            ConnectionDB connection = new ConnectionDB();
            DataTable dtBook = new DataTable();
            dtBook = connection.executeSelect(sql);

            for (int i = 0; i < dtBook.Rows.Count; i++)
            {
                DashboardModel book = new DashboardModel();
                book.User.UserName = dtBook.Rows[i]["userName"].ToString();
                book.User.UrlPicture = dtBook.Rows[i]["picture"].ToString();
                book.Book.NameBook = dtBook.Rows[i]["namebook"].ToString();
                book.UserBook.Review = dtBook.Rows[i]["Review"].ToString();
                books.Add(book);
            }
            return books;
        }

        public int totalRegisterIndex(string search)
        {
            string sql = $"select count(*) as total from UserBook us inner join books b on b.idBook = us.idBook inner join user u on u.userId = us.idUser " +
                         $"where us.canPublishIt = 1 ";
            if (search != "")
            {
                sql += $"and b.namebook like '%{search}%'";
            }
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            return int.Parse(dt.Rows[0]["total"].ToString());
        }
    }
}
