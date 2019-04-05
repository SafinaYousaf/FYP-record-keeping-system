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

        DataTable dt1;
        SqlDataAdapter da2;

        public void disp_data_GE()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select TOP(10) * from [GroupEvaluation];";
            cmd.ExecuteNonQuery();
            
            dt1 = new DataTable();
            da2 = new SqlDataAdapter(cmd);
            da2.Fill(dt1);
            GEdatagrid.DataSource = dt1; //evaldisGrid is name of data grid view present on form
            con.Close();
            //delete buttons
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            GEdatagrid.Columns.Add(btn);
            btn.HeaderText = "Delete";
            btn.Text = "Delete";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            //uodate
            DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
            GEdatagrid.Columns.Add(btn1);
            btn1.HeaderText = "Update";
            btn1.Text = "Update";
            btn1.Name = "btn1";
            btn1.UseColumnTextForButtonValue = true;

        }
        private void button10_Click(object sender, EventArgs e)
        {
            edpanbt.Hide();
            addpan.Hide();
            panel2.Hide();
            shoeGE.Hide();
            
            panel1.Show();

        }

        private void Evaluation_Load(object sender, EventArgs e)
        {
            edpanbt.Hide();
            addpan.Hide();
            panel2.Hide();
            panel3.Hide();
            
            shoeGE.Hide();
            disp_data_GE();
            disp_data();
            panel1.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel2.Hide();
            shoeGE.Hide();
            //addgrpeval.Hide();
            edpanbt.Hide();
            addpan.Show();
            
            
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            ///panel2.Hide();
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
                this.Hide();
                Evaluation eval = new Evaluation();
                eval.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Update_rec();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addpan.Hide();
            edpanbt.Hide();
            //panel2.Show();
            panel1.Show();
            //disp_data();
        }
        
        
        
        private void button8_Click(object sender, EventArgs e)
        {/*
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Evaluation WHERE ([ID] = @ID)", con);
            check_User_Name.Parameters.AddWithValue("ID", //IdDel.Text);
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
            */

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
                MessageBox.Show("Done.");
                this.Hide();
                Evaluation obj = new Evaluation();
                obj.Show();

            }

            catch
            {
                MessageBox.Show("Something Went wrong.");
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
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM GroupEvaluation WHERE EvaluationId = '" + id + "'", con);
            SqlDataReader reader = cmd1.ExecuteReader();
            if (reader.HasRows)
            {
                con.Close();
                MessageBox.Show("Evaluation Is Present in group.");
            }
            else
            {
                
                    SqlCommand cmd = new SqlCommand(" DELETE FROM Evaluation WHERE Id = '" + id + "';", con);

                    cmd.ExecuteNonQuery();

                    evaldispGrid.Rows.RemoveAt(index);
                    evaldispGrid.DataSource = dt;
                    MessageBox.Show("Evaluation Deleted.");
                    


                
            }
            con.Close();
        }
        private void evaldispGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
                
                if (e.ColumnIndex == 4)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this Evaluation?", "Evaluation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        
                        Delete_Rec();
                        // Delete();
                    }
                    else if (result == DialogResult.No)
                    {
                        this.Hide();
                        Evaluation obj = new Evaluation();
                        obj.Show();
                    }
                }
                if (e.ColumnIndex == 5)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to update this Evaluation", "Evaluation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        
                        panel1.Hide();
                         //panel2.Hide();
                          addpan.Hide();
                          edpanbt.Show();
                    
                }
                    else if (result == DialogResult.No)
                    {
                        this.Hide();
                        Evaluation obj = new Evaluation();
                        obj.Show();
                    }


                }
            }

        private void button3_Click_1(object sender, EventArgs e)
        {
            edpanbt.Hide();
            addpan.Hide();
            panel3.Hide();
            panel1.Hide();

            shoeGE.Hide();
            
            panel2.Show();

            /// addgrpeval.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            edpanbt.Hide();
            addpan.Hide();
            panel2.Hide();
            panel1.Hide();

            panel3.Hide(); 
            shoeGE.Show();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void shoeGE_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GEdatagrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Group Evaluation?", "Group Evaluation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DeleteGE();
                }
                else if (result == DialogResult.No)
                {
                    this.Hide();
                    Evaluation obj = new Evaluation();
                    obj.Show();
                }
            }
            if (e.ColumnIndex == 5)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to update this Group Evaluation?", "Group Evaluation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    edpanbt.Hide();
                    addpan.Hide();
                    panel2.Hide();
                    panel1.Hide();

                    shoeGE.Hide();
                    
                    panel3.Show();
                }
                else if (result == DialogResult.No)
                {
                    this.Hide();
                    Evaluation obj = new Evaluation();
                    obj.Show();
                }


            }
        }

        //delete content of grid contaning group eval
        private void DeleteGE()
        {
            con.Close();
            con.Open();

            int index = GEdatagrid.CurrentCell.RowIndex;
            GEdatagrid.Rows[index].Selected = true;
            string gi = GEdatagrid.SelectedCells[0].Value.ToString();
            string ei = GEdatagrid.SelectedCells[1].Value.ToString();
            try
            {

                SqlCommand cmd = new SqlCommand(" DELETE FROM GroupEvaluation WHERE GroupId = '" + gi + "' AND EvaluationId = '" + ei + "';", con);

                cmd.ExecuteNonQuery();
                
                GEdatagrid.Rows.RemoveAt(index);
                GEdatagrid.DataSource = dt1;
                MessageBox.Show("Group Evaluation Deleted.");
                

            }

            catch
            {
                con.Close();
                MessageBox.Show("Something went wrong.");
            }
            con.Close();
        }

        private void addGEbt_Click(object sender, EventArgs e)
        {

            con.Close();
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT GroupId FROM GroupEvaluation WHERE ([GroupId] = @GI) AND ([EvaluationId] = @EI)", con);

            check_User_Name.Parameters.AddWithValue("GI", GItxt.Text);
            check_User_Name.Parameters.AddWithValue("EI", GItxt.Text);
            SqlDataReader reader = check_User_Name.ExecuteReader();
            if (reader.HasRows)
            {
                con.Close();
                MessageBox.Show("Already exists.");

            }
            else
            {


                //string id = evaldispGrid.SelectedCells[0].Value.ToString();
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM Evaluation WHERE Id = '" + EItxt.Text + "'", con);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                if (!(reader1.HasRows))
                {
                    con.Close();
                    MessageBox.Show("Evaluation Is Not Present Defined.");
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("Select TotalMarks from Evaluation WHERE Id = '" + EItxt.Text + "';", con);
                    object countstd1 = cmd.ExecuteScalar();
                    int countstd = 0;
                    if (!(countstd1 == DBNull.Value))
                    {
                        countstd = Convert.ToInt32(countstd1);
                    }

                    if (countstd <= Convert.ToInt32(OMtxt.Text))
                    {
                        con.Close();
                        MessageBox.Show("Obtained marks must not excedd TotalMarks.");
                    }
                    else
                    {

                    
                    cmd1 = new SqlCommand("SELECT * FROM [Group] WHERE Id = '" + GItxt.Text + "'", con);
                    SqlDataReader reader2 = cmd1.ExecuteReader();
                    if (!(reader2.HasRows))
                    {
                        con.Close();
                        MessageBox.Show("Group does not exists.");
                    }
                    else
                    {
                        try
                        {
                            cmd = con.CreateCommand();
                            cmd = new SqlCommand("INSERT INTO GroupEvaluation(GroupId,EvaluationId,ObtainedMarks,EvaluationDate) VALUES(@GroupId,@EvaluationId,@ObtainedMarks,@EvaluationDate);SELECT SCOPE_IDENTITY();", con);
                            cmd.Parameters.AddWithValue("@GroupId", GItxt.Text);
                            cmd.Parameters.AddWithValue("@EvaluationId", EItxt.Text);
                            cmd.Parameters.AddWithValue("@ObtainedMarks", OMtxt.Text);
                            cmd.Parameters.AddWithValue("@EvaluationDate", DateTime.Parse(datetx.Text));
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Added.");
                            this.Hide();
                            Evaluation obj = new Evaluation();
                            obj.Show();
                        }
                        catch
                        {
                            MessageBox.Show("Something went wrong.");
                        }
                    }
                }
                }
            }
        }
        private void update_GE()
        {
            con.Open();
            int index = GEdatagrid.CurrentCell.RowIndex;
            GEdatagrid.Rows[index].Selected = true;
            string gi = GEdatagrid.SelectedCells[0].Value.ToString();
            string ei = GEdatagrid.SelectedCells[1].Value.ToString();

            //UPDATE Advisor SET Designation = '" + g + "', Salary = '" + Salary.Text + "' WHERE ID = '" + ID.Text + "';
            SqlCommand cmd = new SqlCommand("UPDATE GroupEvaluation SET GroupId = @GI,EvaluationId = @EI ,ObtainedMarks = @OM,EvaluationDate =@eval WHERE GroupId='" + gi + "' AND EvaluationId='" + ei + "';", con);
            cmd.Parameters.AddWithValue("@GI", GItxtup.Text);
            cmd.Parameters.AddWithValue("@EI", EItxtup.Text);
            cmd.Parameters.AddWithValue("@OM", OMtxtup.Text);
            cmd.Parameters.AddWithValue("@eval",DateTime.Parse(dateTimeEG.Text));
            cmd.ExecuteNonQuery();
            MessageBox.Show("Updated.");
        }

        private void UpdateGE_Click(object sender, EventArgs e)
        {
            update_GE();
            this.Hide();
            Evaluation obj = new Evaluation();
            obj.Show();
        }
        int count = 0;
        int countt = 0;
        private void OMtxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if(count>0 || countt >0)
            {
                MessageBox.Show("Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8 ||e.KeyChar== 46 || e.KeyChar == 47)
            {


                e.Handled = false;
                if(e.KeyChar== 46 )
                {
                    count += 1;
                }
                if (e.KeyChar == 46)
                {
                    countt += 1;
                }

            }
            else
            {
                MessageBox.Show("Please Enter only Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Handled = true;

            }
        }

        private void GItxt_KeyPress(object sender, KeyPressEventArgs e)
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

        private void EItxt_KeyPress(object sender, KeyPressEventArgs e)
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

        private void GItxtup_KeyPress(object sender, KeyPressEventArgs e)
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

        private void EItxtup_KeyPress(object sender, KeyPressEventArgs e)
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
        int count1 = 0;
        int countt1 = 0;
        private void OMtxtup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (count1 > 0 || countt1 > 0)
            {
                MessageBox.Show("Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8 || e.KeyChar == 46 || e.KeyChar == 47)
            {


                e.Handled = false;
                if (e.KeyChar == 46)
                {
                    count1 += 1;
                }
                if (e.KeyChar == 46)
                {
                    countt1 += 1;
                }

            }
            else
            {
                MessageBox.Show("Please Enter only Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Handled = true;

            }

        }

        private void Namme_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space)
            {


                e.Handled = false;

            }
            else
            {
                MessageBox.Show("Please Enter only Alphabets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Handled = true;

            }
        }

        private void TotalMarks_KeyPress(object sender, KeyPressEventArgs e)
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

        private void weightage_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Group_Click(object sender, EventArgs e)
        {
            Group grp = new Group();
            grp.Show();
        }
    }
}
