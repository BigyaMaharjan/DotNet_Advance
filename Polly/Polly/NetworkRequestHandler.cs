using Polly;
using Polly.Retry;

public class NetworkRequestHandler : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    public NetworkRequestHandler()
    {
        _httpClient = new HttpClient();

        _retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode) // Retry if status code isn't successful
            .Or<HttpRequestException>()                                      // Retry on network exceptions
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),             // Exponential backoff: 2, 4, 8 seconds
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    Console.WriteLine($"Network request attempt {retryAttempt} failed. Retrying in {timespan}.");
                });
    }

    public async Task<HttpResponseMessage> MakeNetworkRequestAsync(string requestUri)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            Console.WriteLine($"Attempting request to {requestUri}");
            var response = await _httpClient.GetAsync(requestUri);
            return response;
        });
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
