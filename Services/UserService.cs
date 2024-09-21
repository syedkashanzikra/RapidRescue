using RapidRescue.Context;
using RapidRescue.Models;

namespace RapidRescue.Services
{
    public class UserService
    {
        private readonly RapidRescueContext _context;

        public UserService(RapidRescueContext context)
        {
            _context = context;
        }

        public Users GetUserById(int? userId)
        {
            // Check if userId is not null
            if (userId == null)
            {
                return null; // Or handle it accordingly
            }

            return _context.Users.FirstOrDefault(u => u.User_id == userId.Value);
        }

    }
}
