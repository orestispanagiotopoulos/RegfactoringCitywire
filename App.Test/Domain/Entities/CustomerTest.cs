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
    }
}
