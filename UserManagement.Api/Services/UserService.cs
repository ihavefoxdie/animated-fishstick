using System;
using UserManagement.Api.Authentication;
using UserManagement.Api.Entities;
using UserManagement.Api.Repositories.Interfaces;
using UserManagement.Models.DTOs;

namespace UserManagement.Api.Services;

public class UserService
{
    private readonly IUserRepository<User> _userRepository;

    public UserService(IUserRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateUser(UserDTO user, string login, string password, bool admin, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.");
        }

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
            RevokedOn = user.RevokedOn,
            RevokedBy = null,
        };

        await _userRepository.Create(newUser);
    }

    public async Task<UserDTO?> UpdateUserInfo(string login, string name, int gender, string birthday)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Login is required.");
        }

        UserDTO? userDTO = null;

        User? foundUser = await _userRepository.Read(x => x.Login == login);

        if (foundUser != null)
        {
            foundUser = await _userRepository.Update(foundUser);
            if(foundUser != null)
            {
                userDTO = ConvertToDTO(foundUser);
            }
        }

        return userDTO;
    }

    public async Task UpdateUserPassword(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.");
        }

        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Login is required.");
        }

        string hashedPassword = PasswordHasher.HashPassword(password);

        User? foundUser = await _userRepository.Read(x => x.Login == login);

        if (foundUser != null)
        {
            foundUser.Password = hashedPassword;
            await _userRepository.Update(foundUser);
        }
    }

    public async Task UpdateLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Login is required.");
        }

        User? foundUser = await _userRepository.Read(x => x.Login == login);

        if (foundUser != null)
        {
            foundUser.Login = login;
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

    public async Task<UserDTO?> Login(string login, string password)
    {
        UserDTO? userDTO = null;
        User? user = await _userRepository.Read(x => x.Login == login);

        if (user != null)
        {
            bool check = PasswordHasher.VerifyPassword(password, user.Password);
            if (check)
            {
                userDTO = ConvertToDTO(user);
            }
        }

        return userDTO;
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

    public async Task<UserDTO?> DeleteSoft(string login)
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
}
