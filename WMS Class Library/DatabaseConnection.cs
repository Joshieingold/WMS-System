using Npgsql;
namespace WMS_Class_Library
{

    public class DatabaseConnection
    {
        string connectionString = "Host=localhost;Username=postgres;Password=Koreanishard2424$$;Database=postgres";
    
    public void ReadAllData(string databaseName)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand($"SELECT * FROM {databaseName}", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object value = reader.GetValue(i);
                    Console.Write($"{columnName}: {value}  ");
                }
                Console.WriteLine(); // New line after each row
            }
        }

    } 
}
