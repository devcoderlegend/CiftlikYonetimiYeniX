using System;
using System.Security.Cryptography;
using System.Text;

namespace CiftlikYonetimiYeni.Helper
{
    public static class PasswordHelper
    {
        // MD5 ile şifreyi hash'ler ve hexadecimal formatında döndürür
        public static string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); // Hexadecimal formatta döndürür
            }
        }

        // Hash'lenmiş şifreyi doğrular
        public static bool VerifyPassword(string password, string storedHash)
        {
            string computedHash = HashPassword(password);
            return storedHash == computedHash;
        }
    }
}
