using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;
using Accounting_DataLayer.Context;
using System.IO;

namespace Accounting_App.Customers
{
    public partial class FrmAddEditCustomer : Form
    {
        public int customerId = 0;
        UnitOfWork db = new UnitOfWork();
        public FrmAddEditCustomer()
        {
            InitializeComponent();
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                pcCustomer.ImageLocation = openfile.FileName;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                using (UnitOfWork db = new UnitOfWork())
                {

                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);
                    string path = Application.StartupPath + "/Images/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    pcCustomer.Image.Save(path + imageName);
                    Accounting_DataLayer.Customers customer = new Accounting_DataLayer.Customers
                    {
                        Address = txtAddress.Text,
                        Mobile = txtMobile.Text,
                        FullName = txtName.Text,
                        Email = txtEmail.Text,
                        CustomerImage = imageName
                    };
                    if (customerId == 0)
                    {
                        db.CustomerRepository.InsertCustomer(customer);
                    }
                    else
                    {
                        customer.CustomerID = customerId;
                        db.CustomerRepository.UpdateCustomer(customer);
                    }

                    db.Save();

                    DialogResult = DialogResult.OK;

                }
            }
        }

        private void FrmAddEditCustomer_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {

                FrmAddEditCustomer frm = new FrmAddEditCustomer();

                if (customerId != 0)
                {
                    this.Text = "وبرایش شخص";
                    this.btnSubmit.Text = " ثبت وبرایش";
                    var customer = db.CustomerRepository.GetCustomerById(customerId);
                    txtName.Text = customer.FullName;
                    txtMobile.Text = customer.Mobile;
                    txtEmail.Text = customer.Email;
                    txtAddress.Text = customer.Address;
                    pcCustomer.ImageLocation = Application.StartupPath + "/Images/" + customer.CustomerImage;
                }

            }
        }
    }
}
