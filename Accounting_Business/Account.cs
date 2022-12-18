
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting_DataLayer.Context;
using Acounting_ViewModels;

namespace Accounting_Business
{
    public class Account
    {
        public static AmountViewModel ReporViewModel()
        {
            AmountViewModel rp = new AmountViewModel();
            using (UnitOfWork db = new UnitOfWork())
            {
                DateTime StarDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);

                var recive = db.AccountingRepository.Get(c => c.TypeID == 1 && c.DateTime >= StarDate && c.DateTime <= EndDate).Select(a => a.Amount).ToList();
                var pay = db.AccountingRepository.Get(c => c.TypeID == 2 && c.DateTime >= StarDate && c.DateTime <= EndDate).Select(a => a.Amount).ToList();

                rp.pay = pay.Sum();
                rp.Recive = recive.Sum();
                rp.accountBalance =  recive.Sum() - pay.Sum();
            }
            return rp;
        }
         
   
    }
}
