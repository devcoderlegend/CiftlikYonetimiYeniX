using System.Security.Cryptography;
using System.Text;

public static class JwtSettingsx
{
    public static string HashPassword(string password)
    {
        using (var sha1 = SHA1.Create())
        {
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }

    public static bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        var hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == hashedPassword;
    }
}