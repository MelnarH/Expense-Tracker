using ExpenseTracker.BLL.DTO;
using ExpenseTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL.Services.API;

public interface ICategoryService
{
    Task<DTOResponse<CategoryDto>> CreateCategoryAsync(CategoryDto categoryDto, int userId);
    Task<DTOResponse<CategoryDto>> GetCategoryByIdAsync(int categoryId, int userId);
    Task<DTOResponse<List<CategoryDto>>> GetAllCategoriesAsync(int userId);
    Task<DTOResponse<CategoryDto>> UpdateCategoryAsync(CategoryDto categoryDto, int userId);
    Task<DTOResponse<CategoryDto>> DeleteCategoryAsync(int categoryId, int userId);
}
