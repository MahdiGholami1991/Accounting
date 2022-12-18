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
using ValidationComponents;
using Accounting_DataLayer;

namespace Accounting_App
{
    public partial class FrmNewAccounting : Form
    {
        // UnitOfWork db = new UnitOfWork();
        public int AccountingID = 0;
        public FrmNewAccounting()
        {
            InitializeComponent();
        }

        private void FrmNewAccounting_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvPerson.AutoGenerateColumns = false;
                dgvPerson.DataSource = db.CustomerRepository.GetAllCustomers();
                if (AccountingID != 0)
                {
                    using (UnitOfWork db2 = new UnitOfWork())
                    {
                        var result = db2.AccountingRepository.GetById(AccountingID);
                        this.Text = "ویرایش";
                        txtAmount.Value = result.Amount;
                        txtDescription.Text = result.Description;
                        txtName.Text = db2.CustomerRepository.GetNameCustomerById(result.CustomerID);
                        btnSave.Text = "ویرایش";
                        if (result.TypeID == 2)
                        {
                            rbPay.Checked = true;
                        }
                        else
                        {
                            rbRecive.Checked = true;
                        }
                    }

                }
            }
        }

        private void txtAccountingFilter_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvPerson.AutoGenerateColumns = false;
                dgvPerson.DataSource = db.CustomerRepository.GetNameCustomers(txtAccountingFilter.Text);
            }
        }

        private void dgvPerson_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvPerson.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                if (rbPay.Checked || rbRecive.Checked)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        Accounting_DataLayer.Accounting accounting = new Accounting_DataLayer.Accounting()
                        {
                            ID=AccountingID,
                            Amount = int.Parse(txtAmount.Value.ToString()),
                            CustomerID = db.CustomerRepository.GetCustomerIDByName(txtName.Text),
                            TypeID = (rbRecive.Checked) ? 1 : 2,
                            DateTime = DateTime.Now,
                            Description = txtDescription.Text
                        };
                        if (AccountingID == 0)
                        {
                                db.AccountingRepository.Insert(accounting);

                        }
                        else
                        {
                                db.AccountingRepository.Update(accounting);

                        }
                         db.Save();
                        DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    RtlMessageBox.Show("لطفا نوع تراکنش را انتخاب کنید");
                }
            }
        }
    }
}
