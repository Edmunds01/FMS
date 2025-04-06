using Microsoft.EntityFrameworkCore;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class UserRepository(FMSContext context) : IUserRepository
{
    private readonly FMSContext _context = context;

    public async Task<User> AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public Task<User?> GetUserAsync(string email) => _context.Users.FirstOrDefaultAsync(u => u.Email == email);
}
