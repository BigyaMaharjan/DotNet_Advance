using Polly;
using System.Data.SQLite;

public class DatabaseConnectionHandler
{
    private readonly string _connectionString = "Data Source=mydatabase.db;Version=3;";
    public async Task<bool> InsertRecordAsync(
    Record record,
    CancellationToken cancellationToken)
    {
        int maxRetryCount = 3;

        // Define the retry policy with logging on each retry attempt
        var retryPolicy = Policy
            .Handle<SQLiteException>()  // Specify the type of exception to handle
            .WaitAndRetryAsync(maxRetryCount,
                retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),  // Exponential backoff
                onRetry: (exception, timespan, retryAttempt, context) =>
                {
                    // Log details about the retry attempt
                    Console.WriteLine($"Retry attempt {retryAttempt} due to: {exception?.Message}. Waiting {timespan.TotalSeconds} seconds before next attempt.");
                });

        // Ensure the table exists before attempting to insert
        await CreateTableIfNotExistsAsync();

        return await retryPolicy.ExecuteAsync(async ct =>
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync(ct);

            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Records (Name) VALUES (@Name)";
            command.Parameters.AddWithValue("@Name", record.Name);

            try
            {
                // First insert: should succeed
                var result = await command.ExecuteNonQueryAsync(ct);
                Console.WriteLine($"Record inserted: {record.Name}");
                return result == 1;  // Return true if one row is affected
            }
            catch (SQLiteException ex) when (ex.ErrorCode == 19) // Error code 19 is for constraint violations
            {
                // Simulate a duplicate entry failure after inserting once
                Console.WriteLine($"Attempt to insert duplicate record: {record.Name}. This should trigger retries.");
                throw new SQLiteException("Unique constraint failed. Retrying...");
            }
        }, cancellationToken);
    }


    // Method to create the table if it does not already exist
    private async Task CreateTableIfNotExistsAsync()
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Records (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT
            );";

        using var command = new SQLiteCommand(createTableQuery, connection);
        await command.ExecuteNonQueryAsync();
    }

    public sealed record Record(
        int Id,
        string Name);
}
