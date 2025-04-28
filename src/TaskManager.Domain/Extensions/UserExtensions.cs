
namespace TaskManager.Domain.Extensions
{
    public static class UserExtensions
    {
        public static string GeneratePasswordHash(this string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha não pode ser nula ou vazia.");

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPasswordHash(this string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
