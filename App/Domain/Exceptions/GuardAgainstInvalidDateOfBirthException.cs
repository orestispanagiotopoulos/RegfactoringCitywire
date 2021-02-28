using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain.Exceptions
{
    public class GuardAgainstInvalidDateOfBirthException : Exception
    {
        private const string message = "Invalid date of birth: {0}";

        public GuardAgainstInvalidDateOfBirthException()
        {
        }

        public GuardAgainstInvalidDateOfBirthException(string dateOfBirth)
            : base(String.Format(message, dateOfBirth))
        {
        }

        public GuardAgainstInvalidDateOfBirthException(string dateOfBirth, Exception inner)
             : base(String.Format(message, dateOfBirth), inner)
        {
        }
    }
}
