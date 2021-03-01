using App.Model.Entities;
using System;

namespace App.Factories
{
    public interface ICustomerFactory
    {
        Customer Create(Company company, string firstname, string surname, DateTime dateOfBirth, string emailAddress);
    }
}
