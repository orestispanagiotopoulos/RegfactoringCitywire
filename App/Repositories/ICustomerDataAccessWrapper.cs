using App.Model.Entities;

namespace App.Repositories
{
    public interface ICustomerDataAccessWrapper
    {
        void AddCustomer(Customer customer);
    }
}
