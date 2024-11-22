using Polly;
using Polly.RateLimit;

namespace PollyPlayground
{
    public class ResiliencePolicies
    {
        private readonly MemoryCacheProvider _cacheProvider;

        public ResiliencePolicies()
        {
            _cacheProvider = new MemoryCacheProvider();
        }
        
        // Retry Policy
        public async Task<string> TestRetryPolicy(MockService mockService)
        {
            var policy = Policy
                .Handle<HttpRequestException>()
                .RetryAsync(3); // Retry 3 times on failure

            try
            {
                var result = await policy.ExecuteAsync(() => mockService.FetchDataAsync());
                return result;
            }
            catch (Exception ex)
            {
                return ($"Retry Policy failed: {ex.Message}");
            }
        }

        // Circuit Breaker Policy
        public async Task<string> TestCircuitBreaker(MockService mockService)
        {
            var policy = Policy
                .Handle<HttpRequestException>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(1));  // Break after 2 failures, for 5 seconds

            try
            {
                var result = await policy.ExecuteAsync(() => mockService.FetchDataAsync());
                return result;
            }
            catch (Exception ex)
            {
                return ($"Circuit Breaker failed: {ex.Message}");
            }
        }

        // Cache Policy
        public async Task<string> TestCachePolicy(MockService mockService)
        {
            var cache = new Dictionary<string, string>();
            var cachePolicy = Policy.CacheAsync<string>(
                _cacheProvider, 
                TimeSpan.FromSeconds(10));

            var result = await cachePolicy.ExecuteAsync(() => mockService.FetchDataAsync());
            Console.WriteLine(result);

            // Simulate a cache hit
            result = await cachePolicy.ExecuteAsync(() => mockService.FetchDataAsync());
            return result;
        }

        // Fallback Policy
        public async Task<string> TestFallbackPolicy(MockService mockService)
        {
            var fallbackPolicy = Policy<string>
                .Handle<HttpRequestException>()
                .FallbackAsync( "Fallback data returned on failure");

            try
            {
                var result = await fallbackPolicy.ExecuteAsync(() => mockService.FetchDataAsync());
                return result;
            }
            catch (Exception )
            {
                return "Fallback failed";
            }
        }

        // RateLimit Policy
        public async Task TestRateLimitPolicy(MockService mockService)
        {
            var rateLimitPolicy = Policy
                .RateLimitAsync(2, TimeSpan.FromSeconds(5));  // Limit to 2 requests per 5 seconds

            // Simulating requests
            await rateLimitPolicy.ExecuteAsync(() => mockService.FetchDataAsync());
            await rateLimitPolicy.ExecuteAsync(() => mockService.FetchDataAsync());

            try
            {
                await rateLimitPolicy.ExecuteAsync(() => mockService.FetchDataAsync());  // This will be rate-limited
            }
            catch (RateLimitRejectedException)
            {
                Console.WriteLine("Rate limit exceeded!");
            }
        }
    }
}
