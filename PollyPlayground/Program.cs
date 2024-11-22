namespace PollyPlayground
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var resiliencePolicies = new ResiliencePolicies();
            var mockService = new MockService();

            Console.WriteLine("Testing Retry Policy...");
            await resiliencePolicies.TestRetryPolicy(mockService);

            Console.WriteLine("\nTesting Circuit Breaker...");
            await resiliencePolicies.TestCircuitBreaker(mockService);

            Console.WriteLine("\nTesting Cache Policy...");
            await resiliencePolicies.TestCachePolicy(mockService);

            Console.WriteLine("\nTesting Fallback...");
            await resiliencePolicies.TestFallbackPolicy(mockService);

            Console.WriteLine("\nTesting RateLimit...");
            await resiliencePolicies.TestRateLimitPolicy(mockService);
        }
    }
}