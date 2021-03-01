using App.Model.Entities;
using App.Repositories;

namespace App.Factories
{
    public class CustomerDataAccessWrapper : ICustomerDataAccessWrapper
    {
        public void AddCustomer(Customer customer)
        {
            CustomerDataAccess.AddCustomer(customer);
        }
    }
}
