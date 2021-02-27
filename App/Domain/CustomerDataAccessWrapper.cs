using App.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain
{
    public class CustomerDataAccessWrapper : ICustomerDataAccessWrapper
    {
        public void AddCustomer(Customer customer)
        {
            CustomerDataAccess.AddCustomer(customer);
        }
    }
}
