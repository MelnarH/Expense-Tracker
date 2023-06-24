using ExpenseTracker.DAL.Entities;
using ExpenseTracker.DAL.Repositories.API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Repositories;

internal class TransactionsRepository : ITransactionsRepository
{
    private readonly DbSet<Transaction> _set;
    private readonly ApplicationDbContext _context;

    public TransactionsRepository(ApplicationDbContext context)
    {
        _set = context.Transactions;
        _context = context;
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        await _set.AddAsync(transaction);
        if (await _context.SaveChangesAsync() > 0)
        {
            return transaction;
        }

        return null;
    }

    public async Task<Transaction> DeleteAsync(Transaction transaction)
    {
        _set.Remove(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }


    public async Task<IEnumerable<Transaction>> GetAllAsync(int loggedInUserId)
    {
        return await _set.Where(t => t.UserId == loggedInUserId).ToListAsync();
    }

    public async Task<Transaction> GetByIdAsync(int id)
    {
        return await _set.FindAsync(id);
    }

    public async Task<Transaction> UpdateAsync(Transaction transaction)
    {
        _set.Update(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }
}

