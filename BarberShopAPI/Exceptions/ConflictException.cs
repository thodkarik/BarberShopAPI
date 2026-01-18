namespace BarberShopAPI.Exceptions
{
    public class ConflictException : BaseException
    {
        private static readonly string DEFAULT_CODE = "CONFLICT";
        public ConflictException(string code, string message) : base(code + DEFAULT_CODE, message)
        {
        }
    }
}
