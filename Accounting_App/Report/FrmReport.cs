using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting_DataLayer.Context;
using Acounting_Utility;
using Acounting_ViewModels;

namespace Accounting_App.Report
{
    public partial class FrmReport : Form
    {
        public int TypeID = 0;
        public FrmReport()
        {
            InitializeComponent();
        }

        private void FrmReport_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db=new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel()
                {
                    CustomerID=0,
                    FullName="انتخاب کنید"
                });
                list.AddRange(db.CustomerRepository.GetNameCustomers());
                cmbFilter.DataSource = list;
                cmbFilter.DisplayMember = "FullName";
                cmbFilter.ValueMember = "CustomerID";
            }
            if (TypeID == 2)
            {
                this.Text = "گزارش پرداخت";
            }
            else this.Text = "گزارش درآمد";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Filter();
        }
        public void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
               
                List<Accounting_DataLayer.Accounting> result = new List<Accounting_DataLayer.Accounting>();

                DateTime? startDate;
                DateTime? endDate;
              
                
                
                if ((int)cmbFilter.SelectedValue!=0)
                {
                    int customerId=Convert.ToInt16(cmbFilter.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.Get(c => c.TypeID == TypeID && c.CustomerID==customerId));
                }
                else
                {
                   result.AddRange(db.AccountingRepository.Get(c => c.TypeID == TypeID ));
                }
                if (txtFromDate.Text != "    /  /")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    startDate = DateConvertor.ToMiladi(startDate.Value);
                    result = result.Where(r => r.DateTime >= startDate.Value).ToList();
                }
                if (txtToDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);
                    endDate = DateConvertor.ToMiladi(endDate.Value);
                    result = result.Where(r => r.DateTime <= endDate.Value).ToList();
                }
                 
                dgvDisplay.Rows.Clear();
                //var Result = db.AccountingRepository.Get(c => c.TypeID == TypeID);
                //dgvDisplay.AutoGenerateColumns = false;
                //dgvDisplay.DataSource=Result;
                foreach (var item in result)
                {
                    string customerName = db.CustomerRepository.GetNameCustomerById(item.CustomerID);
                    dgvDisplay.Rows.Add(item.ID, customerName, item.Amount, item.DateTime.ToShamsi(), item.Description);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (dgvDisplay.CurrentRow != null && RtlMessageBox.Show("آیا از حذف مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    db.AccountingRepository.Delete(Convert.ToInt16(dgvDisplay.CurrentRow.Cells[0].Value.ToString()));
                    db.Save();
                    Filter();
                }

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (dgvDisplay.CurrentRow != null)
            {
                FrmNewAccounting frmNewAccounting = new FrmNewAccounting();
                frmNewAccounting.AccountingID = Convert.ToInt16(dgvDisplay.CurrentRow.Cells[0].Value.ToString());
                if ( frmNewAccounting.ShowDialog()==DialogResult.OK)
                {
                    Filter();
                }
               


            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add("Description");
            foreach (DataGridViewRow item in dgvDisplay.Rows)
            {
                dtPrint.Rows.Add(
                    item.Cells[0].Value.ToString(),
                    item.Cells[1].Value.ToString(),
                    item.Cells[2].Value.ToString(),
                    item.Cells[3].Value.ToString()
                    );
            }
        }
    }
}
