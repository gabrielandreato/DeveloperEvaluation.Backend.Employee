using Employes.DataLibrary.Context;
using Employes.DataLibrary.Repository;
using Employes.DataLibrary.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Employes.DataLibrary;

public static class ConfigureDataLibraryDependencies
{
    public static void ConfigureDataLibrary(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbContext(services, configuration);
        ConfigureRepositories(services);
    }

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
    }

    private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<IEmployeeDataContext,EmployeeDbContext>(opts =>
            {
                opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    mysqlOptions =>
                    {
                        mysqlOptions.MigrationsAssembly("EmployeeAPI.DataLibrary");

                    });
            opts.EnableSensitiveDataLogging();
        });

    }
}