using Microsoft.Ajax.Utilities;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using WebApplication4.Models;

namespace WebApplication4.Data
{
    internal class YourReviewDAO
    {

        private string conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

        public YourReviewModel ReviewFetch(string username, string track)
        {
            var newConn = new NpgsqlConnection(conn);



            string sql = "SELECT * from public.reviews where trackid=@track and username=@username";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, newConn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("track", track);
            newConn.Open();

            NpgsqlDataReader reader = cmd.ExecuteReader();
            YourReviewModel yourReview = new YourReviewModel(); 


            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    yourReview.username = reader.GetString(0);
                    yourReview.review_desc= reader.GetString(2);


                }

            }


            return yourReview;
        }
    }
}