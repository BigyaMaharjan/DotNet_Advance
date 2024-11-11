namespace PollyRetryDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var networkHandler = new NetworkRequestHandler();
            var databaseHandler = new DatabaseConnectionHandler();
            int idCounter = 1;

            // Network request with retry policy
            try
            {
                var response = await networkHandler.MakeNetworkRequestAsync("https://www.google.com/sitemap.xml");
                Console.WriteLine($"Network request completed with status code: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network request ultimately failed: {ex.Message}");
            }

            // Database insert operation with retry policy
            try
            {
                var record = new DatabaseConnectionHandler.Record(idCounter++, "Sample Record");

                // Insert record into database with retry logic
                bool success = await databaseHandler.InsertRecordAsync(record, default);
                if (success)
                {
                    Console.WriteLine("Record successfully inserted into the database.");
                }
                else
                {
                    Console.WriteLine("Record insertion failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database operation ultimately failed: {ex.Message}");
            }

            Console.WriteLine("Program finished.");
        }
    }
}
