namespace PollyPlayground
{
    public class MockService
    {
        private Random _random = new Random();

        // Simulate a random failure
        public async Task<string> FetchDataAsync()
        {
            await Task.Delay(500); // Simulate network delay
            var fail = _random.Next(0, 5);
            if (fail == 0) // Simulate 20% failure
            {
                throw new HttpRequestException("Random failure");
            }
            return "Success: Data fetched from service";
        }
    }
}