using App.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain
{
    public interface ICustomerDataAccessWrapper
    {
        void AddCustomer(Customer customer);
    }
}
