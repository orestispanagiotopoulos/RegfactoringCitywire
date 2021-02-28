using App.Domain.Entity;
using App.Domain.Exceptions;
using App.Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Test.Domain.Entities
{
    class CustomerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Customer_WhenAgeLessThan21_ThenThrowGuardAgainstInvalidDateOfBirthException()
        {
            // Arrange
            var fakeDateNow = new DateTime(2021, 1, 1);
            var invalidDateOfBirth = new DateTime(2000, 1, 2);

            // Act - Assert
            using (new DateTimeContext(fakeDateNow))
            {
                var ex = Assert.Throws<GuardAgainstInvalidDateOfBirthException>(
                    () => new Customer(new Company(), "Robbert", "Fisher", invalidDateOfBirth, "R.Fisher@gmail.com"));
            }
        }

        [Test]
        public void Customer_WhenAgeIs21_ThenReturnCustomerObject()
        {
            // Arrange
            var fakeDateNow = new DateTime(2021, 1, 1);
            var invalidDateOfBirth = new DateTime(2000, 1, 1); // Customer is exactly 21 years old

            // Act
            Customer customer = null;
            using (new DateTimeContext(fakeDateNow)) // Date time at present
            {
                customer = new Customer(new Company(), "Robbert", "Fisher", invalidDateOfBirth, "R.Fisher@gmail.com");
            }

            // Assert 
            Assert.IsInstanceOf(typeof(Customer), customer);
        }

        [Test]
        public void Customer_WhenFirstnameIsEmpty_ThenThrowGuardAgainstEmptyNamesException()
        {
            // Arrange
            var firstName = string.Empty;

            // Act - Assert
            var ex = Assert.Throws<GuardAgainstEmptyNamesException>(
                () => new Customer(new Company(), firstName, "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com"));
        }

        [Test]
        public void Customer_WhenFirstnameIsNull_ThenThrowGuardAgainstEmptyNamesException()
        {
            // Arrange
            string firstName = null;

            // Act - Assert
            var ex = Assert.Throws<GuardAgainstEmptyNamesException>(
                () => new Customer(new Company(), firstName, "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com"));
        }

        [Test]
        public void Customer_WhenInvalidEmailWithoutAtAndDot_ThenThrowGuardAgainstInvalidEmailException()
        {
            // Arrange
            string email = "R,Fisher,gmail,com";

            // Act - Assert
            var ex = Assert.Throws<GuardAgainstInvalidEmailException>(
                () => new Customer(new Company(), "Robbert", "Fisher", new DateTime(1995, 01, 01), email));
        }

        [Test]
        public void Customer_WhenValidInput_ThenReturnValidCustomer()
        {
            // Arrange
            var firstName = "Robbert";
            var surname = "Fisher";
            var dateOfBirth = new DateTime(1995, 01, 01);
            var emailaddress = "R.Fisher@gmail.com";

            // Act
            var customer = new Customer(new Company(), firstName, surname, dateOfBirth, emailaddress);

            // Assert
            Assert.AreEqual(firstName, customer.Firstname);
            Assert.AreEqual(surname, customer.Surname);
            Assert.AreEqual(dateOfBirth, customer.DateOfBirth);
            Assert.AreEqual(emailaddress, customer.EmailAddress);
        }

        [Test]
        public void ShouldPerformCreditCheck_WhenVeryImportanClient_ThenReturnFalse()
        {
            // Arrange
            var company = new Company() { Name = Company.VeryImportantClient };
            var customer = new Customer(company, "Robbert", "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com");

            // Act 
            var result = customer.ShouldPerformCreditCheck();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldPerformCreditCheck_WhenNotVeryImportanClient_ThenReturnTrue()
        {
            // Arrange
            var company = new Company() { Name = Company.ImportantClient };
            var customer = new Customer(company, "Robbert", "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com");

            // Act 
            var result = customer.ShouldPerformCreditCheck();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void HasEnoughCredit_WhenHasCreditLimitSmallerThanThreshold_ThenReturnFalse()
        {
            // Arrange
            var customer = new Customer(new Company(), "Robbert", "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com") 
            {
                HasCreditLimit = true, 
                CreditLimit = 450
            };

            // Act 
            var result = customer.HasEnoughCredit();

            // Assert
            Assert.False(result);
        }

        [Test]
        public void HasEnoughCredit_WhenHasCreditLimitGreaterThanThreshold_ThenReturnTrue()
        {
            // Arrange
            var customer = new Customer(new Company(), "Robbert", "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com")
            {
                HasCreditLimit = true,
                CreditLimit = 550
            };

            // Act 
            var result = customer.HasEnoughCredit();

            // Assert
            Assert.True(result);
        }

        [Test]
        public void HasEnoughCredit_WhenFlagHasCreditLimitIsFalse_ThenReturnTrue()
        {
            // Arrange
            var customer = new Customer(new Company(), "Robbert", "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com")
            {
                HasCreditLimit = false
            };

            // Act 
            var result = customer.HasEnoughCredit();

            // Assert
            Assert.True(result);
        }

        [Test]
        public void ApplyCreditLimit_WhenVeryImportantClient_ThenSetHasCreditLimitToFalse()
        {
            // Arrange
            var name = Company.VeryImportantClient;
            var customer = new Customer(new Company { Name = name }, "Robbert", "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com");

            // Act 
            customer.ApplyCreditLimit(0);

            // Assert
            Assert.AreEqual(false, customer.HasCreditLimit);
        }

        [Test]
        public void ApplyCreditLimit_WhenImportantClient_ThenDoubleTheCreditLimit()
        {
            // Arrange
            var name = Company.ImportantClient;
            var creditLimit = 1000;
            var customer = new Customer(new Company { Name = name }, "Robbert", "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com");

            // Act 
            customer.ApplyCreditLimit(creditLimit);

            // Assert
            Assert.AreEqual(true, customer.HasCreditLimit);
            Assert.AreEqual(2* creditLimit, customer.CreditLimit);
        }

        [Test]
        public void ApplyCreditLimit_WhenAnyNotImportantClient_ThenDoNotDoubleTheCreditLimit()
        {
            // Arrange
            var name = "AnyNotImportantClient";
            var creditLimit = 1000;
            var customer = new Customer(new Company { Name = name }, "Robbert", "Fisher", new DateTime(1995, 01, 01), "R.Fisher@gmail.com");

            // Act 
            customer.ApplyCreditLimit(creditLimit);

            // Assert
            Assert.AreEqual(true, customer.HasCreditLimit);
            Assert.AreEqual(creditLimit, customer.CreditLimit);
        }
    }
}
