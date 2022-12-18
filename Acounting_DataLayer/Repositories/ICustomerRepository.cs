using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acounting_ViewModels;
namespace Accounting_DataLayer.Repositories
{
  public  interface ICustomerRepository
    {
        List<Customers> GetAllCustomers();
        IEnumerable<Customers> GetCustomerByFilter(string paramater);
        List<ListCustomerViewModel> GetNameCustomers(string Filter = "");
        Customers GetCustomerById(int customerId);
        bool InsertCustomer(Customers customer);
        bool UpdateCustomer(Customers customer);
        bool DeleteCustomer(Customers customer);
        bool DeleteCustomer(int customerId);
        int GetCustomerIDByName(string Name);
        string GetNameCustomerById(int customerId);
    }
}
