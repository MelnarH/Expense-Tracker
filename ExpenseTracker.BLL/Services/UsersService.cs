using AutoMapper;
using ExpenseTracker.BLL.DTO;
using ExpenseTracker.BLL.Services.API;
using ExpenseTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL.Services;

public class UsersService : IUsersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UsersService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DTOResponse<UserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user != null)
            {
                UserDto userDto = _mapper.Map<UserDto>(user);
                return new DTOResponse<UserDto>(userDto);
            }

            return new DTOResponse<UserDto>("User not found.");
        }
        catch (Exception ex)
        {
            return new DTOResponse<UserDto>("An error occurred while retrieving the user.");
        }
    }

    public async Task<DTOResponse<decimal>> GetUserWalletAsync(int userId)
    {
        var response = new DTOResponse<decimal>("Failed to get user wallet.");
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user != null)
            {
                return new DTOResponse<decimal>(user.Wallet);
            }

            return new DTOResponse<decimal>("User not found.");
        }
        catch (Exception ex)
        {
            return new DTOResponse<decimal>("An error occurred while retrieving the user's wallet.");
        }
    }

    public async Task<DTOResponse<UserDto>> UpdateUserWalletAsync(int userId, decimal newWalletValue)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.Wallet = newWalletValue;
                await _unitOfWork.UserRepository.UpdateWalletAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var userDto = _mapper.Map<UserDto>(user);
                return new DTOResponse<UserDto>(userDto);
            }
            return new DTOResponse<UserDto>("User not found.");
        }
        catch (Exception ex)
        {
            return new DTOResponse<UserDto>("An error occurred while updating the user's wallet.");
        }
    }
}
