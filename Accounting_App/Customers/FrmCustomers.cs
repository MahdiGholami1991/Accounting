using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting_DataLayer;
using Accounting_DataLayer.Context;

namespace Accounting_App
{
    public partial class FrmCustomers : Form
    {
        public FrmCustomers()
        {
            InitializeComponent();
        }

        private void FrmCustomers_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        void BindGrid()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvCustomers.AutoGenerateColumns = false;
                dgvCustomers.DataSource = db.CustomerRepository.GetAllCustomers();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            BindGrid();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvCustomers.DataSource = db.CustomerRepository.GetCustomerByFilter(txtFilter.Text);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow != null)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    int customerId = Convert.ToInt16(dgvCustomers.CurrentRow.Cells[0].Value.ToString());

                    if (RtlMessageBox.Show(" آیا از حذف مطمئن هستید؟ ", "توجه!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        db.CustomerRepository.DeleteCustomer(customerId);
                        db.Save();
                        BindGrid();
                    }

                }
            }
            else
            {
                RtlMessageBox.Show("لطفا یک شخص را انتخاب نمایید");
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (new Customers.FrmAddEditCustomer().ShowDialog()==DialogResult.OK)
            {
                BindGrid();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow!=null)
            {
                Customers.FrmAddEditCustomer frmaddEdit = new Customers.FrmAddEditCustomer();
                int customerId = int.Parse(dgvCustomers.CurrentRow.Cells[0].Value.ToString());
                frmaddEdit.customerId = customerId;
                if (frmaddEdit.ShowDialog() == DialogResult.OK)
                {
                    BindGrid();
                }
            }
        }

       
    }
}
