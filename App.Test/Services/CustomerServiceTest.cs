using App.Factories;
using App.Helper;
using App.Model.Entities;
using App.Repositories;
using App.Services;
using Moq;
using NUnit.Framework;
using System;

namespace App.Test.Services
{
    public class CustomerServiceTest
    {
        public CustomerFactory CustomerFactory { get; private set; }
        public Mock<ICompanyRepository> CompanyRepository { get; private set; }
        public Mock<ICustomerCreditService> CustomerCreditServiceClient { get; private set; }
        public Mock<ICustomerDataAccessWrapper> CustomerDataAccessWrapper { get; private set; }
        public CustomerService CustomerService { get; private set; }

        [SetUp]
        public void Setup()
        {
            CustomerFactory = new CustomerFactory();
            CompanyRepository = new Mock<ICompanyRepository>();
            CustomerCreditServiceClient = new Mock<ICustomerCreditService>();
            CustomerDataAccessWrapper = new Mock<ICustomerDataAccessWrapper>();

            CustomerService = new CustomerService(CustomerFactory, CompanyRepository.Object, CustomerCreditServiceClient.Object, CustomerDataAccessWrapper.Object);
        }

        [Test]
        public void AddCustomer_WhenAgeLessThan21_ThenReturnFalse()
        {
            // Arrange
            var fakeDateNow = new DateTime(2021, 1, 1);
            var invalidDateOfBirth = new DateTime(2001, 2, 10);

            // Act
            bool result = true;
            using (new DateTimeContext(fakeDateNow))
            {
                result = CustomerService.AddCustomer("Robbert", "Fisher", "R.Fisher@gmail.com", invalidDateOfBirth, 100);
            }

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddCustomer_WhenInvalidEmail_ThenReturnFalse()
        {
            // Arrange
            var invalidEmail = "R,Fisher,gmail,com";

            // Act
             var result = CustomerService.AddCustomer("Robbert", "Fisher", invalidEmail, new DateTime(1995, 2, 10), 100);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddCustomer_WhenNullSurname_ThenReturnFalse()
        {
            // Arrange
            string surname = null;

            // Act
            var result = CustomerService.AddCustomer("Robbert", surname, "R.Fisher@gmail.com", new DateTime(1995, 2, 10), 100);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddCustomer_WhenVeryImportantCustomer_ThenAddCorrectCustomerAndReturnTrue()
        {
            // Arrange
            var name = Company.VeryImportantClient;

            CompanyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company 
            {
                Name = name
            });

            Customer customer = null;
            CustomerDataAccessWrapper.Setup(x => x.AddCustomer(It.IsAny<Customer>()))
                .Callback<Customer>((c) => customer = c);

            // Act
            var shouldAddClient = CustomerService.AddCustomer("Robbert", "Fisher", "R.Fisher@gmail.com", new DateTime(1995, 2, 10), 100);

            // Assert
            Assert.AreEqual(true, shouldAddClient);
            Assert.AreEqual(false, customer.HasCreditLimit);
            Assert.IsNull(customer.CreditLimit); // Default value as the customer doesn't have credit limit.
            CustomerDataAccessWrapper.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [Test]
        public void AddCustomer_WhenImportantCustomer_ThenAddCorrectCustomerAndReturnTrue()
        {
            // Arrange
            var name = Company.ImportantClient;
            var creditLimit = 1000;

            CompanyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company
            {
                Name = name
            });

            CustomerCreditServiceClient.Setup(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(creditLimit);

            Customer customer = null;
            CustomerDataAccessWrapper.Setup(x => x.AddCustomer(It.IsAny<Customer>()))
                .Callback<Customer>((c) => customer = c);

            // Act
            var shouldAddClient = CustomerService.AddCustomer("Robbert", "Fisher", "R.Fisher@gmail.com", new DateTime(1995, 2, 10), 100);

            // Assert
            Assert.AreEqual(true, shouldAddClient);
            Assert.AreEqual(true, customer.HasCreditLimit);
            Assert.AreEqual(2*creditLimit, customer.CreditLimit);
            CustomerDataAccessWrapper.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [Test]
        public void AddCustomer_WhenImportanCustomerDoNotPassTheCreditThreshold_ThenDoNotAddCustomerAndReturnFalse()
        {
            // Arrange
            var name = Company.ImportantClient;
            var creditLimit = 200; // 2*200 < 500

            CompanyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company
            {
                Name = name
            });

            CustomerCreditServiceClient.Setup(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(creditLimit);

            Customer customer = null;
            CustomerDataAccessWrapper.Setup(x => x.AddCustomer(It.IsAny<Customer>()))
                .Callback<Customer>((c) => customer = c);

            // Act
            var shouldAddClient = CustomerService.AddCustomer("Robbert", "Fisher", "R.Fisher@gmail.com", new DateTime(1995, 2, 10), 100);

            // Assert
            Assert.AreEqual(false, shouldAddClient);
            CustomerCreditServiceClient.Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
            CustomerDataAccessWrapper.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.Never);
        }

        [Test]
        public void AddCustomer_WhenAnyNotImportantCustomerWhoPassesTheCreditLimitThreshold_ThenAddCorrectCustomerAndReturnTrue()
        {
            // Arrange
            var name = "AnyNotImportantClient";
            var creditLimit = 1000;

            CompanyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company
            {
                Name = name
            });

            CustomerCreditServiceClient.Setup(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(creditLimit);

            Customer customer = null;
            CustomerDataAccessWrapper.Setup(x => x.AddCustomer(It.IsAny<Customer>()))
                .Callback<Customer>((c) => customer = c);

            // Act
            var shouldAddClient = CustomerService.AddCustomer("Robbert", "Fisher", "R.Fisher@gmail.com", new DateTime(1995, 2, 10), 100);

            // Assert
            Assert.AreEqual(true, shouldAddClient);
            Assert.AreEqual(true, customer.HasCreditLimit);
            Assert.AreEqual(creditLimit, customer.CreditLimit);
            CustomerDataAccessWrapper.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [Test]
        public void AddCustomer_WhenAnyNotImportantCustomerWhoDoNotPassesTheCreditLimitThreshold_ThenDoNotAddCustomerAndReturnFalse()
        {
            // Arrange
            var name = "AnyNotImportantClient";
            var creditLimit = 400; 

            CompanyRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Company
            {
                Name = name
            });

            CustomerCreditServiceClient.Setup(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(creditLimit);

            Customer customer = null;
            CustomerDataAccessWrapper.Setup(x => x.AddCustomer(It.IsAny<Customer>()))
                .Callback<Customer>((c) => customer = c);

            // Act
            var shouldAddClient = CustomerService.AddCustomer("Robbert", "Fisher", "R.Fisher@gmail.com", new DateTime(1995, 2, 10), 100);

            // Assert
            Assert.AreEqual(false, shouldAddClient);
            CustomerCreditServiceClient.Verify(x => x.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
            CustomerDataAccessWrapper.Verify(x => x.AddCustomer(It.IsAny<Customer>()), Times.Never);
        }
    }
}
