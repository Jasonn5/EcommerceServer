using Authentication.Entities;
using Authentication.Entities.RequestParameters;
using Authentication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userService.RegisterUser(user);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("complete-user")]
        public async Task<ActionResult<User>> PostCompleteUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userService.RegisterCompleteUser(user);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] User user)
        {
            var authUser = await userService.Login(user);

            if (authUser == null)
                return BadRequest(new { message = "Username or password is incorrect." });

            return Ok(authUser);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            return Ok(userService.FindById(id));
        }

        [HttpPost]
        [Route("changepassword")]
        public async Task<ActionResult> ChangePassword([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }

            var result = await userService.ChangePassword(user);

            return Ok(user);
        }

        [HttpGet]
        [Route("")]
        public ActionResult<ICollection<User>> Get([FromQuery] DoctorRequestParameters query)
        {
            return Ok(userService.ListUsers(query));
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<User>> Update(User user)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }

            var result = await userService.UpdateUser(user);

            return Ok(user);
        }

        [HttpPatch]
        [Route("without-email/{id}")]
        public async Task<ActionResult<User>> UpdateWithoutEmail(User user)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToArray());

                return BadRequest(errors);
            }

            var result = await userService.UpdateUserWithoutEmail(user);

            return Ok(user);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                userService.DeleteUser(id);

                return Ok();
            }
            catch (Exception exception)
            {
                throw new ApplicationException("El Doctor no puede ser eliminado por que contiene informacion relacionada con el sistema");
            }
        }
    }
}
