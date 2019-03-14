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
    public partial class Evaluation : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;MultipleActiveResultSets=true;Integrated Security=True");
        public Evaluation()
        {
            InitializeComponent();
        }
        DataTable dt;
        public void disp_data()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select TOP(10) * from Evaluation;";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            evaldispGrid.DataSource = dt; //evaldisGrid is name of data grid view present on form
            con.Close();
            //delete buttons
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            evaldispGrid.Columns.Add(btn);
            btn.HeaderText = "Delete";
            btn.Text = "Delete";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            //uodate
            DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
            evaldispGrid.Columns.Add(btn1);
            btn1.HeaderText = "Update";
            btn1.Text = "Update";
            btn1.Name = "btn1";
            btn1.UseColumnTextForButtonValue = true;

        }
        private void button10_Click(object sender, EventArgs e)
        {
            Evaluation evalfm = new Evaluation();
            evalfm.Show();
            addpan.Hide();
            edpanbt.Hide();
            panel2.Hide();
            panel1.Show();
            //disp_data();

        }

        private void Evaluation_Load(object sender, EventArgs e)
        {
            edpanbt.Hide();
            addpan.Hide();
            panel2.Hide();
            panel1.Show();
            disp_data();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel2.Hide();
            edpanbt.Hide();
            addpan.Show();
            
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel2.Hide();
            addpan.Hide();
            edpanbt.Show();
        }

        private void addrec_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Namme.Text) || string.IsNullOrWhiteSpace(TotalMarks.Text) || string.IsNullOrWhiteSpace(weightage.Text) )
            {
                MessageBox.Show("An evaluation must have defined name, marks and weightage <=100");
            }
           
            else
            {
                
                con.Close();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd = new SqlCommand("INSERT INTO Evaluation(Name, TotalMarks,TotalWeightage) VALUES(@Name,@TotalMarks,@TotalWeightage)", con);
                cmd.Parameters.AddWithValue("@Name", Namme.Text);
                cmd.Parameters.AddWithValue("@TotalMarks", TotalMarks.Text);
                cmd.Parameters.AddWithValue("@TotalWeightage", weightage.Text);


                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data Inserted Successfully");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Update_rec();
            /*
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Evaluation WHERE ([ID] = @ID)", con);
            check_User_Name.Parameters.AddWithValue("ID", Idbx.Text);
            SqlDataReader reader = check_User_Name.ExecuteReader();
            if (reader.HasRows)
            {

                
                SqlCommand cmd = new SqlCommand("UPDATE Evaluation SET Name = '" + nameebx.Text + "', TotalMarks = '" + TMbx.Text + "', TotalWeightage = '"+TWbx.Text+"' WHERE ID = '" + Idbx.Text + "'; ", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Updated Sucessfully");
            }
            else
            {
                con.Close();
                MessageBox.Show("Record does not exists.");
            }
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addpan.Hide();
            edpanbt.Hide();
            panel2.Show();
            panel1.Show();
            //disp_data();
        }
        
        
        
        private void button8_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Evaluation WHERE ([ID] = @ID)", con);
            check_User_Name.Parameters.AddWithValue("ID", IdDel.Text);
            SqlDataReader reader = check_User_Name.ExecuteReader();
            if (reader.HasRows)
            {


                SqlCommand cmd = new SqlCommand("DELETE FROM Evaluation WHERE ID='" + IdDel.Text + "';", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Deleted Sucessfully");
                //disp_data();
            }
            else
            {
                con.Close();
                MessageBox.Show("Record does not exists.");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Display studisp = new Display();
            studisp.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Advisorr AdvisorForm = new Advisorr();
            AdvisorForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Project profm = new Project();
            profm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ProjectAdvisor proadvfm = new ProjectAdvisor();
            proadvfm.Show();
        }

        private void addpan_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Update_rec()
        {
            con.Open();

            int index = evaldispGrid.CurrentCell.RowIndex;
            evaldispGrid.Rows[index].Selected = true;
            string id = evaldispGrid.SelectedCells[0].Value.ToString();
            try
            {
                
                SqlCommand cmd = new SqlCommand(" UPDATE Evaluation SET Name ='"+nameebx.Text+ "', TotalMarks = '"+TMbx.Text+ "', TotalWeightage= '"+TWbx.Text+"' WHERE Id = '" + id + "';", con);
                cmd.ExecuteNonQuery();
                evaldispGrid.Rows.RemoveAt(index);
                evaldispGrid.DataSource = dt;
                MessageBox.Show("Chal gya");
                this.Hide();
                Display obj = new Display();
                obj.Show();

            }

            catch
            {
                MessageBox.Show("Naaaaaaaahi hua");
            }
            con.Close();
        }
        private void Delete_Rec()
        {
            con.Close();
            con.Open();

            int index = evaldispGrid.CurrentCell.RowIndex;
            evaldispGrid.Rows[index].Selected = true;
            string id = evaldispGrid.SelectedCells[0].Value.ToString();
            try
            {

                SqlCommand cmd = new SqlCommand(" DELETE FROM Evaluation WHERE Id = '" + id + "';", con);

                cmd.ExecuteNonQuery();
                
                evaldispGrid.Rows.RemoveAt(index);
                evaldispGrid.DataSource = dt;
                MessageBox.Show("Evaluation Deleted.");

            }

            catch
            {
                MessageBox.Show("Something went wrong.");
            }
            con.Close();
        }
        private void evaldispGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
                MessageBox.Show("ok1");
                if (e.ColumnIndex == 4)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this Evaluation?", "Evaluation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        MessageBox.Show("ok");
                    Delete_Rec();
                        // Delete();
                    }
                    else if (result == DialogResult.No)
                    {
                        this.Hide();
                        Display obj = new Display();
                        obj.Show();
                    }
                }
                if (e.ColumnIndex == 5)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to update this Evaluation", "Evaluation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        MessageBox.Show("ok");
                        panel1.Hide();
                         panel2.Hide();
                          addpan.Hide();
                          edpanbt.Show();
                    /*panel1.Hide();
                    panel2.Hide();
                    //panel6.Hide();
                    panel4.Hide();
                    //panel3.Hide();
                    AddStudent.Hide();
                    panel5.Hide();
                    updatepan.Show();
                    //Update_rec();*/
                }
                    else if (result == DialogResult.No)
                    {
                        this.Hide();
                        Display obj = new Display();
                        obj.Show();
                    }


                }
            }
    }
}
