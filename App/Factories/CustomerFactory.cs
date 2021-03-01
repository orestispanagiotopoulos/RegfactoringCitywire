using App.Model.Entities;
using System;

namespace App.Factories
{
    public class CustomerFactory : ICustomerFactory
    {
        public Customer Create(Company company, string firstname, string surname, DateTime dateOfBirth, string emailAddress)
        {
            return new Customer(company, firstname, surname, dateOfBirth, emailAddress);
        }
    }
}
