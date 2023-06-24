using ExpenseTracker.BLL.DTO;
using ExpenseTracker.BLL.Services.API;
using ExpenseTracker.DAL.Entities;
using ExpenseTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ExpenseTracker.BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DTOResponse<CategoryDto>> CreateCategoryAsync(CategoryDto categoryDto, int loggedInUserId)
    {
        DTOResponse<CategoryDto> response = new DTOResponse<CategoryDto>("Failed to add category.");
        try
        {
            var existingCategory = await _unitOfWork.CategoryRepository.GetByNameAsync(categoryDto.Name);
            if (existingCategory != null)
            {
                categoryDto.Id = existingCategory.Id;
                return new DTOResponse<CategoryDto>(categoryDto);
            }

            var category = _mapper.Map<Category>(categoryDto);
            category.UserId = loggedInUserId;
            category.IsDeleted = false;

            response = await _unitOfWork.WrapInTransaction(
                async () =>
                {
                    var createdCategory = await _unitOfWork.CategoryRepository.AddAsync(category);
                    await _unitOfWork.SaveChangesAsync();

                    if (createdCategory != null)
                    {
                        var categoryResponse = _mapper.Map<CategoryDto>(createdCategory);
                        return new DTOResponse<CategoryDto>(categoryResponse);
                    }

                    return new DTOResponse<CategoryDto>("Failed to create category.");
                },
                result => result.Success,
                new DTOResponse<CategoryDto>("An error occurred during category creation.")
            );
        }
        catch (Exception ex)
        {
            response = new DTOResponse<CategoryDto>("An error occurred while retrieving existing category.");
        }

        return response;
    }


    public async Task<DTOResponse<CategoryDto>> GetCategoryByIdAsync(int categoryId, int loggedInUserId)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

        if (category != null && category.UserId == loggedInUserId)
        {
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new DTOResponse<CategoryDto>(categoryDto);
        }

        return new DTOResponse<CategoryDto>("Category not found or unauthorized.");
    }

    public async Task<DTOResponse<List<CategoryDto>>> GetAllCategoriesAsync(int loggedInUserId)
    {
        var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

        var userCategories = categories
             .Where(c => c.UserId == loggedInUserId)
             .ToList();

        var categoryDtos = _mapper.Map<List<CategoryDto>>(userCategories);

        return new DTOResponse<List<CategoryDto>>(categoryDtos);
    }

    public async Task<DTOResponse<CategoryDto>> UpdateCategoryAsync(CategoryDto categoryDto, int loggedInUserId)
    {
        DTOResponse<CategoryDto> response = new DTOResponse<CategoryDto>("Failed to update category.");

        try
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryDto.Id);

            if (category == null)
            {
                response.Errors.Add("Category not found.");
                return response;
            }

            // Check if the logged-in user has access to the category
            if (category.UserId != loggedInUserId)
            {
                response.Errors.Add("Unauthorized access to category.");
                return response;
            }

            category.Name = categoryDto.Name; // Update the category name

            response = await _unitOfWork.WrapInTransaction(
                async () =>
                {
                    var updatedCategory = await _unitOfWork.CategoryRepository.UpdateAsync(category);
                    await _unitOfWork.SaveChangesAsync();

                    if (updatedCategory != null)
                    {
                        var categoryResponse = _mapper.Map<CategoryDto>(updatedCategory);
                        return new DTOResponse<CategoryDto>(categoryResponse);
                    }

                    return new DTOResponse<CategoryDto>("Failed to update category.");
                },
                result => result.Success,
                new DTOResponse<CategoryDto>("Failed to update category.")
            );
        }
        catch (Exception ex)
        {
            response.Errors.Add("An error occurred while updating the category.");
        }

        return response;
    }


    public async Task<DTOResponse<CategoryDto>> DeleteCategoryAsync(int categoryId, int loggedInUserId)
    {
        DTOResponse<CategoryDto> response = new DTOResponse<CategoryDto>("Failed to delete category.");

        try
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                response.Errors.Add("Category not found.");
                return response;
            }

            // Check if the logged-in user has access to the category
            if (category.UserId != loggedInUserId)
            {
                response.Errors.Add("Unauthorized access to category.");
                return response;
            }

            response = await _unitOfWork.WrapInTransaction(
                async () =>
                {
                    var deletedCategory = await _unitOfWork.CategoryRepository.DeleteAsync(categoryId);
                    await _unitOfWork.SaveChangesAsync();

                    if (deletedCategory != null)
                    {
                        var categoryResponse = _mapper.Map<CategoryDto>(deletedCategory);

                        return new DTOResponse<CategoryDto>(categoryResponse);
                    }

                    return new DTOResponse<CategoryDto>("Failed to delete category.");
                },
                result => result.Success,
                new DTOResponse<CategoryDto>("Failed to delete category.")
            );
        }
        catch (Exception ex)
        {
            response.Errors.Add("An error occurred while deleting the category.");
        }

        return response;
    }

}
