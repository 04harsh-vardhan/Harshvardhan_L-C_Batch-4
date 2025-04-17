namespace OnShopApi.HelperClasses
{
    public class NoRecordException : Exception
    {
        public NoRecordException(string message) : base(message) { }

    }
}
