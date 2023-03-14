﻿using Microsoft.Ajax.Utilities;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using WebApplication4.Models;

namespace WebApplication4.Data
{
    internal class RatingDAO
    {

        private string conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

        public RatingModel RatingFetch(string track)
        {
            var newConn = new NpgsqlConnection(conn);



            string sql = "SELECT AVG(rating) from public.ratings where trackid=@track";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, newConn);
            cmd.Parameters.AddWithValue("track", track);
            newConn.Open();

            NpgsqlDataReader reader = cmd.ExecuteReader();
            RatingModel total = new RatingModel();


            try 
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    total.total = reader.GetFloat(0);


                }

            }
            else
            {
                total.total = 0;
            }


            return total;
        }
    }
}