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
    internal class PlayListDAO
    {

        private string conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

        public PlaylistModel PlaylistFetch(string username)
        {
            var newConn = new NpgsqlConnection(conn);



            string sql = "SELECT @username,array_to_string(array_agg(distinct playlistname), ',') as playlistname FROM playlists GROUP BY @username; ";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, newConn);
            cmd.Parameters.AddWithValue("username", username);
            newConn.Open();

            NpgsqlDataReader reader = cmd.ExecuteReader();
            PlaylistModel playlist = new PlaylistModel();


            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    playlist.playlistname = reader.GetString(1);


                }

            }


            return playlist;
        }
    }
}