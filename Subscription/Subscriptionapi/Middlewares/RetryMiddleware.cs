namespace Subscriptionapi.Middlewares
{
    public class RetryMiddleware
    {
        private readonly RequestDelegate _next;

        public RetryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            int maxRetries = 5; 
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    await _next(context); 
                    return; 
                }
                catch (Exception)
                {
                    
                    retryCount++;
                    await Task.Delay(TimeSpan.FromSeconds(2)); 
                }
            }

           
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("API request failed after multiple retries.");
        }
    }
}