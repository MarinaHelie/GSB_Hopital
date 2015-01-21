using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gsb_Hopital_Version1
{
    public partial class Naviguation : Form
    {
        public Naviguation()
        {
            InitializeComponent();
        }

        private void btnGoToAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            NewCustomer account = new NewCustomer();
            account.Show();
        }

        private void btnGoToFillOrCancel_Click(object sender, EventArgs e)
        {
            this.Hide();

            FillOrCancel account = new FillOrCancel();
            account.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
