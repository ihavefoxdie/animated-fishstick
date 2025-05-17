using System;
using UserManagement.Models.DTOs;

namespace UserManagement.Api.Services.Interfaces;

/// <summary>
/// A user interface
/// </summary>
public interface IUserService
{   
    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="user">General user information</param>
    /// <param name="login">User login</param>
    /// <param name="password">User password</param>
    /// <param name="admin">Is this user going to be an admin</param>
    /// <param name="createdBy">Login of the user initiating the creation</param>
    /// <returns>A Task</returns>
    public Task CreateUser(UserDTO user, string login, string password, bool admin, string createdBy);


    /// <summary>
    /// Updates user information
    /// </summary>
    /// <param name="login">Login of the user whose details will be modified</param>
    /// <param name="name">New name</param>
    /// <param name="gender">New gender value</param>
    /// <param name="birthday">New birthday date</param>
    /// <param name="modifiedBy">Login of the user initiating the update</param>
    /// <returns>A UserDTO object corresponding to the new data.</returns>
    public Task<UserDTO?> UpdateUserInfo(string login, string name, int gender, string birthday, string modifiedBy);
    /// <summary>
    /// Updates user password
    /// </summary>
    /// <param name="login">Login of the user whose password will be modified</param>
    /// <param name="password">New password</param>
    /// <param name="modifiedBy">Login of the user initiating the update</param>
    /// <returns>A Task.</returns>
    public Task UpdateUserPassword(string login, string password, string modifiedBy);
    /// <summary>
    /// Updates user login
    /// </summary>
    /// <param name="login">Login of the user whose details will be modified</param>
    /// <param name="newLogin">New login</param>
    /// <param name="modifiedBy">Login of the user initiating the update</param>
    /// <returns>A Task.</returns>
    public Task UpdateLogin(string login, string newLogin, string modifiedBy);


    /// <summary>
    /// Gets all users who don't have their access revoked
    /// </summary>
    /// <returns>A list of found UserDTO objects</returns>
    public Task<List<UserDTO>> ReadAllActive();
    /// <summary>
    /// Gets found user with the specified login
    /// </summary>
    /// <param name="login">User login to search for</param>
    /// <returns>UserDTO corresponding to the user found; null if not found.</returns>
    public Task<UserDTO?> Read(string login);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="login">User login to search for</param>
    /// <param name="password">The password associated with the provided login to authenticate the user</param>
    /// <returns>An access token corresponding to the user's role and a UserDTO object corresponding to the found user.</returns>
    public Task<(string?, UserDTO?)> Login(string login, string password);
    /// <summary>
    /// Gets all users who are older then the age specified
    /// </summary>
    /// <param name="age">Age value</param>
    /// <returns>A list of found UserDTO objects</returns>
    public List<UserDTO> ReadAllOlder(int age);


    /// <summary>
    /// Revokes access of the specified user
    /// </summary>
    /// <param name="login">Login of the user for whom access will be revoked</param>
    /// <param name="revokedBy">Login of the user initiating the procedure</param>
    /// <returns>A UserDTO object corresponding to the user whose access has been revoked.</returns>
    public Task<UserDTO?> DeleteSoft(string login, string revokedBy);
    /// <summary>
    /// Completely removes the specified user from database
    /// </summary>
    /// <param name="login">Login of the user who is going to be removed</param>
    /// <returns>A UserDTO object corresponding to the user who has been removed.</returns>
    public Task<UserDTO?> DeleteHard(string login);


    /// <summary>
    /// Lifts access restrictions for the specified user
    /// </summary>
    /// <param name="login">Login of the user for whose access restrictions will be lifted.</param>
    /// <returns>A UserDTO object corresponding to the user whose restrictions have been lifted.</returns>
    public Task<UserDTO?> Restore(string login);
}
