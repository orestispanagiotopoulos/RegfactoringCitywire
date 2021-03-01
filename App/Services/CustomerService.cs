using App.Exceptions;
using App.Factories;
using App.Model.Entities;
using App.Repositories;
using System;

namespace App.Services
{
    public class CustomerService
    {
        private ICustomerFactory _customerFactory;
        private ICompanyRepository _companyRepository;
        private ICustomerCreditService _customerCreditServiceClient;
        private ICustomerDataAccessWrapper _customerDataAccessWrapper;

        public CustomerService(
            ICustomerFactory customerFactory,
            ICompanyRepository companyRepository,
            ICustomerCreditService customerCreditServiceClient,
            ICustomerDataAccessWrapper customerDataAccessWrapper
        )
        {
            _customerFactory = customerFactory;
            _companyRepository = companyRepository;
            _customerCreditServiceClient = customerCreditServiceClient;
            _customerDataAccessWrapper = customerDataAccessWrapper;
        }

        public bool AddCustomer(string firstname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            var company = _companyRepository.GetById(companyId);

            Customer customer;
            try
            {
                customer = _customerFactory.Create(company, firstname, surname, dateOfBirth, email);
            }
            catch (Exception ex)
            {
                if (ex is EmptyNamesException || ex is InvalidDateOfBirthException || ex is InvalidEmailException)
                {
                    return false;
                }
                throw;
            }

            int? creditLimit = null;

            if (customer.ShouldPerformCreditCheck())
            {
                creditLimit = _customerCreditServiceClient.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
            }

            customer.ApplyCreditLimit(creditLimit);

			if (!customer.HasEnoughCredit())
            {
				return false;
			}

            _customerDataAccessWrapper.AddCustomer(customer);

            return true;
        }
    }
}
