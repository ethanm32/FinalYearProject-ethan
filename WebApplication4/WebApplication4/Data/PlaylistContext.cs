using Npgsql;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.CompilerServices;
using WebApplication4.Models;

public class PlaylistContext
{
    private string conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";
    

    public IEnumerable<PlaylistModel> GetPlaylists(string username)
    {
        var newConn = new NpgsqlConnection(conn);

        string query = "SELECT distinct playlistname from public.playlists where username = @username;";
        NpgsqlCommand cmd = new NpgsqlCommand(query, newConn);

        var param = new { Username = username};
        var result = newConn.Query<PlaylistModel>(query, param);
        return result;
    }
}