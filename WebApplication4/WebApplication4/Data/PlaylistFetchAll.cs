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
    internal class PlaylistFetchAll
    {

        private string conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

        public List<PlaylistModel> PlaylistAll(string username, string playlistname)
        {

            List<PlaylistModel> returnList = new List<PlaylistModel>();
            var newConn = new NpgsqlConnection(conn);



            string sql = "select * from public.playlists where username=@username and playlistname=@playlistname and trackname IS NOT NULL AND genre IS NOT NULL AND artist IS NOT NULL AND img IS NOT NULL;";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, newConn);
            cmd.Parameters.AddWithValue("playlistname", playlistname);
            cmd.Parameters.AddWithValue("username", username);
            newConn.Open();

            NpgsqlDataReader reader = cmd.ExecuteReader();


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    PlaylistModel playlists = new PlaylistModel();

                    playlists.trackname = reader.GetString(1);
                    playlists.genre= reader.GetString(3);
                    playlists.artist= reader.GetString(4);
                    playlists.img= reader.GetString(5);

                    returnList.Add(playlists);

                }

            } else
            {
                
            }


            return returnList;
        }
    }
}