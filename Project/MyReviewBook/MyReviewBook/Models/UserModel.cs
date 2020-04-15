using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyReviewBook.Useful;
using MySql.Data.MySqlClient;

namespace MyReviewBook.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        private string UserPass { get; set; }
        public string UrlPicture { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        
        public UserModel() { }

        public UserModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public string GetUserSession()
        {
            return @HttpContextAccessor.HttpContext.Session.GetString("userSession");
        }

        public bool getLogin(string user, string pass)
        {
            string sql = $"select username, userpass from user where username = '{user.ToLower()}' and userpass = '{pass}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            
            if(dt != null)
            {
                if(dt.Rows.Count == 1)
                {
                    //UserName = dt.Rows[0]["username"].ToString();
                    return true;
                }
            }
            
            return false;
        }

        public bool insertUser(string user, string pass) {
            string sql = $"insert into user(username,userpass,active,createdate,isFirstAccess)" +
                         $"values('{user.ToLower()}','{pass}','1','{DateTime.Now.ToString("yyyy-MM-dd")}','1')";
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
        public char getFirstAccess(string user)
        {
            string sql = $"select isFirstAccess from user where username = '{user.ToLower()}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            return char.Parse(dt.Rows[0]["isFirstAccess"].ToString());
        }

        public bool getUser(string user)
        {
            string sql = $"select username, userpass from user where username = '{user.ToLower()}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            if(dt != null)
            {
                if(dt.Rows.Count == 1)
                {
                    return false;
                }
            }
            
            return true;
        }

        public bool updatePassword(string user, string pass)
        {
            string sql = $"update user set userpass = '{pass}' where username = '{user.ToLower()}'";
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

        public char getUserActive(string user)
        {
            string sql = $"select active from user where username = '{user}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = new DataTable();
            dt = connection.executeSelect(sql);
            if(dt != null){
                if(dt.Rows.Count == 1)
                {
                    if(dt.Rows[0]["active"].ToString() == "1")
                    {
                        return '1';
                    }
                }
            }
            return '0';
        }

        public char updateActiveUser(string user)
        {
            char currentActiveUser = getUserActive(user);
            if(currentActiveUser == '1')
            {
                currentActiveUser = '0';
            }
            else
            {
                currentActiveUser = '1';
            }
            string sql  = $"update user set active = '{currentActiveUser}' where username = '{user}'";
            ConnectionDB connection = new ConnectionDB();
            return currentActiveUser;
        }

        public string getPictureUser(string user)
        {
            string sql = $"select picture from user where username = '{user}'";
            string picture = "";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            if(dt != null)
            {
                picture =  dt.Rows[0]["picture"].ToString();
                if(picture != "")
                {
                    return picture;
                }
            }
            return "noPicture";
        }

        public void updatePictureUser(string user, string url)
        {
            string sql = $"update user set picture = '{url}' where username = '{user}'";
            ConnectionDB connection = new ConnectionDB();
            connection.executeComandSQL(sql);
        }
        private int getUserId(string user)
        {
            string sql = $"select userid from user where username = '{user}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            return int.Parse(dt.Rows[0]["userid"].ToString());
        }
        public bool updateForgetPassword(string user, string typeData, string validDate, string question, string answer)
        {
            validDate = MySqlHelper.EscapeString(validDate);
            question = MySqlHelper.EscapeString(question);
            answer = MySqlHelper.EscapeString(answer);
            int userId = getUserId(user);
            string sql = $"insert into userForgetPassword (userId,TypeData,ValidData,Question,AnsweredQuestion)" +
                         $"values({userId},'{typeData}','{validDate}','{question}','{answer}')";
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

        public bool updateFirstAccess(string user)
        {
            string sql = $"update user set isFirstAccess = '0' where username = '{user}'";
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

        public bool validUser(string user)
        {
            string sql = $"select userName from user where userName = '{user.ToLower()}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            if(dt != null)
            {
                if(dt.Rows.Count == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public int validUserId(string user)
        {
            string sql = $"select userId from user where userName = '{user.ToLower()}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            if (dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                    return int.Parse(dt.Rows[0]["userId"].ToString());
                }
            }
            return 0;
        }

        public string[] getValidData(string user)
        {
            string[] datas = new string[2];
            string sql = $"select TypeData, ValidData from `user` u " +
                        $"inner join `userForgetPassword` ufp on ufp.userId = u.userId " + 
                        $"where u.userName = '{user.ToLower()}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            datas[0] = dt.Rows[0]["TypeData"].ToString();
            datas[1] = dt.Rows[0]["ValidData"].ToString();
            return datas;
        }

        public string[] getAnwser(string user)
        {
            string[] datas = new string[2];
            string sql = $"select Question, AnsweredQuestion from `user` u " +
                        $"inner join `userForgetPassword` ufp on ufp.userId = u.userId " +
                        $"where u.userName = '{user.ToLower()}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            datas[0] = dt.Rows[0]["Question"].ToString();
            datas[1] = dt.Rows[0]["AnsweredQuestion"].ToString();
            return datas;
        }
        public string getPassword(string user)
        {
            string sql = $"select userPass from user where userName = '{user.ToLower()}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            return dt.Rows[0]["userPass"].ToString();
        }
    }
}
