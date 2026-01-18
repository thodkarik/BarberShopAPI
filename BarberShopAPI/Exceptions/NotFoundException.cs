namespace BarberShopAPI.Exceptions
{
    public class NotFoundException : BaseException
    {
        private static readonly string DEFAULT_CODE = "NOT_FOUND";

        public NotFoundException(string code, string message) : base(code + DEFAULT_CODE, message)
        {
        }
    }
}
