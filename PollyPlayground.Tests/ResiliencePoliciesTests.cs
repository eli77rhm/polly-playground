using Xunit;

namespace PollyPlayground.Tests
{
    public class ResiliencePoliciesTests
    {
        private readonly MockService _mockService;
        private readonly ResiliencePolicies _policies;

        public ResiliencePoliciesTests()
        {
            _mockService = new MockService();
            _policies = new ResiliencePolicies();
        }

        [Fact]
        public async Task TestRetryPolicy_Success()
        {
            var result = await _policies.TestRetryPolicy(_mockService);
            Assert.Equal("Success: Data fetched from service", result);
        }

        [Fact]
        public async Task TestCircuitBreaker_Success()
        {
            var result = await _policies.TestCircuitBreaker(_mockService);
            Assert.Equal("Success: Data fetched from service", result);
        }

        [Fact]
        public async Task TestFallbackPolicy_Fallback()
        {
            var result = await _policies.TestFallbackPolicy(_mockService);
            Assert.Equal("Success: Data fetched from service", result);
        }

        /*[Fact]
        public async Task TestRateLimitPolicy_RateLimitExceeded()
        {
            await _policies.TestRateLimitPolicy(_mockService);
            // Expect rate limit to be exceeded and exception thrown
        }*/
    }
}