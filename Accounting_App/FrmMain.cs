using Acounting_Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting_Business;
using Acounting_ViewModels;

namespace Accounting_App
{
    public partial class FrmAccounting : Form
    {
        public FrmAccounting()
        {
            InitializeComponent();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            new FrmCustomers().ShowDialog();
        }

        private void btnNewAccounting_Click(object sender, EventArgs e)
        {
            new FrmNewAccounting().ShowDialog();
        }

        private void btnReportPay_Click(object sender, EventArgs e)
        {
            Report.FrmReport frmReport = new Report.FrmReport();
            frmReport.TypeID = 2;
            frmReport.ShowDialog();
        }

        private void btnReportRecive_Click(object sender, EventArgs e)
        {
            Report.FrmReport frmReport = new Report.FrmReport();
            frmReport.TypeID = 1;
            frmReport.ShowDialog();
        }

        private void FrmAccounting_Load(object sender, EventArgs e)
        {
           
            this.Hide();
            FrmLogin frmLogin = new FrmLogin();

            if (frmLogin.ShowDialog()==DialogResult.OK)
            {
                this.Show();
                dateNow.Text = DateTime.Now.ToShamsi();
                timeNow.Text = DateTime.Now.ToString("HH:mm:ss");
                Report();

            }
            else
            {
                Application.Exit();
            }
            
        }
        void Report()
        {
            AmountViewModel viewModel = Account.ReporViewModel();
            lblRecive.Text = viewModel.Recive.ToString("#,0");
            lblPay.Text = viewModel.pay.ToString("#,0");
            lblAccountingBalance.Text = viewModel.accountBalance.ToString("#,0");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeNow.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnSetLogin_Click(object sender, EventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.IsLogin = false;
            frmLogin.ShowDialog();
        }
    }
}
