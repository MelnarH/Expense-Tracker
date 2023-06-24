using ExpenseTracker.DAL.Entities;
using ExpenseTracker.DAL.Repositories.API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly DbSet<User> _set;
    private readonly ApplicationDbContext _context;

    public UsersRepository(ApplicationDbContext context)
    {
        _set = context.Users;
        _context = context;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _set.FindAsync(id);
    }

    public async Task<User> UpdateAsync(User user)
    {
        _set.Attach(user);
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateWalletAsync(User user)
    {
        _set.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task<decimal> GetUserWalletAsync(User user)
    {
        if (user != null)
        {
            return user.Wallet;
        }
        return 0;
    }
}