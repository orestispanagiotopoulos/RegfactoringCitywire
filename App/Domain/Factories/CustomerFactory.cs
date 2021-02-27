using App.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain.Factory
{
    public class CustomerFactory : ICustomerFactory
    {
        public Customer Create(Company company, string firstname, string surname, DateTime dateOfBirth, string emailAddress)
        {
            return new Customer(company, firstname, surname, dateOfBirth, emailAddress);
        }
    }
}
