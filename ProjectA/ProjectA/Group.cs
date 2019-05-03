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

    public partial class Group : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;MultipleActiveResultSets=true;Integrated Security=True");
        public Group()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        SqlDataAdapter da;
        public void disp_data()
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP(10) * FROM GroupProject AS GP ORDER BY GP.GroupId DESC";
            cmd.ExecuteNonQuery();
            //DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dispgrid.DataSource = dt; //Student is name of data grid view present on form
            con.Close();
            //delete buttons
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            dispgrid.Columns.Add(btn);
            btn.HeaderText = "Delete";
            btn.Text = "Delete";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            //update
            DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
            dispgrid.Columns.Add(btn1);
            btn1.HeaderText = "Update";
            btn1.Text = "Update";
            btn1.Name = "btn1";
            btn1.UseColumnTextForButtonValue = true;

        }
        private void button5_Click(object sender, EventArgs e)
        {
            Display studfrm = new Display();
            studfrm.Show();
        }

        private void adv_Click(object sender, EventArgs e)
        {
            Advisorr advisorfrm = new Advisorr();
            advisorfrm.Show();
        }

        private void pro_Click(object sender, EventArgs e)
        {
            Project pro = new Project();
            pro.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ProjectAdvisor proadd = new ProjectAdvisor();
            proadd.Show();
        }

        private void eval_Click(object sender, EventArgs e)
        {
            Evaluation evalfm = new Evaluation();
            evalfm.Show();
        }

        private void Adddata_Click(object sender, EventArgs e)
        {
            con.Close();
            con.Open();
            if (string.IsNullOrWhiteSpace(grpid.Text))
            {
                con.Close();
                MessageBox.Show("Plese enter Project Id.");

            }
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM Project WHERE Id = '" +grpid.Text+ "'", con);
            SqlDataReader reader = cmd1.ExecuteReader();
            if (!(reader.HasRows))
            {
                con.Close();
                MessageBox.Show("Project is Not Defined.");
            }
            else
            {
                SqlCommand check_User_Name = new SqlCommand("SELECT ProjectId FROM [GroupProject] WHERE ([ProjectId] = @ID)", con);
                check_User_Name.Parameters.AddWithValue("ID", grpid.Text);
                SqlDataReader reader1 = check_User_Name.ExecuteReader();

                if (reader1.HasRows)
                {
                    con.Close();
                    MessageBox.Show("Project group already exist against provided group Id.");
                }

                else
                {
                    try
                    {
                        SqlCommand cmd = con.CreateCommand();
                        cmd = new SqlCommand("INSERT INTO [dbo].[Group](Created_On) VALUES(@Created_On);SELECT SCOPE_IDENTITY();", con);
                        cmd.Parameters.AddWithValue("@Created_On", DateTime.Parse(dated.Text));
                        int modified = 0;

                        modified = Convert.ToInt32(cmd.ExecuteScalar());
                        try
                        {
                            cmd = new SqlCommand("INSERT INTO [GroupProject](GroupId, ProjectId,AssignmentDate) values('" + modified + "','" + grpid.Text + "','" + DateTime.Parse(dated.Text) + "');", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Group and Group Project Is created Successfully.");
                            this.Hide();
                            Group grpfm = new Group();
                            grpfm.Show();
                        }
                        catch
                        {
                            con.Close();
                            MessageBox.Show("something went wrong.");
                        }

                    }
                    catch
                    {
                        con.Close();
                        MessageBox.Show("something went wrong.");
                    }
                }
            }









        }

        private void grpid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8)
            {


                e.Handled = false;

            }
            else
            {
                MessageBox.Show("Please Enter only Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Handled = true;

            }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Group_Load(object sender, EventArgs e)
        {
            disp_data();
            panel1.Hide();// add info
            updpan.Hide();
            dispgrppan.Show();
        }
        private void Delete()
        {
            
            con.Close();
            con.Open();

            int index = dispgrid.CurrentCell.RowIndex;
            dispgrid.Rows[index].Selected = true;
            string id = dispgrid.SelectedCells[1].Value.ToString();
           

            try
            {

                SqlCommand cmd = new SqlCommand(" DELETE FROM [GroupProject] WHERE GroupId  = '" + id + "';", con);

                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(" DELETE FROM [Group] WHERE Id = '" + id + "';", con);

                cmd.ExecuteNonQuery();
                dispgrid.Rows.RemoveAt(index);
                dispgrid.DataSource = dt;
                MessageBox.Show("Group Deleted.");

            }

            catch
            {
                MessageBox.Show("Something went wrong.");
            }
            
            con.Close();
            
        }

        private void dispgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Group?", "Group", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Delete();
                }
                else if (result == DialogResult.No)
                {
                    this.Hide();
                    Group obj = new Group();
                    obj.Show();
                }
            }
            if (e.ColumnIndex == 4)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Group?", "Group", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {


                    dispgrppan.Hide();
                    panel1.Hide();// add info
                    updpan.Show();
                }
                else
                {
                    this.Hide();
                    Group obj = new Group();
                    obj.Show();

                }

            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            try
            {
                con.Close();
                con.Open();
                int index = dispgrid.CurrentCell.RowIndex;
                dispgrid.Rows[index].Selected = true;
                string id = dispgrid.SelectedCells[1].Value.ToString();
                SqlCommand cmd = new SqlCommand("UPDATE GroupProject SET ProjectId = @ProjectId,GroupId = @GroupId ,AssignmentDate = @date WHERE GroupId ='" + id + "';", con);
                cmd.Parameters.AddWithValue("@ProjectId", upproid.Text);
                cmd.Parameters.AddWithValue("@GroupId", id);
                cmd.Parameters.AddWithValue("@date", DateTime.Parse(updated.Text));
                MessageBox.Show("done");

                cmd.ExecuteNonQuery();
                dispgrid.Rows.RemoveAt(index);
                dispgrid.DataSource = dt;
                MessageBox.Show("Group Deleted.");
            }
            catch
            {
                con.Close();
                MessageBox.Show("something went wrong.");
            }
        }

        private void Addgrp_Click(object sender, EventArgs e)
        {
            updpan.Hide();
            dispgrppan.Hide();
            panel1.Show();// add info
            
        }

        private void grp_Click(object sender, EventArgs e)
        {
           
            panel1.Hide();// add info
            updpan.Hide();
            dispgrppan.Show();
        }

        private void Group_FormClosed(object sender, FormClosedEventArgs e)
        {

            Environment.Exit(0);
        }
    }
}
