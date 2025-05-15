using System;
using BCrypt.Net;

namespace UserManagement.Api.Authentication;

public static class PasswordHasher
{
    /// <summary>
    /// Password hasher, using Bcrypt
    /// </summary>
    /// <param name="password">Password to hash</param>
    /// <returns></returns>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, 12);
    }

    /// <summary>
    /// Compare passed password with a hashed one
    /// </summary>
    /// <param name="password">Passed in password</param>
    /// <param name="hashedPassword">Hashed password</param>
    /// <returns></returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
