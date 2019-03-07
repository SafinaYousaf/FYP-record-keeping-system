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

namespace ProjectA
{
    public partial class AddStudenForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;Integrated Security=True");
        public AddStudenForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into Student values('" + stuId.Text + "','" + regNo.Text + "')";
            cmd.ExecuteNonQuery();
            con.Close();
            
            //Message box
            MessageBox.Show("Inserted");
        }

        private void AddStudenForm_Load(object sender, EventArgs e)
        {
            panel1.Hide();
            panel2.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            panel1.Left = 25;
            panel2.Top = 25;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
           
            
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Person (FirstName,LastName,Email,Contact,DateofBirth) VALUES('" +Fname.Text + "','" + Lname.Text + "','" + email.Text + "','" + Cont.Text + "','" + dob.Text + "')";
            cmd.ExecuteNonQuery();
            con.Close();
            
            //Message box
            MessageBox.Show("Inserted");
            

        }
        private String GetDbValue(String data)
        {
            if (String.IsNullOrEmpty(data))
                return "NULL";
            else
                return "'" + data + "'";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel2.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            panel2.Hide();
            panel1.Show();
        }

        private void Lname_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
