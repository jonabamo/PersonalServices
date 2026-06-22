using BCrypt.Net;

namespace UserAPI.Common
{
    public class Utils
    {
        public static string SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be empty or default!", nameof(password));
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool ValidPassword(string userInputPassword, string storedHashFromDb)
        {
            if (string.IsNullOrWhiteSpace(userInputPassword)) return false;
            if (string.IsNullOrWhiteSpace(storedHashFromDb)) return false;

            return BCrypt.Net.BCrypt.Verify(userInputPassword, storedHashFromDb);
        }
    }
}
