using Employes.DataLibrary.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelLibrary.Entities;

namespace Employes.DataLibrary;

public static class ConfigureDataLibraryDependencies
{
    public static void ConfigureDataLibrary(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbContext(services, configuration);
    }

    private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<EmployesDbContext>(opts =>
        {
            opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                mysqlOptions =>
                {
                    mysqlOptions.MigrationsAssembly("EmployeeAPI.DataLibrary");
                });
        });



        services.AddScoped<IEmployesDataContext, EmployesDbContext>();

        services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<EmployesDbContext>()
            .AddDefaultTokenProviders();
    }
}