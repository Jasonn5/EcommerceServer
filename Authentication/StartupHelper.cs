using Authentication.DataAccess.Interfaces;
using Authentication.DataAccess.Repositories;
using Authentication.Services;
using Authentication.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication
{
    public class StartupHelper
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
