using Authentication.DataAccess.Interfaces;
using Authentication.Entities;
using Authentication.Entities.RequestParameters;
using Authentication.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;
        private readonly IPasswordService passwordService;
        private readonly IHostingEnvironment env;
        private readonly IConfiguration configuration;

        public UserService(
            IUserRepository userRepository,
            IEmailService emailService,
            IPasswordService passwordService,
            IHostingEnvironment env,
            IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
            this.passwordService = passwordService;
            this.env = env;
            this.configuration = configuration;
        }

        public Task<bool> ChangePassword(User user)
        {
            return userRepository.ChangePassword(user);
        }

        public void DeleteUser(int id)
        {
            var doctor = userRepository.FindById(id);

            if (doctor != null)
            {
                userRepository.Delete(doctor);
            }
        }

        public User FindById(int id)
        {
            var user = userRepository.FindById(id);
            var identityUser = userRepository.FindIdentityUserByName(user.Username).Result;
            var roles = userRepository.GetRoles(identityUser).Result;
            user.Roles = roles.Select(r => new Role { Name = r }).ToList();

            return user;
        }

        public Task<IdentityUser> FindIdentityUserByName(string username)
        {
            return userRepository.FindIdentityUserByName(username);
        }

        public ICollection<User> ListUsers(DoctorRequestParameters query)
        {
            var users = userRepository.List(query.Search);
            var userList = new List<User>();

            foreach (var user in users)
            {
                var identityUser = userRepository.FindIdentityUserByName(user.Username).Result;
                var roles = userRepository.GetRoles(identityUser).Result;
                user.Roles = roles.Select(r => new Role { Name = r }).ToList();
                userList.Add(user);
            }

            return userList;
        }

        public Task<User> Login(User user)
        {
            var registeredUser = userRepository.FindByUsername(user.Username);

            if (registeredUser != null)
            {
                if (registeredUser.IsEnabled)
                {
                    return userRepository.Login(user);
                }
                else
                {
                    throw new ApplicationException("Cuenta de usuario deshabilitada.");
                }
            }
            else
            {
                throw new ApplicationException("Nombre de usuario o contraseña incorrecta.");
            }
        }

        public Task<bool> RegisterCompleteUser(User user)
        {
            user.Username = user.Username;
            user.GivenName = user.FirstName + ' ' + user.LastName;
            user.Password = user.Password;
            user.IsEnabled = true;

            if (FindIdentityUserByName(user.Username).Result != null)
            {
                throw new ApplicationException("No se puede registrar al usuario, el correo electronico ya está en uso");
            }
            else
            {
                var userCreated = userRepository.RegisterUser(user);

                return userCreated;
            }
        }

        public async Task<bool> RegisterUser(User user)
        {
            var identityUser = userRepository.FindIdentityUserByEmail(user.Email);

            if (identityUser.Result == null)
            {
                user.Username = user.FirstName[0].ToString().ToLower() + user.LastName.ToLower();
                user.GivenName = user.FirstName + ' ' + user.LastName;
                user.Password = passwordService.GeneratePassword(8);
                user.IsEnabled = true;

                var cont = 1;
                var usernameVerified = user.Username;
                while (FindIdentityUserByName(usernameVerified).Result != null)
                {
                    usernameVerified = user.Username + cont;
                    cont++;
                }
                user.Username = usernameVerified;

                var userCreated = userRepository.RegisterUser(user);

                if (userCreated.Result)
                {
                    var webRoot = env.WebRootPath;
                    var pathToFile = env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplate"
                            + Path.DirectorySeparatorChar.ToString()
                            + configuration["WelcomeEmailTemplate"];
                    var builder = new BodyBuilder();

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        builder.HtmlBody = SourceReader.ReadToEnd();
                    }

                    string htmlBody = builder.HtmlBody
                        .Replace("{", "{{")
                        .Replace("}", "}}")
                        .Replace("#username#", user.Username)
                        .Replace("#password#", user.Password);

                    var email = new Email
                    {
                        Subject = "Bienvenido!",
                        Message = string.Format(htmlBody),
                        ToAddresses = user.Email,
                        IsBodyHtml = true
                    };

                    emailService.SendEmail(email);

                    return userCreated.Result;
                }
                else
                {
                    throw new ApplicationException("No se puede registrar al usuario, el nombre de usuario ya está en uso");
                }
            }
            else
            {
                throw new ApplicationException("No se puede registrar al usuario, el email ya está en uso");
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            var identityUser = userRepository.FindIdentityUserByEmail(user.Email);
            var registeredUser = userRepository.FindByUsername(user.Username);

            if (identityUser.Result == null || user.Email == registeredUser.Email)
            {
                if (user.Email != registeredUser.Email)
                {
                    user.Password = passwordService.GeneratePassword(8);
                }
                var result = await userRepository.UpdateUser(user);

                if (result && user.Password != null)
                {
                    var email = new Email
                    {
                        Subject = "Bienvenido!",
                        Message = string.Format("<p>Tu nombre de usuario es: {0}</p><p>Tu contraseña es: {1}</p>", user.Username, user.Password),
                        ToAddresses = user.Email,
                        IsBodyHtml = true
                    };

                    emailService.SendEmail(email);
                }

                return result;
            }
            else
            {
                throw new ApplicationException("No se puede actualizar al usuario, el correo electrónico ya está en uso");
            }
        }

        public async Task<bool> UpdateUserWithoutEmail(User user)
        {
            var registeredUser = userRepository.FindByUsername(user.Username);

            if (registeredUser != null)
            {
                var result = await userRepository.UpdateUserWithoutEmail(user);

                return result;
            }
            else
            {
                throw new ApplicationException("El usuario no esta registrado");
            }
        }
    }
}
