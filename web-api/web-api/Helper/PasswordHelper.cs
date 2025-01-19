using System.Security.Cryptography;
using System.Text;

namespace web_api.Helper
{
    public static class PasswordHelper
    {
        public static byte[] GenerateVarbinary64FromPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                return sha256.ComputeHash(passwordBytes);
            }
        }

        public static bool ComparePasswords(byte[] passwordHash, string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                var hashBytes = sha256.ComputeHash(passwordBytes);

                for (var i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != hashBytes[i])
                        return false;
                }

                return true;
            }
        }
    }
}
