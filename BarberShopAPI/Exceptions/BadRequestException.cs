namespace BarberShopAPI.Exceptions
{
    public class BadRequestException : BaseException
    {
        private static readonly string DEFAULT_CODE = "BAD_REQUEST";
        public BadRequestException(string code, string message) : base(code + DEFAULT_CODE, message)
        {
        }
    }
}
