using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting_DataLayer;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountingEntities db = new AccountingEntities();
           var list= db.Customers.ToList();
        }
    }
}
