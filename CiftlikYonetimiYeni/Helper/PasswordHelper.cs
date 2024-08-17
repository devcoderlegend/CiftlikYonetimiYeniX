using System;
using System.Security.Cryptography;
using System.Text;

namespace CiftlikYonetimiYeni.Helper
{
    public static class PasswordHelper
    {
        // Şifreyi hashler ve salt'ı ile birlikte döner
        public static string HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = hmac.Key;
                var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Salt ve Hash'i birlikte saklıyoruz
                var hashBytes = new byte[salt.Length + hashedPassword.Length];
                Array.Copy(salt, 0, hashBytes, 0, salt.Length);
                Array.Copy(hashedPassword, 0, hashBytes, salt.Length, hashedPassword.Length);

                return Convert.ToBase64String(hashBytes);
            }
        }

        // Şifreyi doğrular (Veritabanında saklanan salt'ı kullanır)
        public static bool VerifyPassword(string password, string storedHash)
        {
            var hashBytes = Convert.FromBase64String(storedHash);

            // Hash'in ilk kısmı salt, kalan kısmı şifrenin hash'i
            var salt = new byte[64];
            Array.Copy(hashBytes, 0, salt, 0, 64);

            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Hash'in kalan kısmını kontrol et
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (hashBytes[i + 64] != computedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
