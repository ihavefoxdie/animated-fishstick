using UserManagement.Utils;
using UserManagement.Api.Authentication;
using UserManagement.Api.Entities;
using UserManagement.Api.Repositories.Interfaces;
using UserManagement.Api.Services.Interfaces;
using UserManagement.Models.DTOs;
using System.Text.RegularExpressions;

namespace UserManagement.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository<User> _userRepository;
    private readonly JWTAuth _jWTAuth;

    public UserService(IUserRepository<User> userRepository, JWTAuth jWTAuth)
    {
        _userRepository = userRepository;
        _jWTAuth = jWTAuth;
    }




    public async Task CreateUser(UserDTO user, string login, string password, bool admin, string createdBy)
    {
        CheckValues(user.Name, login, password);

        Regex regex = RegularExpressions.GetLatinAndNumbers();
        Regex regex1 = RegularExpressions.GetLatinAndCyrillic();
        CheckRegexCompliance(regex, login, password);
        CheckRegexCompliance(regex1, user.Name);

        string hashedPassword = PasswordHasher.HashPassword(password);

        User newUser = new()
        {
            Guid = Guid.NewGuid(),
            Login = login,
            Password = hashedPassword,
            Name = user.Name,
            Gender = user.Gender,
            Birthday = user.Birthday,
            Admin = admin,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = createdBy,
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = createdBy,
            RevokedOn = null,
            RevokedBy = null,
        };

        await _userRepository.Create(newUser);
    }




    public async Task<UserDTO?> UpdateUserInfo(string login, string name, int gender, string birthday, string modifiedBy)
    {
        CheckValues(login, name);

        Regex regex = RegularExpressions.GetLatinAndCyrillic();
        CheckRegexCompliance(regex, name);

        UserDTO? userDTO = null;
        User? foundUser = await _userRepository.Read(x => x.Login == login);

        if (foundUser != null)
        {
            CheckAccess(foundUser);

            foundUser.Name = name;
            foundUser.Gender = gender;
            foundUser.Birthday = DateTime.Parse(birthday).ToUniversalTime();
            foundUser.ModifiedBy = modifiedBy;
            foundUser.ModifiedOn = DateTime.UtcNow;

            foundUser = await _userRepository.Update(foundUser);
            if (foundUser != null)
            {
                userDTO = ConvertToDTO(foundUser);
            }
        }

        return userDTO;
    }

    public async Task UpdateUserPassword(string login, string password, string modifiedBy)
    {
        CheckValues(login, password);

        Regex regex = RegularExpressions.GetLatinAndNumbers();
        CheckRegexCompliance(regex, password);

        User? foundUser = await _userRepository.Read(x => x.Login == login);

        if (foundUser != null)
        {
            CheckAccess(foundUser);

            string hashedPassword = PasswordHasher.HashPassword(password);
            foundUser.Password = hashedPassword;
            foundUser.ModifiedBy = modifiedBy;
            foundUser.ModifiedOn = DateTime.UtcNow;

            await _userRepository.Update(foundUser);
        }
    }

    public async Task UpdateLogin(string login, string newLogin, string modifiedBy)
    {
        CheckValues(login, newLogin);

        Regex regex = RegularExpressions.GetLatinAndNumbers();
        CheckRegexCompliance(regex, login, newLogin);

        User? foundUser = await _userRepository.Read(x => x.Login == login);

        if (foundUser != null)
        {
            CheckAccess(foundUser);

            foundUser.Login = newLogin;
            foundUser.ModifiedBy = modifiedBy;
            foundUser.ModifiedOn = DateTime.UtcNow;

            await _userRepository.Update(foundUser);
        }
    }




    public async Task<List<UserDTO>> ReadAllActive()
    {
        Task<List<User>> users = Task.Run(() => _userRepository.ReadAll(x => x.RevokedOn == null).OrderBy(x => x.CreatedOn).ToList());
        await users;

        List<UserDTO> usersDTO = [];

        foreach (User user in users.Result)
        {
            usersDTO.Add(ConvertToDTO(user));
        }

        return usersDTO;
    }

    public async Task<UserDTO?> Read(string login)
    {
        UserDTO? userDTO = null;
        User? user = await _userRepository.Read(x => x.Login == login);

        if (user != null)
        {
            userDTO = ConvertToDTO(user);
        }

        return userDTO;
    }

    public async Task<(string?, UserDTO?)> Login(string login, string password)
    {
        UserDTO? userDTO = null;
        User? foundUser = await _userRepository.Read(x => x.Login == login);
        string? token = null;

        if (foundUser != null)
        {
            CheckAccess(foundUser);

            bool check = PasswordHasher.VerifyPassword(password, foundUser.Password);
            if (check)
            {
                userDTO = ConvertToDTO(foundUser);
                token = _jWTAuth.CreateToken(foundUser);
            }
        }

        return (token, userDTO);
    }

    public List<UserDTO> ReadAllOlder(int age)
    {
        IEnumerable<User> users = _userRepository.ReadAll(x =>
        {
            if (x.Birthday != null && (DateTime.UtcNow.Year - x.Birthday.Value.Year) > age)
            {
                return true;
            }
            return false;
        });

        List<UserDTO> usersDTO = [];

        foreach (User user in users)
        {
            usersDTO.Add(ConvertToDTO(user));
        }

        return usersDTO;
    }




    public async Task<UserDTO?> DeleteSoft(string login, string revokedBy)
    {
        User? user = await _userRepository.Read(x => x.Login == login);
        UserDTO? userDTO = null;

        if (user != null)
        {
            user.RevokedOn = DateTime.UtcNow;

            if (await _userRepository.Update(user) != null)
            {
                userDTO = ConvertToDTO(user);
            }
        }

        return userDTO;
    }

    public async Task<UserDTO?> DeleteHard(string login)
    {
        User? user = await _userRepository.Read(x => x.Login == login);
        UserDTO? userDTO = null;


        if (user != null)
        {
            if (await _userRepository.Delete(user) != null)
            {
                userDTO = ConvertToDTO(user);
            }
        }

        return userDTO;
    }




    public async Task<UserDTO?> Restore(string login)
    {
        CheckValues(login);

        User? user = await _userRepository.Read(x => x.Login == login);
        UserDTO? userDTO = null;

        if (user != null)
        {
            user.RevokedOn = null;
            user.RevokedBy = null;

            if (await _userRepository.Update(user) != null)
            {
                userDTO = ConvertToDTO(user);
            }
        }

        return userDTO;
    }


    private static UserDTO ConvertToDTO(User user)
    {
        return new UserDTO
        {
            //Guid = user.Guid,
            //Login = user.Login,
            //Password = user.Password,
            Name = user.Name,
            Gender = user.Gender,
            Birthday = user.Birthday,
            //Admin = user.Admin,
            //CreatedOn = user.CreatedOn,
            //CreatedBy = user.CreatedBy,
            //ModifiedOn = user.ModifiedOn,
            //ModifiedBy = user.ModifiedBy,
            RevokedOn = user.RevokedOn,
            //RevokedBy = user.RevokedBy,
        };
    }

    private static void CheckValues(params string[] values)
    {
        foreach (string value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("One or more arguments are incorrect.");
            }
        }
    }

    private static void CheckRegexCompliance(Regex regex, params string[] values)
    {
        foreach (string value in values)
        {
            if (!regex.Match(value).Success)
            {
                throw new ArgumentException("One or more arguments are incorrect.");
            }
        }
    }

    private static void CheckAccess(User user)
    {
        if (user.RevokedOn != null)
        {
            throw new UnauthorizedAccessException("This user's access has been revoked!");
        }
    }
}
