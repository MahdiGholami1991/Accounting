using System;
using System.Collections.Generic;
using Accounting_DataLayer.Repositories;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Acounting_ViewModels;

namespace Accounting_DataLayer.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private AccountingEntities db;
        public CustomerRepository(AccountingEntities Context)
        {
            db = Context;
        }

        public List<Customers> GetAllCustomers()
        {

            return db.Customers.ToList();
        }

        public Customers GetCustomerById(int customerId)
        {
            return db.Customers.Find(customerId);
        }

        public bool InsertCustomer(Customers customer)
        {
            try
            {
                db.Customers.Add(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                var customer = GetCustomerById(customerId);
                DeleteCustomer(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public IEnumerable<Customers> GetCustomerByFilter(string paramater)
        {
            return db.Customers.Where(c => c.FullName.Contains(paramater) || c.Email.Contains(paramater) || c.Mobile.Contains(paramater)).ToList();
        }


        public List<ListCustomerViewModel> GetNameCustomers(string Filter = "")
        {
            if (Filter == "")
            {
                return db.Customers.Select(c => new ListCustomerViewModel()
                {
                    CustomerID = c.CustomerID,
                    FullName = c.FullName
                }).ToList();
            }
            return db.Customers.Where(c => c.FullName.Contains(Filter)).Select(c => new ListCustomerViewModel()
            {
                CustomerID = c.CustomerID,
                FullName = c.FullName
            }).ToList();
        }





        public int GetCustomerIDByName(string Name)
        {
            return db.Customers.First(c => c.FullName == Name).CustomerID;
        }




        public string GetNameCustomerById(int customerId)
        {
            return db.Customers.First(c => c.CustomerID == customerId).FullName;
        }
    }
}
