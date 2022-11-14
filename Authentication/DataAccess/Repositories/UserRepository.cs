using Authentication.DataAccess.Interfaces;
using Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IPasswordHasher<IdentityUser> passwordHasher;
        private readonly IConfiguration configuration;
        private readonly IdentityDbContext context;

        public UserRepository(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IPasswordHasher<IdentityUser> passwordHasher,
            IConfiguration configuration,
            IdentityDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
            this.configuration = configuration;
            this.context = context;
        }

        public ICollection<User> List(string search)
        {
            var result = context.Set<User>().FromSqlRaw($"dbo.GetDoctors '{search}'").AsEnumerable().ToList();

            return result;
        }

        public async Task<bool> ChangePassword(User user)
        {
            var identityUser = await userManager.FindByNameAsync(user.Username);

            if (identityUser != null)
            {
                if (passwordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, user.OldPassword) != PasswordVerificationResult.Failed)
                {
                    var result = await userManager.ChangePasswordAsync(identityUser, user.OldPassword, user.Password);

                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("La contraseña debe contener al menos una letra mayúscula.");
                    }
                    else
                    {
                        return result.Succeeded;
                    }
                }
                else
                {
                    throw new ApplicationException("La contraseña anterior es incorrecta.");
                }
            }
            else
            {
                throw new ApplicationException("Usuario no encontrado.");
            }
        }

        public User FindById(int id)
        {
            return context.Set<User>().Find(id);
        }

        public User FindByUsername(string username)
        {
            return context.Set<User>().SingleOrDefault(u => u.Username == username);
        }

        public async Task<IdentityUser> FindIdentityUserByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityUser> FindIdentityUserByName(string username)
        {
            return await userManager.FindByNameAsync(username);
        }

        public async Task<IList<string>> GetRoles(IdentityUser user)
        {
            var roles = userManager.GetRolesAsync(user);

            return await roles;
        }

        public async Task<User> Login(User user)
        {
            var result = await signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);
            var identityUser = await FindIdentityUserByName(user.Username);

            if (result.Succeeded)
            {
                var appUser = userManager.Users.SingleOrDefault(r => r.UserName == user.Username);
                var userToClaim = FindByUsername(user.Username);
                user.Token = await GenerateJwtToken(userToClaim, appUser);
                await userManager.ResetAccessFailedCountAsync(identityUser);

                return user;
            }
            else
            {
                if (userManager.IsLockedOutAsync(identityUser).Result)
                {
                    var lockoutEndDateOffset = await userManager.GetLockoutEndDateAsync(identityUser);
                    DateTime lockoutEndDate = Convert.ToDateTime(lockoutEndDateOffset.ToString());
                    DateTime currentDate = DateTime.Now;
                    TimeSpan lockoutEndtime = (lockoutEndDate - currentDate);

                    throw new ApplicationException("Cuenta bloqueada temporalmente, intente nuevamente en " + ((int)lockoutEndtime.TotalMinutes + 1) + " minutos.");
                }
                else
                {
                    await userManager.AccessFailedAsync(identityUser);

                    throw new ApplicationException("Nombre de usuario o contraseña incorrecta.");
                }
            }
        }

        public async Task<bool> RegisterUser(User user)
        {
            var newUser = new IdentityUser
            {
                UserName = user.Username,
                Email = user.Email
            };

            var result = await userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                context.Set<User>().Add(user);
                context.SaveChanges();

                if (user.Roles != null)
                {
                    foreach (var role in user.Roles)
                    {
                        await userManager.AddToRoleAsync(newUser, role.Name);
                    }
                }
            }

            return result.Succeeded;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var identityUser = userManager.FindByNameAsync(user.Username).Result;
            var roles = GetRoles(identityUser);

            await userManager.RemoveFromRolesAsync(identityUser, roles.Result);

            if (user.Password != null)
            {
                identityUser.PasswordHash = userManager.PasswordHasher.HashPassword(identityUser, user.Password);
            }
            identityUser.UserName = user.Username;
            identityUser.Email = user.Email;
            var result = userManager.UpdateAsync(identityUser).Result;

            if (result.Succeeded)
            {
                if (user.Roles != null)
                {
                    foreach (var role in user.Roles)
                    {
                        await userManager.AddToRoleAsync(identityUser, role.Name);
                    }
                }

                var userToUpdate = context.Set<User>().Find(user.Id);

                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.Username = user.Username;
                userToUpdate.Email = user.Email;
                userToUpdate.GivenName = user.FirstName + ' ' + user.LastName;
                userToUpdate.IsEnabled = user.IsEnabled;
                context.SaveChanges();
            }

            return result.Succeeded;
        }

        public async Task<bool> UpdateUserWithoutEmail(User user)
        {
            var identityUser = userManager.FindByNameAsync(user.Username).Result;
            var roles = GetRoles(identityUser);
            await userManager.RemoveFromRolesAsync(identityUser, roles.Result);
            var result = new IdentityResult();

            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    result = await userManager.AddToRoleAsync(identityUser, role.Name);
                }
            }

            if (result.Succeeded)
            {
                var userToUpdate = context.Set<User>().Find(user.Id);

                userToUpdate.IsEnabled = user.IsEnabled;
                context.SaveChanges();
            }

            return result.Succeeded;
        }

        private async Task<string> GenerateJwtToken(User user, IdentityUser identityUser)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                new Claim("user_id", user.Id.ToString()),
                new Claim("user_email", user.Email)
            };

            var roles = GetRoles(identityUser);

            foreach (var role in roles.Result)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtExpireMinutes"]));

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void Delete(User doctor)
        {
            var identityUser = userManager.FindByNameAsync(doctor.Username).Result;
            context.Set<IdentityUser>().Remove(identityUser);
            context.Set<User>().Remove(doctor);
            context.SaveChanges();
        }
    }
}
