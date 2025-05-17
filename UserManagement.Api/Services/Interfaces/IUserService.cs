using System;
using UserManagement.Models.DTOs;

namespace UserManagement.Api.Services.Interfaces;

public interface IUserService
{
    public Task CreateUser(UserDTO user, string login, string password, bool admin, string createdBy);


    public Task<UserDTO?> UpdateUserInfo(string login, string name, int gender, string birthday);
    public Task UpdateUserPassword(string login, string password);
    public Task UpdateLogin(string login);


    public Task<List<UserDTO>> ReadAllActive();
    public Task<UserDTO?> Read(string login);
    public Task<(string?, UserDTO?)> Login(string login, string password);
    public List<UserDTO> ReadAllOlder(int age);


    public Task<UserDTO?> DeleteSoft(string login);
    public Task<UserDTO?> DeleteHard(string login);


    public Task<UserDTO?> Restore(string login);
}
