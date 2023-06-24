using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL.DTO;

public class UserDto
{
    public int Id { get; set; }
    public decimal Wallet { get; set; }
    public ICollection<CategoryDto> Categories { get; set; }
    public ICollection<TransactionDto> Transactions { get; set; }
}
