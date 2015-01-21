using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;

namespace Gsb_Hopital_Version1
{
    public partial class FillOrCancel : Form
    {
        private int parsedOrderID;

        string connstr = Gsb_Hopital_Version1.Utility.GetConnectionString();

        public FillOrCancel()
        {
            InitializeComponent();
        }
        private bool isOrderID()
        {
            if (txtOrderID.Text == "")
            {
                MessageBox.Show("Please specify the Order ID.");
                return false;
            }
            else if (Regex.IsMatch(txtOrderID.Text, @"^\D*$"))
            {
                MessageBox.Show("Please specify integers only.");
                txtOrderID.Clear();
                return false;
            }
            else
            {
                parsedOrderID = Int32.Parse(txtOrderID.Text);
                return true;
            }
        }
        private void btnFindByOrderID_Click(object sender, EventArgs e)
        {
            if (isOrderID())
            {
                SqlConnection conn = new SqlConnection(connstr);
                string sql = "select * from Orders where orderID = @orderID";
                SqlCommand cmdOrderID = new SqlCommand(sql, conn);
                cmdOrderID.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
                cmdOrderID.Parameters["@orderID"].Value = parsedOrderID;
                try
                {
                    conn.Open();
                    SqlDataReader rdr = cmdOrderID.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(rdr);
                    this.dgvCustomerOrders.DataSource = dataTable;
                    rdr.Close();
                }
                catch
                {
                    MessageBox.Show("The requested order could not be loaded into the form.");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (isOrderID())
            {
                SqlConnection conn = new SqlConnection(connstr);
                SqlCommand cmdCancelOrder = new SqlCommand("uspCancelOrder", conn);
                cmdCancelOrder.CommandType = CommandType.StoredProcedure;
                cmdCancelOrder.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
                cmdCancelOrder.Parameters["@orderID"].Value = parsedOrderID;

                try
                {
                    conn.Open();
                    cmdCancelOrder.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("The cancel operation was not completed.");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnFillOrder_Click(object sender, EventArgs e)
        {
            if (isOrderID())
            {
                SqlConnection conn = new SqlConnection(connstr);
                SqlCommand cmdFillOrder = new SqlCommand("uspFillOrder", conn);
                cmdFillOrder.CommandType = CommandType.StoredProcedure;
                cmdFillOrder.Parameters.Add(new SqlParameter("@orderID", SqlDbType.Int));
                cmdFillOrder.Parameters["@orderID"].Value = parsedOrderID;
                cmdFillOrder.Parameters.Add(new SqlParameter("@FilledDate", SqlDbType.DateTime, 8));
                cmdFillOrder.Parameters["@FilledDate"].Value = dtpFillDate.Value;

                try
                {
                    conn.Open();
                    cmdFillOrder.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("The fill operation was not completed.");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnFinishUpdates_Click(object sender, EventArgs e)
        {
            this.Hide();
            Naviguation account = new Naviguation();
            account.Show();
        }
    }
}
