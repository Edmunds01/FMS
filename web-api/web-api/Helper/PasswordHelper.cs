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

                var hashBytes = sha256.ComputeHash(passwordBytes);

                var varbinary64 = new byte[64];
                Buffer.BlockCopy(hashBytes, 0, varbinary64, 0, hashBytes.Length);

                return varbinary64;
            }
        }
    }
}
