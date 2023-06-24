using ExpenseTracker.DAL.Repositories;
using ExpenseTracker.DAL.Repositories.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL;

public interface IUnitOfWork
{
    ICategoriesRepository CategoryRepository { get; }
    ITransactionsRepository TransactionRepository { get; }
    IUsersRepository UserRepository { get; }
    Task SaveChangesAsync();
    Task<T> WrapInTransaction<T>(Func<Task<T>> method, Func<T, bool> checkIfOk, T defaultErrorState);
}

internal class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    private ICategoriesRepository _categoryRepository;
    public ICategoriesRepository CategoryRepository
    {
        get
        {
            if (_categoryRepository == null)
            {
                _categoryRepository = new CategoriesRepository(_context);
            }
            return _categoryRepository;
        }
    }

    private ITransactionsRepository _transactionsRepository;
    public ITransactionsRepository TransactionRepository
    {
        get
        {
            if(_transactionsRepository == null)
            {
                _transactionsRepository = new TransactionsRepository(_context);
            }
            return _transactionsRepository;
        }
    }

    private IUsersRepository _usersRepository;
    public IUsersRepository UserRepository { 
    
        get {
            if (_usersRepository == null)
            {
                _usersRepository = new UsersRepository(_context);
            }
            
            return _usersRepository; 
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<T> WrapInTransaction<T>(Func<Task<T>> method, Func<T, bool> checkIfOk, T defaultErrorResponse)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await method();
            if (checkIfOk(result))
            {
                await transaction.CommitAsync();
            }
            else
            {
                await transaction.RollbackAsync();
            }
            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }
        return defaultErrorResponse;
    }
}


