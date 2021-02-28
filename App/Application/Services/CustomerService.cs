using App.Domain;
using App.Domain.Entity;
using App.Domain.Exceptions;
using App.Domain.Factory;
using App.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Application.Service
{

    //public interface ICustomerDataAccessWrapper 
    //{
    //    void AddCustomer(Customer customer);
    //}

    public class CustomerService
    {
        private ICustomerFactory _customerFactory { get; set; }
        private ICompanyRepository _companyRepository { get; set; }
        private ICustomerCreditService _customerCreditServiceClient { get; set; }
        private CustomerDataAccessWrapper _customerDataAccessWrapper { get; set; }

        public CustomerService(
            ICustomerFactory customerFactory,
            ICompanyRepository companyRepository,
            ICustomerCreditService customerCreditServiceClient,
            CustomerDataAccessWrapper customerDataAccessWrapper
        )
        {
            _customerFactory = customerFactory;
            _companyRepository = companyRepository;
            _customerCreditServiceClient = customerCreditServiceClient;
            _customerDataAccessWrapper = customerDataAccessWrapper;
        }

        public bool AddCustomer(string firname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            var company = _companyRepository.GetById(companyId);

            Customer customer;
            try
            {
                customer = _customerFactory.Create(company, firname, surname, dateOfBirth, email);
            }
            catch (Exception ex)
            {
                if (ex is GuardAgainstEmptyNamesException || ex is GuardAgainstInvalidDateOfBirthException || ex is GuardAgainstInvalidEmailException)
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
