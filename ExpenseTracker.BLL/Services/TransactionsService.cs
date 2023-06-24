using AutoMapper;
using ExpenseTracker.BLL.DTO;
using ExpenseTracker.BLL.Services.API;
using ExpenseTracker.DAL;
using ExpenseTracker.DAL.Entities;
using ExpenseTracker.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL.Services;

public class TransactionsService : ITransactionsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public TransactionsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }

    public async Task<DTOResponse<TransactionDto>> AddTransactionAsync(TransactionDto transactionDto, int loggedInUserId)
    {
        DTOResponse<TransactionDto> response = new DTOResponse<TransactionDto>("Failed to add transaction.");
        try
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(transactionDto.CategoryId);
            if (category == null || category.UserId != loggedInUserId)
            {
                return new DTOResponse<TransactionDto>("Invalid category selection.");
            }

            var transaction = _mapper.Map<Transaction>(transactionDto);
            transaction.CreatedDate = DateTime.Now;
            transaction.UserId = loggedInUserId;

            response = await _unitOfWork.WrapInTransaction(
                async () =>
                {
                    var addedTransaction = await _unitOfWork.TransactionRepository.AddAsync(transaction);
                    await _unitOfWork.SaveChangesAsync();

                    var user = await _unitOfWork.UserRepository.GetByIdAsync(loggedInUserId);

                    if (transactionDto.TransactionType == TransactionType.Expense)
                    {
                        user.Wallet -= transactionDto.Amount;
                    }
                    else if (transactionDto.TransactionType == TransactionType.Income)
                    {
                        user.Wallet += transactionDto.Amount;
                    }
                    
                    await _unitOfWork.UserRepository.UpdateWalletAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    var transactionResponse = _mapper.Map<TransactionDto>(addedTransaction);

                    return new DTOResponse<TransactionDto>(transactionResponse);
                },
                result => result.Success,
                new DTOResponse<TransactionDto>("An error occurred during transaction creation.")
            );
        }
        catch (Exception ex)
        {
            response = new DTOResponse<TransactionDto>("An error occurred while processing the transaction.");
        }

        return response;
    }


    public async Task<DTOResponse<TransactionDto>> DeleteTransactionAsync(int transactionId, int loggedInUserId)
    {
        DTOResponse<TransactionDto> response = new DTOResponse<TransactionDto>("Failed to delete transaction.");
        try
        {
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);
            if (transaction == null || transaction.UserId != loggedInUserId)
            {
                return new DTOResponse<TransactionDto>("Invalid transaction or unauthorized.");
            }

            var transactionDto = _mapper.Map<TransactionDto>(transaction);

            response = await _unitOfWork.WrapInTransaction(
                async () =>
                {
                    await _unitOfWork.TransactionRepository.DeleteAsync(transaction);
                    await _unitOfWork.SaveChangesAsync();

                    var user = await _unitOfWork.UserRepository.GetByIdAsync(loggedInUserId);

                    if (transaction.TransactionType == TransactionType.Expense)
                    {
                        user.Wallet += transaction.Amount;
                    }
                    else if (transaction.TransactionType == TransactionType.Income)
                    {
                        user.Wallet -= transaction.Amount;
                    }
                    
                    await _unitOfWork.UserRepository.UpdateWalletAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    return new DTOResponse<TransactionDto>(transactionDto);
                },
                result => result.Success,
                new DTOResponse<TransactionDto>("An error occurred during transaction deletion.")
            );
        }
        catch (Exception ex)
        {
            response = new DTOResponse<TransactionDto>("An error occurred while processing the transaction deletion.");
        }

        return response;
    }

    public async Task<DTOResponse<List<TransactionDto>>> GetAllTransactionsAsync(int loggedInUserId)
    {
        var response = new DTOResponse<List<TransactionDto>>("Failed to retrieve transactions.");

        try
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAllAsync(loggedInUserId);

            if (transactions != null && transactions.Any())
            {
                var transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);

                return new DTOResponse<List<TransactionDto>>(transactionDtos);
            }

            return response;
        }
        catch (Exception ex)
        {
            
        }

        return response;
    }


    public async Task<DTOResponse<TransactionDto>> GetTransactionByIdAsync(int transactionId, int loggedInUserId)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionId);

        if (transaction != null && transaction.UserId == loggedInUserId)
        {
            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            return new DTOResponse<TransactionDto>(transactionDto);
        }

        return new DTOResponse<TransactionDto>("Transaction not found or unauthorized.");
    }


    public async Task<DTOResponse<TransactionDto>> UpdateTransactionAsync(TransactionDto transactionDto, int loggedInUserId)
    {
        DTOResponse<TransactionDto> response = new DTOResponse<TransactionDto>("Failed to update transaction.");
        try
        {
            var existingTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(transactionDto.Id);
            if (existingTransaction == null || existingTransaction.UserId != loggedInUserId)
            {
                return new DTOResponse<TransactionDto>("Invalid transaction or unauthorized access.");
            }

            existingTransaction = _mapper.Map(transactionDto, existingTransaction);

            response = await _unitOfWork.WrapInTransaction(
                async () =>
                {
                    await _unitOfWork.TransactionRepository.UpdateAsync(existingTransaction);
                    await _unitOfWork.SaveChangesAsync();

                    var user = await _unitOfWork.UserRepository.GetByIdAsync(loggedInUserId);

                    // Adjust the wallet based on the transaction type and amount difference
                    var amountDifference = transactionDto.Amount - existingTransaction.Amount;
                    if (transactionDto.TransactionType == TransactionType.Expense)
                    {
                        user.Wallet -= amountDifference;
                    }
                    else if (transactionDto.TransactionType == TransactionType.Income)
                    {
                        user.Wallet += amountDifference;
                    }

                    await _unitOfWork.UserRepository.UpdateWalletAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    var transactionResponse = _mapper.Map<TransactionDto>(existingTransaction);
                    transactionResponse.Category = transactionDto.Category;

                    return new DTOResponse<TransactionDto>(transactionResponse);
                },
                result => result.Success,
                new DTOResponse<TransactionDto>("An error occurred during transaction update.")
            );
        }
        catch (Exception ex)
        {
            response = new DTOResponse<TransactionDto>("An error occurred while processing the transaction.");
        }

        return response;
    }

}
