﻿using App.Exceptions;
using App.Helper;
using System;

namespace App.Model.Entities
{
    public class Customer
    {
        public const int CreditLimitThreshold = 500;

        public Customer(Company company, string firstname, string surname, DateTime dateOfBirth, string emailAddress)
        {
            ValidateCustomerCreation(firstname, surname, dateOfBirth, emailAddress);

            Company = company;
            Firstname = firstname;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            EmailAddress = emailAddress;
        }

        public int Id { get; private set; }

        public string Firstname { get; private set; }

        public string Surname { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public string EmailAddress { get; private set; }

        public bool? HasCreditLimit { get; private set; }

        public int? CreditLimit { get; private set; }

        public Company Company { get; private set; }

        public bool ShouldPerformCreditCheck()
        {
            if (Company.VeryImportantClient != Company.Name)
            {
                return true;
            }

            return false;
        }
        public bool HasEnoughCredit()
        {
            if (HasCreditLimit.Value && CreditLimit < CreditLimitThreshold)
            {
                return false;
            }

            return true;
        }

        public void ApplyCreditLimit(int? creditLimit)
        {
            if (!ShouldPerformCreditCheck())
            {
                HasCreditLimit = false;
            }
            else if (Company.Name == Company.ImportantClient)
            {
                HasCreditLimit = true;
                CreditLimit = 2 * creditLimit;
            }
            else
            {
                HasCreditLimit = true;
                CreditLimit = creditLimit;
            }
        }

        private void ValidateCustomerCreation(string firstname, string surname, DateTime dateOfBirth, string emailAddress)
        {
            GuardAgainstEmptyNames(firstname, surname);
            GuardAgainstInvalidEmail(emailAddress);
            GuardAgainstInvalidDateOfBirth(dateOfBirth);
        }

        private void GuardAgainstEmptyNames(string firstname, string surname)
        {
            if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(surname))
            {
                throw new EmptyNamesException($"{firstname} {surname}");
            }
        }

        private void GuardAgainstInvalidEmail(string emailAddress)
        {
            if (!emailAddress.Contains("@") && !emailAddress.Contains("."))
            {
                throw new InvalidEmailException(emailAddress);
            }
        }

        private void GuardAgainstInvalidDateOfBirth(DateTime dateOfBirth)
        {
            var now = DateTimeWrapper.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                throw new InvalidDateOfBirthException(dateOfBirth.ToString());
            }
        }
    }
}
