using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyReviewBook.Useful
{
    public class ConnectionDB
    {
        private string Server = "35.188.222.189";
        private string DataBase = "myreviewbook";
        private string User = "root";
        private string Pass = "GM9xBskq8xuhGkdy";
        private MySqlConnection conn;

        public ConnectionDB()
        {
            conn = new MySqlConnection($"Server={Server};Database={DataBase};Uid={User};Pwd={Pass};");
            conn.Open();
        }

        public DataTable executeSelect(string sql)
        {
            DataTable dataTable = new DataTable();
            MySqlCommand cmd = new MySqlCommand(sql,conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dataTable);
            return dataTable;
        }

        public void executeComandSQL(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();
        }

    }
}
