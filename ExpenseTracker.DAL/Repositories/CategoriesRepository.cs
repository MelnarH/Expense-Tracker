using ExpenseTracker.DAL.Entities;
using ExpenseTracker.DAL.Repositories.API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Repositories;

internal class CategoriesRepository : ICategoriesRepository
{
    private readonly DbSet<Category> _set;
    private readonly ApplicationDbContext _context;

    public CategoriesRepository(ApplicationDbContext context)
    {
        _set = context.Categories;
        _context = context;
    }
    public async Task<Category> AddAsync(Category category)
    {
        await _set.AddAsync(category);
        if (await _context.SaveChangesAsync() > 0)
        {
            return category;
        }

        return null;
    }

    public async Task<Category> DeleteAsync(int id)
    {
        var category = await _set.FindAsync(id);
        if (category == null || category.IsDeleted)
            return null;

        category.IsDeleted = true;
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _set.Where(c => !c.IsDeleted).ToListAsync();
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _set.FindAsync(id);
    }

    public async Task<Category> GetByNameAsync(string name)
    {
        return await _set.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        _set.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }
}
