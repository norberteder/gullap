using System;

namespace DevTyr.Gullap.Yaml
{
    public class InvalidPageException : Exception
    {
        public InvalidPageException()
        {
        }

        public InvalidPageException(string message)
            : base(message) { }

        public InvalidPageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
