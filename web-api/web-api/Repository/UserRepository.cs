using web_api.Models;

namespace web_api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly FMSContext _context;

        public UserRepository(FMSContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public User? GetUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
