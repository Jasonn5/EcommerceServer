using Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Authentication.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> RegisterUser(User user);
        Task<User> Login(User user);
        Task<IdentityUser> FindIdentityUserByEmail(string email);
        Task<IdentityUser> FindIdentityUserByName(string username);
        Task<bool> ChangePassword(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> UpdateUserWithoutEmail(User user);
        Task<IList<string>> GetRoles(IdentityUser user);
        ICollection<User> List(string search);
        User FindById(int id);
        User FindByUsername(string username);
        void Delete(User doctor);
    }
}
