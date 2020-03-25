using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MyReviewBook.Useful;

namespace MyReviewBook.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string UserPass { get; set; }

        public UserModel() { }

        public bool getLogin(string user, string pass)
        {
            string sql = $"select username, userpass from user where username = '{user.ToLower()}' and userpass = '{pass}'";
            ConnectionDB connection = new ConnectionDB();
            DataTable dt = connection.executeSelect(sql);
            
            if(dt != null)
            {
                if(dt.Rows.Count == 1)
                {
                    UserName = dt.Rows[0]["username"].ToString();
                    return true;
                }
            }
            return false;
        }

        public bool insertUser(string user, string pass)
        {
            string sql = $"insert into user(username,userpass,active)values('{user.ToLower()}','{pass}',1);";
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
    }
}
