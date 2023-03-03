using Microsoft.Ajax.Utilities;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using WebApplication4.Models;

namespace WebApplication4.Data
{ 
    internal class UserDAO
    {
        
        private string conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

        public UserModel Fetch(string username)
        {
            var newConn = new NpgsqlConnection(conn);



            string sql = "select * from public.users where username= @username";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, newConn);
            cmd.Parameters.AddWithValue("username", username);
            newConn.Open();

            NpgsqlDataReader reader = cmd.ExecuteReader();

            UserModel user = new UserModel();

            if (reader.HasRows)
            {
                while (reader.Read())
                {


                    user.email = reader.GetString(0);
                    user.password = reader.GetString(1);
                    user.name = reader.GetString(2);
                    user.username = reader.GetString(3);

                }

            }


            return user;
        }
    }
}