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
            catch(Exception e)
            {
                connection.closeConnection();
                throw e;
            }
           
        }

        public bool insertUserBook(UserBookModel userBook, int idBook)
        {
            if(userBook.Review != null)
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
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
