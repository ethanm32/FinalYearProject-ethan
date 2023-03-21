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
    internal class ReviewDAO
    {

        private string conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

        public List<ReviewModel> ReviewAll(string track)
        {

            List<ReviewModel> returnList = new List<ReviewModel>();
            var newConn = new NpgsqlConnection(conn);



            string sql = "select * from public.reviews where trackid=@track;";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, newConn);
            cmd.Parameters.AddWithValue("track", track);

            newConn.Open();

            NpgsqlDataReader reader = cmd.ExecuteReader();


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ReviewModel reviews = new ReviewModel();

                    reviews.username = reader.GetString(0);
                    reviews.trackid = reader.GetString(1);
                    reviews.review_desc = reader.GetString(2);

                    returnList.Add(reviews);

                }

            }
            else
            {
                //display nothing
            }


            return returnList;
        }
    }
}