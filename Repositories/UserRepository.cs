using AccesoUsuariosAPI.Data;
using AccesoUsuariosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccesoUsuariosAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task<bool> EmailExistsAsync(string email)
        => await _db.Users.AnyAsync(x => x.Email == email);

    public async Task AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

    public async Task<User?> GetByIdAsync(Guid id)
        => await _db.Users.FindAsync(id);
}
