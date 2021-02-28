using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain.Exceptions
{
    public class GuardAgainstEmptyNamesException : Exception
    {
        private const string message = "Invalid Name: {0}";

        public GuardAgainstEmptyNamesException()
        {
        }

        public GuardAgainstEmptyNamesException(string name)
            : base(String.Format(message, name))
        {
        }

        public GuardAgainstEmptyNamesException(string name, Exception inner)
             : base(String.Format(message, name), inner)
        {
        }
    }
}
