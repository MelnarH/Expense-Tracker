using ExpenseTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Repositories.API;

public interface ITransactionsRepository
{
    Task<IEnumerable<Transaction>> GetAllAsync(int loggedInUserId);
    Task<Transaction> GetByIdAsync(int id);
    Task<Transaction> AddAsync(Transaction transaction);
    Task<Transaction> UpdateAsync(Transaction transaction);
    Task<Transaction> DeleteAsync(Transaction transaction);
}