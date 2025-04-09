using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace web_api.Helper;

public static class PasswordHelper
{
    public static byte[] GenerateVarbinary64FromPassword(string password)
    {
        ValidatePassword(password);

        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        return sha256.ComputeHash(passwordBytes);
    }

    public static bool ComparePasswords(byte[] passwordHash, string password)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var hashBytes = sha256.ComputeHash(passwordBytes);

        for (var i = 0; i < passwordHash.Length; i++)
        {
            if (passwordHash[i] != hashBytes[i])
            {
                return false;
            }
        }

        return true;
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty or whitespace.");
        }

        if (password.Length < 10)
        {
            throw new ArgumentException("Password must be at least 10 characters long.");
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            throw new ArgumentException("Password must contain at least one uppercase letter.");
        }

        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            throw new ArgumentException("Password must contain at least one lowercase letter.");
        }

        if (!Regex.IsMatch(password, @"\d"))
        {
            throw new ArgumentException("Password must contain at least one number.");
        }

        if (!Regex.IsMatch(password, @"[\W_]"))
        {
            throw new ArgumentException("Password must contain at least one symbol.");
        }
    }
}
