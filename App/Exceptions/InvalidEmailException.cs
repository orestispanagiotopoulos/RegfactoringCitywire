using System;

namespace App.Exceptions
{
    public class InvalidEmailException : Exception
    {
        private const string message = "Invalid emailAddress: {0}";

        public InvalidEmailException()
        {
        }

        public InvalidEmailException(string emailAddress)
            : base(String.Format(message, emailAddress))
        {
        }

        public InvalidEmailException(string emailAddress, Exception inner)
             : base(String.Format(message, emailAddress), inner)
        {
        }
    }
}
