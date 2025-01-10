using web_api.Models;

namespace web_api.Repository
{
    public class UserRepository
    {
        private readonly FMSContext _context;

        public UserRepository(FMSContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
