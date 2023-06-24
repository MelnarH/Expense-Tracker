using ExpenseTracker.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL.Services.API;

public interface IUsersService
{
    Task<DTOResponse<UserDto>> GetUserByIdAsync(int id);
    Task<DTOResponse<UserDto>> UpdateUserWalletAsync(int userId, decimal newWalletValue);
    Task<DTOResponse<decimal>> GetUserWalletAsync(int userId);
}
