using Authentication.Entities;
using Authentication.Entities.RequestParameters;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUser(User user);
        Task<bool> RegisterCompleteUser(User user);
        Task<User> Login(User user);
        Task<bool> ChangePassword(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> UpdateUserWithoutEmail(User user);
        ICollection<User> ListUsers(DoctorRequestParameters query);
        User FindById(int id);
        Task<IdentityUser> FindIdentityUserByName(string username);
        void DeleteUser(int id);
    }
}
