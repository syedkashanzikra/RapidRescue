namespace RapidRescue.Services
{
    public class SessionRemoval
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Inject IHttpContextAccessor to access the session
        public SessionRemoval(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Method to remove 'request_id' from the session
        public void RemoveRequestIdFromSession()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var requestId = session.GetInt32("request_id");

            // Log the current session value
            if (requestId != null)
            {
                session.Remove("request_id"); // Remove 'request_id'
                Console.WriteLine($"Session request_id {requestId} has been removed.");
            }
            else
            {
                Console.WriteLine("No request_id found in the session to remove.");
            }
        }
    }
}
