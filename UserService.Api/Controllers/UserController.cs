using UserService.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using UserService.Models.DTOs;
using UserService.Api.Authentication;

namespace UserService.Api.Controllers
{
    //TODO: use bcrypt
    //TODO: implement jwt functionality
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User> _userRepository;

        public UserController(IUserRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        #region Post 
        [HttpPost]
        public ActionResult Test()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string login, string password, string name, int gender, string birthday, bool admin)
        {
            try
            {
                User user = new()
                {
                    Login = login,
                    Password = PasswordHasher.HashPassword(password),
                    Name = name,
                    Gender = gender,
                    Birthday = DateTime.UtcNow,
                    Admin = admin,
                    CreatedBy = string.Empty,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    ModifiedBy = string.Empty,
                };


                await _userRepository.Create(user);
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
        public async Task<ActionResult> UpdateInfo(Guid guid, string name, int gender, string birthday)
        {
            return await ProcessUpdate(guid, name: name, gender: gender, birtday: DateTime.Parse(birthday));
        }

        /// <summary>
        /// Update method for changing password of the user specified
        /// </summary>
        /// <param name="guid">User Guid</param>
        /// <param name="password">A new password value</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> UpdatePassword(Guid guid, string password)
        {
            return await ProcessUpdate(guid, password: PasswordHasher.HashPassword(password));
        }

        /// <summary>
        /// Update method for changing lgoin of the user specified
        /// </summary>
        /// <param name="guid">User Guid</param>
        /// <param name="login">A new login value</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> UpdateLogin(Guid guid, string login)
        {
            return await ProcessUpdate(guid, login: login);
        }

        /// <summary>
        /// General update method for processing changes to any values passed for the specified user
        /// </summary>
        /// <param name="guid">User Guid</param>
        /// <param name="login">A new login value</param>
        /// <param name="password">A new password value</param>
        /// <param name="name">A new name value</param>
        /// <param name="gender">A new gender value</param>
        /// <param name="birthday">A new birthday value</param>
        /// <param name="admin">A new admin value</param>
        /// <param name="createdOn">A new created date value</param>
        /// <param name="createdBy">A new creator name value</param>
        /// <param name="modifiedOn">A new modified date value</param>
        /// <param name="modifiedBy">A new modifier name value</param>
        /// <param name="revokedOn">A new revoked date value</param>
        /// <param name="revokedBy">A new revoker name value</param>
        /// <returns></returns>
        private async Task<ActionResult> ProcessUpdate(Guid guid, string? login = null, string? password = null, string? name = null, int? gender = null,
        DateTime? birtday = null, bool? admin = null, DateTime? createdOn = null, string? createdBy = null, DateTime? modifiedOn = null, string? modifiedBy = null,
        DateTime? revokedOn = null, string? revokedBy = null)
        {
            try
            {
                User? userToUpdate = await _userRepository.Read(x => x.Guid == guid);

                if (userToUpdate == null)
                {
                    return NotFound();
                }

                userToUpdate.Login = login ?? userToUpdate.Login;
                userToUpdate.Password = password ?? userToUpdate.Password;
                userToUpdate.Name = name ?? userToUpdate.Name;
                userToUpdate.Gender = gender ?? userToUpdate.Gender;
                userToUpdate.Birthday = birtday ?? userToUpdate.Birthday;
                userToUpdate.Admin = admin ?? userToUpdate.Admin;
                userToUpdate.CreatedOn = createdOn ?? userToUpdate.CreatedOn;
                userToUpdate.CreatedBy = createdBy ?? userToUpdate.CreatedBy;
                userToUpdate.ModifiedOn = modifiedOn ?? userToUpdate.ModifiedOn;
                userToUpdate.ModifiedBy = modifiedBy ?? userToUpdate.ModifiedBy;
                userToUpdate.RevokedOn = revokedOn ?? userToUpdate.RevokedOn;
                userToUpdate.RevokedBy = revokedBy ?? userToUpdate.RevokedBy;

                if (await _userRepository.Update(userToUpdate) == null)
                {
                    return BadRequest("Item not found");
                }
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }

            return Ok();
        }
        #endregion


        #region Get
        [HttpGet]
        public async Task<ActionResult> ReadAllActive()
        {
            try
            {
                Task<List<User>> users = Task.Run(() => _userRepository.ReadAll(x => x.RevokedOn == null).OrderBy(x => x.CreatedOn).ToList());
                await users;

                List<UserDTO> usersDTO = [];

                foreach (User user in users.Result)
                {
                    usersDTO.Add(ConvertToDTO(user));
                }

                return Ok(usersDTO);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Read(string login)
        {
            try
            {
                User? user = await _userRepository.Read(x => x.Login == login);
                if (user == null)
                {
                    return NotFound();
                }

                UserDTO userDTO = ConvertToDTO(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Login(string login, string password)
        {
            try
            {
                User? user = await _userRepository.Read(x => x.Login == login && x.RevokedOn == null);
                if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
                {
                    return NotFound();
                }

                return Ok(ConvertToDTO(user));
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }
        #endregion


        #region Delete
        [HttpDelete]
        public async Task<ActionResult> DeleteHard(string login)
        {
            try
            {
                User? user = await _userRepository.Read(x => x.Login == login);
                if (user == null)
                {
                    return NotFound();
                }

                await _userRepository.Delete(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteSoft(string login)
        {
            try
            {
                User? user = await _userRepository.Read(x => x.Login == login);
                if (user == null)
                {
                    return NotFound();
                }

                user.RevokedOn = DateTime.Now;
                await _userRepository.Update(user);

                return Ok();
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

        //TODO: factory method?
        private static UserDTO ConvertToDTO(User user)
        {
            return new UserDTO
            {
                Guid = user.Guid,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy,
                ModifiedOn = user.ModifiedOn,
                ModifiedBy = user.ModifiedBy,
                RevokedOn = user.RevokedOn,
                RevokedBy = user.RevokedBy,
            };
        }
    }
}
