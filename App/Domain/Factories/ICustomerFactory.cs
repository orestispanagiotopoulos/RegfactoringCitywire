using App.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain.Factory
{
    public interface ICustomerFactory
    {
        Customer Create(Company company, string firstname, string surname, DateTime dateOfBirth, string emailAddress);
    }
}
