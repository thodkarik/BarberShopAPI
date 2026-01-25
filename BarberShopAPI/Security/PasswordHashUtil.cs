namespace BarberShopAPI.Security
{
    public static class PasswordHashUtil
    {
        public static string Hash(string plainText)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainText);
        }

        public static bool Verify(string plainText, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, hash);
        }
    }
}
