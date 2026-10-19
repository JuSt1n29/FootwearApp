using MySqlConnector;
using System;
using System.Data;

namespace FootwearApp
{
    public class DB
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        string connectionString = "Server=localhost;Database=footwear_catalog;Uid=root;Pwd=root;";

        private static DB instance = null;

        private DB()
        {
            conn = new MySqlConnection(connectionString);
            cmd = conn.CreateCommand();
        }

        public static DB getInstance()
        {
            if (instance == null)
            {
                instance = new DB();
            }
            return instance;
        }

        public bool execSql(string cmdtext) 
        {
            try
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Open();
                cmd.CommandText = cmdtext;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                if (conn.State == ConnectionState.Open) conn.Close();
                return false;
            }
        }

        public MySqlDataReader? execSelect(string cmdtext)
        {
            try
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Open();
                cmd.CommandText = cmdtext;
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (conn.State == ConnectionState.Open) conn.Close();
                return null;
            }
        }

        public object? execScalar(string cmdtext)
        {
            try
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Open();
                cmd.CommandText = cmdtext;
                var result = cmd.ExecuteScalar();
                conn.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (conn.State == ConnectionState.Open) conn.Close();
                return null;
            }
        }
    }
}