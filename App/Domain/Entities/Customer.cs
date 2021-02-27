﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain.Entity
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

        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public bool HasCreditLimit { get; set; }

        public int? CreditLimit { get; set; }

        public Company Company { get; set; }

        public bool HasEnoughCredit()
        {
            if (HasCreditLimit && CreditLimit < CreditLimitThreshold)
            {
                return false;
            }

            return true;
        }

        public bool ShouldPerformCreditCheck()
        {
            if (Company.VeryImportantClient != Company.Name)
            {
                return true;
            }

            return false;
        }

        public void ApplyCreditLimit(int? creditLimit)
        {
            if (!ShouldPerformCreditCheck())
            {
                HasCreditLimit = false;
                CreditLimit = creditLimit;
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
                throw new ArgumentException("");
            }
        }

        private void GuardAgainstInvalidEmail(string emailAddress)
        {
            if (!emailAddress.Contains("@") && !emailAddress.Contains("."))
            {
                throw new ArgumentException("");
            }
        }

        private void GuardAgainstInvalidDateOfBirth(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                throw new ArgumentException("");
            }
        }
    }
}