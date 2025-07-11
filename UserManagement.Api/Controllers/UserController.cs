using Microsoft.AspNetCore.Mvc;
using UserManagement.Models.DTOs;
using UserManagement.Api.Authentication;
using Microsoft.AspNetCore.Authorization;
using UserManagement.Api.Services.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace UserManagement.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// A controller for user management
        /// </summary>
        /// <param name="userService">User service implementation to use</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }




        #region Post 
        /// <summary>
        /// Checks if this controller is functional
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public ActionResult Test()
        {
            return Ok();
        }

        /// <summary>
        /// Checks if admin authorization is working
        /// </summary>
        /// <returns>Result</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AdminTest()
        {
            return Ok();
        }

        /// <summary>
        /// Checks if user authorization is working
        /// </summary>
        /// <returns>Result</returns>
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult UserTest()
        {
            return Ok();
        }

        /// <summary>
        /// User creation method, only accessible to admins
        /// </summary>
        /// <param name="user">User object with basic information</param>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <param name="admin">Is the user going to be an admin</param>
        /// <returns>Response.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserDTO user, string login, string password, bool admin)
        {
            try
            {
                await _userService.CreateUser(user, login, password, admin, User.FindFirst(JwtRegisteredClaimNames.Nickname)!.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }
        #endregion


        #region Put
        /// <summary>
        /// Update method for changing name, gender and birthday of the user specified
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="name">A new name value</param>
        /// <param name="gender">A new gender value</param>
        /// <param name="birthday">A new birthday value</param>
        /// <returns>Response with the corresponding UserDTO object.</returns>
        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        public async Task<ActionResult<UserDTO>> UpdateInfo(string login, string name, int gender, string birthday)
        {
            try
            {
                if (!User.Claims.Any(c => c.Value == "Admin" || c.Value == login))
                {
                    return Unauthorized();
                }

                UserDTO? user = await _userService.UpdateUserInfo(login, name, gender, birthday, User.FindFirst(JwtRegisteredClaimNames.Nickname)!.Value);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        /// <summary>
        /// Update method for changing password of the user specified
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">A new password value</param>
        /// <returns>Response.</returns>
        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        public async Task<ActionResult> UpdatePassword(string login, string password)
        {
            try
            {
                if (!User.Claims.Any(c => c.Value == "Admin" || c.Value == login))
                {
                    return Unauthorized();
                }

                await _userService.UpdateUserPassword(login, password, User.FindFirst(JwtRegisteredClaimNames.Nickname)!.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        /// <summary>
        /// Update method for changing login of the user specified
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="newLogin">A new login value</param>
        /// <returns>Response.</returns>
        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        public async Task<ActionResult> UpdateLogin(string login, string newLogin)
        {
            try
            {
                if (!User.Claims.Any(c => c.Value == "Admin" || c.Value == login))
                {
                    return Unauthorized();
                }

                await _userService.UpdateLogin(login, newLogin, User.FindFirst(JwtRegisteredClaimNames.Nickname)!.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        /// <summary>
        /// Lifts user's access restrictions
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Response with the corresponding UserDTO object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<UserDTO>> Restore(string login)
        {
            try
            {
                UserDTO? user = await _userService.Restore(login);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }
        #endregion

        /// <summary>
        /// Returns every active user
        /// </summary>
        /// <returns>Response with the user object</returns>
        #region Get
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> ReadAllActive()
        {
            try
            {
                return Ok(await _userService.ReadAllActive());
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        /// <summary>
        /// Returns a found user by the specified login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Response with the corresponding UserDTO object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> Read(string login)
        {
            try
            {
                UserDTO? user = await _userService.Read(login);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        /// <summary>
        /// Returns every user older than the age specified
        /// </summary>
        /// <param name="age">Age value</param>
        /// <returns>Response with a list of UserDTO objects of the users found.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<List<UserDTO>> GetAllOlder(int age)
        {
            try
            {
                return Ok(_userService.ReadAllOlder(age));
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <returns>Response with a generated access token.</returns>
        [HttpGet]
        public async Task<ActionResult<string>> Login(string login, string password)
        {
            try
            {
                var result = await _userService.Login(login, password);
                string? token = result.Item1;
                UserDTO? user = result.Item2;

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(token);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }
        #endregion



        #region Delete
        /// <summary>
        /// Completely deletes the specified user
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Response with the corresponding UserDTO object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult<UserDTO>> DeleteHard(string login)
        {
            try
            {
                UserDTO? user = await _userService.DeleteHard(login);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        /// <summary>
        /// Revokes access of the user specified
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Response with the corresponding UserDTO object.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult<UserDTO>> DeleteSoft(string login)
        {
            try
            {
                UserDTO? user = await _userService.DeleteSoft(login, User.FindFirst(JwtRegisteredClaimNames.Nickname)!.Value);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }
        #endregion



        private ActionResult ReturnError(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        }
    }
}
