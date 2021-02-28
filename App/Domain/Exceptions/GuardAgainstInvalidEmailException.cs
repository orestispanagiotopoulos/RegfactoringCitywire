using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain.Exceptions
{
    public class GuardAgainstInvalidEmailException : Exception
    {
        private const string message = "Invalid emailAddress: {0}";

        public GuardAgainstInvalidEmailException()
        {
        }

        public GuardAgainstInvalidEmailException(string emailAddress)
            : base(String.Format(message, emailAddress))
        {
        }

        public GuardAgainstInvalidEmailException(string emailAddress, Exception inner)
             : base(String.Format(message, emailAddress), inner)
        {
        }
    }
}
