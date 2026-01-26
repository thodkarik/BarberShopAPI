namespace BarberShopAPI.Exceptions
{
    public class ForbiddenException : BaseException
    {
        private static readonly string DEFAULT_CODE = "FORBIDDEN";

        public ForbiddenException(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
    }
}
