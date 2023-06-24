using ExpenseTracker.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL.DTO;

public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public CategoryDto Category { get; set; }
}