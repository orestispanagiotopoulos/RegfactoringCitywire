using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain.Exceptions
{
    public class InvalidDateOfBirthException : Exception
    {
        private const string message = "Invalid date of birth: {0}";

        public InvalidDateOfBirthException()
        {
        }

        public InvalidDateOfBirthException(string dateOfBirth)
            : base(String.Format(message, dateOfBirth))
        {
        }

        public InvalidDateOfBirthException(string dateOfBirth, Exception inner)
             : base(String.Format(message, dateOfBirth), inner)
        {
        }
    }
}
