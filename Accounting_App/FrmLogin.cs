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

namespace Accounting_App
{
    public partial class FrmLogin : Form
    {
        public bool IsLogin = true;
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    if (IsLogin)
                    {
                        if (db.LoginRepository.Get(c => c.UserName == txtUserName.Text && c.Password == txtPassword.Text).Any())
                        {
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            RtlMessageBox.Show("نام کاربری یا کلمه عبور اشتباه است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Text = "";
                            txtUserName.Text = "";
                            txtUserName.Focus();
                        }
                    }
                    else
                    {
                        var Login = db.LoginRepository.Get().First();
                         Login.UserName=txtUserName.Text ;
                         Login.Password=txtPassword.Text ;
                         db.LoginRepository.Update(Login);
                         db.Save();
                         Application.Restart();
                    }
                  
                }
            }

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtUserName.Focus();
            if (!IsLogin)
            {
                txtUserName.Focus();
                this.Text = "تنظیمات ورود به برنامه";
                btnEnter.Text = "ثبت تغییرات";
                using (UnitOfWork db = new UnitOfWork())
                {
                    var Login = db.LoginRepository.Get().First();
                    txtUserName.Text = Login.UserName;
                    txtPassword.Text = Login.Password;
                }
            }
           
        }
    }
}
