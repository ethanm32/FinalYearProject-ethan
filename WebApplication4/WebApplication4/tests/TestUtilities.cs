using System;
using System.Data;
using System.Drawing;
using Npgsql;

public class NpgsqlDatabaseConnection: MockDatabaseConnection
{
    private readonly NpgsqlConnection _connection;

    public NpgsqlDatabaseConnection(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public IDbCommand CreateCommand()
    {
        return _connection.CreateCommand();
    }

  

    public void Open()
    {
        _connection.Open();
    }
}