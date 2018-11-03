using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LittleCloudServer.Libs
{
    public class DatabaseConnector
    {
        private const string host = "1.224.172.108";
        private const string user = "root";
        private const string password = "1234";
        private const string dbName = "l_cloud";
        private const string connectionStr = "Server=" + host + ";Database=" + dbName + ";Uid=" + user + ";Pwd=" + password + ";Charset=utf8";

        private static DatabaseConnector instance = null;

        public static DatabaseConnector Instance
        {
            get
            {
                return instance == null ? instance = new DatabaseConnector() : instance;
            }
        }

        private MySqlConnection conn;

        private DatabaseConnector()
        {            
            this.conn = new MySqlConnection(connectionStr);
            conn.Open();
        }

        public void ExecuteNonQuery(MySqlCommand command)
        {
            command.Connection = conn;
            command.ExecuteNonQuery();
            command.Connection = null;
        }

        public DataSet ExecuteQuery(MySqlCommand command)
        {
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = command;
            da.SelectCommand.Connection = new MySqlConnection(connectionStr);

            DataSet result = new DataSet();

            da.Fill(result);
            command.Connection = null;

            return result;
        }

        public void Dispose()
        {
            this.conn.Dispose();
        }
    }
}
