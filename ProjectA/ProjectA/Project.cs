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
        int chk = 0;
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;MultipleActiveResultSets=true;Integrated Security=True");
        public Project()
        {
            InitializeComponent();
        }

        private void Project_Load(object sender, EventArgs e)
        {
            chk++;
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

        DataTable dt = new DataTable();
        public void disp_data()
        {
           
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Project";
            cmd.ExecuteNonQuery();
            //DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ProjectGrid.DataSource = dt; //ProjectGrid is name of data grid view present on form
            con.Close();
            
                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                ProjectGrid.Columns.Add(btn);
                btn.HeaderText = "Delete";
                btn.Text = "Delete";
                btn.Name = "btn";
                btn.UseColumnTextForButtonValue = true;
            
            DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
            ProjectGrid.Columns.Add(btn1);
            btn1.HeaderText = "Update";
            btn1.Text = "Update";
            btn1.Name = "btn1";
            btn1.UseColumnTextForButtonValue = true;



        }
        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            panel4.Hide();
            panel5.Hide();
            panel3.Hide();
            panel1.Show();
            //disp_data();
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
            update_rec();
            panel2.Hide();
            panel3.Hide();
            panel5.Hide();
            
            panel4.Hide();

            panel1.Show();

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
        private void ProjectGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this candidate?", "Candidate", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Del_rec();
                }
               
            }
            if (e.ColumnIndex == 4)
            {
                MessageBox.Show("ok");

                DialogResult result = MessageBox.Show("Are you sure you want to update ?", "Project", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    panel1.Hide();
                    panel3.Hide();
                    panel4.Hide();
                    panel5.Show();
                    panel2.Show();
                    
                    
                }

            }

        }

        private void update_rec()
        {
            con.Open();

            int index = ProjectGrid.CurrentCell.RowIndex;
            ProjectGrid.Rows[index].Selected = true;
            string id = ProjectGrid.SelectedCells[0].Value.ToString();
            try
            {
                //SqlCommand con = new SqlCommand("SELECT Id FROM STUDENT WHERE id = ", con);
                //SqlDataReader reader = con.ExecuteReader();
                
                SqlCommand cmd = new SqlCommand(" Update Project SET Description = '"+ProId.Text+"', Title = '"+Title.Text+"';" , con);
                cmd.ExecuteNonQuery();
                ProjectGrid.Rows.RemoveAt(index);
                ProjectGrid.DataSource = dt;
                MessageBox.Show("Chal gya");

            }

            catch
            {
                MessageBox.Show("Can not update data.");
            }
            con.Close();
        }
        private void Del_rec()
        {
            con.Open();

            int index = ProjectGrid.CurrentCell.RowIndex;
            ProjectGrid.Rows[index].Selected = true;
            string id = ProjectGrid.SelectedCells[0].Value.ToString();
            try
            {
                //SqlCommand con = new SqlCommand("SELECT Id FROM STUDENT WHERE id = ", con);
                //SqlDataReader reader = con.ExecuteReader();
                SqlCommand cmd = new SqlCommand(" DELETE FROM Project WHERE Id = '" + id + "';", con);
                cmd.ExecuteNonQuery();
                ProjectGrid.Rows.RemoveAt(index);
                ProjectGrid.DataSource = dt;
                MessageBox.Show("Chal gya");
                
            }

            catch
            {
                MessageBox.Show("Can not delete data.");
            }
            con.Close();
        }

        private void ProjectGrid_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            ProjectGrid_CellContentClick(sender, e);

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Evaluation evalfm = new Evaluation();
            evalfm.Show();
        }
    }
}
