using AutoMapper;
using ExpenseTracker.BLL.Services;
using ExpenseTracker.BLL.Services.API;
using ExpenseTracker.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL;

public static class Startup
{

    public static void RegisterBllServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterDalServices(configuration);

        services.AddScoped<ITransactionsService, TransactionsService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUsersService, UsersService>();
        ConfigureAutoMapper(services);
    }
    private static void ConfigureAutoMapper(IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.AddProfile<MappingProfile>();
          
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}