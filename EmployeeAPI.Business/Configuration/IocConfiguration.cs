using Employes.Feature.User.Interfaces;
using Employes.Feature.User.Services;

namespace Employes.Configuration;

public static class IocConfiguration
{
    public static void ConfigureIoc(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserService, UserService>();
    }
}