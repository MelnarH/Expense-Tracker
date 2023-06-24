using ExpenseTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Repositories.API;

public interface IUsersRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> UpdateAsync(User user);
    Task UpdateWalletAsync(User user);
    Task<decimal> GetUserWalletAsync(User user);
}