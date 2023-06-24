using AutoMapper;
using ExpenseTracker.BLL.DTO;
using ExpenseTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Transaction, TransactionDto>()
             .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
             .ReverseMap();
    }
}
