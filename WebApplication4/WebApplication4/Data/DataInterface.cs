using System.Data;

public interface MockDatabaseConnection
{
    void Open();
    IDbCommand CreateCommand();
}