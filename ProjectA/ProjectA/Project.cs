using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectA
{
    public partial class Project : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;MultipleActiveResultSets=true;Integrated Security=True");
        public Project()
        {
            InitializeComponent();
        }

        private void Project_Load(object sender, EventArgs e)
        {
            panel2.Hide();
            panel3.Hide();
            panel4.Hide();
            panel5.Hide();
            panel1.Show();
            disp_data();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Display studfrm = new Display();
            studfrm.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Advisorr advisorfrm = new Advisorr();
            advisorfrm.Show();
        }
        public void disp_data()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Project";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ProjectGrid.DataSource = dt; //ProjectGrid is name of data grid view present on form
            con.Close();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            panel4.Hide();
            panel5.Hide();
            panel3.Hide();
            panel1.Show();
            disp_data();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel4.Hide();
            panel5.Hide();
            panel2.Show();
            panel3.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Title.Text))
            {
                MessageBox.Show("Title is compulsory.");
            }
            else
            {
                con.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Project WHERE ([ID] = @ID)", con);
                check_User_Name.Parameters.AddWithValue("ID", ProId.Text);
                SqlDataReader reader = check_User_Name.ExecuteReader();
                if (reader.HasRows)
                {
                    con.Close();
                    MessageBox.Show("Record exists against provided ID.");
                }
                else
                {
                    con.Close();
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd = new SqlCommand("INSERT INTO Project(Description, Title) VALUES(@Description,@Title)", con);
                    cmd.Parameters.AddWithValue("@Description", ProId.Text);

                    cmd.Parameters.AddWithValue("@Title", Title.Text);


                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data Inserted Successfully");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            panel3.Hide();
            panel5.Hide();
            panel1.Show();
            panel4.Show();

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel3.Hide();
            panel4.Hide();
            panel5.Show();
            panel2.Show();
           

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Project WHERE ([ID] = @ID)", con);
            check_User_Name.Parameters.AddWithValue("ID", IDdel.Text);
            SqlDataReader reader = check_User_Name.ExecuteReader();
            if (reader.HasRows)
            {

                
                SqlCommand cmd = new SqlCommand("DELETE FROM Project WHERE ID='" + IDdel.Text + "';", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Deleted Sucessfully");
            }
            else
            {
                con.Close();
                MessageBox.Show("Record does not exists.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ProjectAdvisor proadd = new ProjectAdvisor();
            proadd.Show();
        }
    }
}
