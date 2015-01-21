using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Gsb_Hopital_Version1
{
    public partial class NewCustomer : Form
    {
        private int parsedCustomerID;
        private int orderID;

        string connstr = Gsb_Hopital_Version1.Utility.GetConnectionString();
        public NewCustomer()
        {
            InitializeComponent();
        }
        private bool isCustomerName()
        {
            if (txtCustomerName.Text == String.Empty)
            {
                MessageBox.Show("Please enter a name.");
                return false;
            }
            else
            {
                return true;
            }
        }
        private void btnCreateAccount_Click(object sender, EventArgs e)
        {

            if (isCustomerName())
            {
                SqlConnection conn = new SqlConnection(connstr);

                SqlCommand cmdNewCustomer = new SqlCommand("uspNewCustomer", conn);
                cmdNewCustomer.CommandType = CommandType.StoredProcedure;

                cmdNewCustomer.Parameters.Add(new SqlParameter("@CustomerName", SqlDbType.NVarChar, 40));
                cmdNewCustomer.Parameters["@CustomerName"].Value = txtCustomerName.Text;

                cmdNewCustomer.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
                cmdNewCustomer.Parameters["@CustomerID"].Direction = ParameterDirection.Output;

                try
                {
                    conn.Open();
                    cmdNewCustomer.ExecuteNonQuery();
                    this.parsedCustomerID = (int)cmdNewCustomer.Parameters["@CustomerID"].Value;
                    this.txtCustomerID.Text = Convert.ToString(parsedCustomerID);

                 }
                catch
                {
                    MessageBox.Show("Customer ID was not returned. Account could not be created.");
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        private bool isPlaceOrderReady()
        {
          
            if (txtCustomerID.Text == "")
            {
                MessageBox.Show("Please create customer account before placing order.");
                return false;
            }
            else if ((numOrderAmount.Value < 1))
            {
                MessageBox.Show("Please specify an order amount.");
                return false;
            }
            else
            {
                // Order can be submitted.
                return true;
            }
        }
        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (isPlaceOrderReady())
            {
                SqlConnection conn = new SqlConnection(connstr);
                SqlCommand cmdNewOrder = new SqlCommand("uspPlaceNewOrder", conn);
                cmdNewOrder.CommandType = CommandType.StoredProcedure;
                cmdNewOrder.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
                cmdNewOrder.Parameters["@CustomerID"].Value = this.parsedCustomerID;

                cmdNewOrder.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.DateTime, 8));
                cmdNewOrder.Parameters["@OrderDate"].Value = dtpOrderDate.Value;

                cmdNewOrder.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Int));
                cmdNewOrder.Parameters["@Amount"].Value = numOrderAmount.Value;

                cmdNewOrder.Parameters.Add(new SqlParameter("@Status", SqlDbType.Char, 1));
                cmdNewOrder.Parameters["@Status"].Value = "O";

                cmdNewOrder.Parameters.Add(new SqlParameter("@RC", SqlDbType.Int));
                cmdNewOrder.Parameters["@RC"].Direction = ParameterDirection.ReturnValue;

                try
                {
                    conn.Open();
                    cmdNewOrder.ExecuteNonQuery();

                    this.orderID = (int)cmdNewOrder.Parameters["@RC"].Value;
                    MessageBox.Show("Order number " + this.orderID + " has been submitted.");
                }
                catch
                {
                    MessageBox.Show("Order could not be placed.");
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        private void ClearForm()
        {
            txtCustomerName.Clear();
            txtCustomerID.Clear();
            dtpOrderDate.Value = DateTime.Now;
            numOrderAmount.Value = 0;
            this.parsedCustomerID = 0;
        }

        private void btnAddAnotherAccount_Click(object sender, EventArgs e)
        {
            this.ClearForm();
        }

        private void btnAddFinish_Click(object sender, EventArgs e)
        {
            this.Hide();
            Naviguation account = new Naviguation();
            account.Show();
        }
       

    }
}
