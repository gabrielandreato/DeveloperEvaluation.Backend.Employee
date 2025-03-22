using Employes.DataLibrary.Context;
using Employes.Feature.User.Interfaces;
using Employes.Feature.User.Services;
using Microsoft.EntityFrameworkCore;

namespace Employes.Configuration;

public static class IocConfiguration
{
    public static void ConfigureIoc(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserService, UserService>();
    }
    
    
    public static void Migrate(WebApplication webApplication)
    {
        int maxRetries = 5;
        int secondsDelay = 20;
        using (var scope = webApplication.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<EmployeeDbContext>();
            var retries = maxRetries;
            do
            {
                try
                {
                    context.Database.Migrate();
                    break; 
                }
                catch (Exception ex)
                {
                    retries--;
                    Task.Delay(secondsDelay * 1000).Wait();  
                }
            } while (retries > 0);
        }
    }
}