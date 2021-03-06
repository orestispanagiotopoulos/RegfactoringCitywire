﻿using System;

namespace App.Exceptions
{
    public class EmptyNamesException : Exception
    {
        private const string message = "Invalid Name: {0}";

        public EmptyNamesException()
        {
        }

        public EmptyNamesException(string name)
            : base(String.Format(message, name))
        {
        }

        public EmptyNamesException(string name, Exception inner)
             : base(String.Format(message, name), inner)
        {
        }
    }
}
