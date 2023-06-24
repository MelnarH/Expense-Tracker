using ExpenseTracker.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL.Services.API;

public interface ITransactionsService
{
    Task<DTOResponse<TransactionDto>> AddTransactionAsync(TransactionDto transactionDto, int loggedInUserId);
    Task<DTOResponse<TransactionDto>> GetTransactionByIdAsync(int transactionId, int loggedInUserId);
    Task<DTOResponse<List<TransactionDto>>> GetAllTransactionsAsync(int loggedInUserId);
    Task<DTOResponse<TransactionDto>> UpdateTransactionAsync(TransactionDto transactionDto, int loggedInUserId);
    Task<DTOResponse<TransactionDto>> DeleteTransactionAsync(int transactionId, int loggedInUserId);
}
