using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace MyReviewBook.Useful
{
    public class ConnectionDB
    {
        //private string Server = "35.188.222.189";
        //private string DataBase = "myreviewbook";
        //private string User = "root";
        //private string Pass = "GM9xBskq8xuhGkdy";
        private string Server = "localhost";
        private string DataBase = "myreviewbook";
        private string User = "root";
        private string Pass = "12345678";
        private string max_pool_size = "100";
        private MySqlConnection conn;

        public ConnectionDB()
        {
            conn = new MySqlConnection($"Server={Server};Database={DataBase};Uid={User};Pwd={Pass};max pool size={max_pool_size};");
            if (conn.State == ConnectionState.Open)
            {
                closeConnection();
            }
            else
            {
                conn.Open();
            }
                
        }

        public DataTable executeSelect(string sql)
        {
            DataTable dataTable = new DataTable();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            try
            {
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception e)
            {
                closeConnection();
                throw e;
            }
            
        }

        public void executeComandSQL(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, conn);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                closeConnection();
                throw e;
            }
        }

        public void closeConnection()
        {
            conn.Close();
        }

    }
}
