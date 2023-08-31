namespace RusBlazor.Exceptions
{
    public class ThisShouldNotHappenException : Exception
    {
        public ThisShouldNotHappenException()
        {
        }

        public ThisShouldNotHappenException(string message)
            : base(message)
        {
        }

        public ThisShouldNotHappenException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
