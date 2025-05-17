using Microsoft.AspNetCore.Mvc;
using UserManagement.Models.DTOs;
using UserManagement.Api.Authentication;
using Microsoft.AspNetCore.Authorization;
using UserManagement.Api.Services.Interfaces;

namespace UserManagement.Api.Controllers
{
    //TODO: use bcrypt
    //TODO: implement jwt functionality
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }




        #region Post 
        [HttpPost]
        public ActionResult Test()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AuthTest()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserDTO user, string login, string password, bool admin, string createdBy)
        {
            try
            {
                await _userService.CreateUser(user, login, password, admin, createdBy);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }

            return Ok();
        }
        #endregion


        #region Put
        /// <summary>
        /// Update method for changing name, gender and birthday of the user specified
        /// </summary>
        /// <param name="guid">User Guid</param>
        /// <param name="name">A new name value</param>
        /// <param name="gender">A new gender value</param>
        /// <param name="birthday">A new birthday value</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<UserDTO>> UpdateInfo(string login, string name, int gender, string birthday)
        {
            try
            {
                UserDTO? user = await _userService.UpdateUserInfo(login, name, gender, birthday);

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
        /// <param name="guid">User Guid</param>
        /// <param name="password">A new password value</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> UpdatePassword(string login, string password)
        {
            try
            {
                await _userService.UpdateUserPassword(login, password);
                return Ok();
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        /// <summary>
        /// Update method for changing lgoin of the user specified
        /// </summary>
        /// <param name="guid">User Guid</param>
        /// <param name="login">A new login value</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> UpdateLogin(string login)
        {
            try
            {
                await _userService.UpdateLogin(login);
                return Ok();
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

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

                return Ok();
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }
        #endregion


        #region Get
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> Read(string login)
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

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult<UserDTO>> DeleteSoft(string login)
        {
            try
            {
                UserDTO? user = await _userService.DeleteSoft(login);

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
