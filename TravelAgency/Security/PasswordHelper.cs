using BCrypt.Net;

namespace TravelAgency.Security
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.HashPassword(password, workFactor: 12);
        }

        public static bool VerifyPassword(
            string password,
            string passwordHash)
        {
            return BCrypt.Verify(password, passwordHash);
        }
    }
}