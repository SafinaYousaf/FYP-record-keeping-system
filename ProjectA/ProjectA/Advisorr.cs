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
    public partial class Advisorr : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;MultipleActiveResultSets=true;Integrated Security=True");
        public Advisorr()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Display stuFm = new Display();
            stuFm.Show();
        }
        DataTable dt = new DataTable();
        public void disp_data()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP(10) A.Designation, A.Salary FROM Advisor AS A";
            cmd.ExecuteNonQuery();
           // DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            addDis.DataSource = dt; //Student is name of data grid view present on form
            con.Close();
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            addDis.Columns.Add(btn);
            btn.HeaderText = "Delete";
            btn.Text = "Delete";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;

            DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
            addDis.Columns.Add(btn1);
            btn1.HeaderText = "Update";
            btn1.Text = "Update";
            btn1.Name = "btn1";
            btn1.UseColumnTextForButtonValue = true;

        }
        private void button3_Click(object sender, EventArgs e)
        {
            AddAdvisor.Hide();
            panel1.Hide();
            panel2.Hide();
            panel3.Hide();
            AdvisorDisplay.Show();
            //disp_data();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdvisorDisplay.Hide();
            panel1.Hide();
            panel3.Hide();
            AddAdvisor.Show();
            panel2.Show();
        }

        private void Advisorr_Load(object sender, EventArgs e)
        {
            AddAdvisor.Hide();
            AdvisorDisplay.Show();
            
            panel2.Hide();
            panel3.Hide();
            panel1.Hide();
            disp_data(); 
        }
        int val;
        private int Designation_look(string gen)
        {
            string query;

            if (gen == "Professor")
                query = "SELECT Id FROM Lookup where Category= 'DESIGNATION' AND Value = 'Professor'";
            if (gen == "Associate Professor")
                query = "SELECT Id FROM Lookup where Category= 'DESIGNATION' AND Value = 'Associate Professor'";
            if (gen == "Assisstant Professor")
                query = "SELECT Id FROM Lookup where Category= 'DESIGNATION' AND Value = 'Assisstant Professor'";
            if (gen == "Lecturer")
                query = "SELECT Id FROM Lookup where Category= 'DESIGNATION' AND Value = 'Lecturer'";
            else
                query = "SELECT Id FROM Lookup where Category= 'DESIGNATION' AND Value = 'Industry Professional'";
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                val = int.Parse(reader[0].ToString());
            }
            return val;

        }
       
        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ID.Text) || string.IsNullOrWhiteSpace(Genderr.Text))
            {
                MessageBox.Show("ID and Designation is compulsory.");
            }
            else
            {
                con.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Advisor WHERE ([ID] = @ID)", con);
                check_User_Name.Parameters.AddWithValue("ID", ID.Text);
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
                    cmd = new SqlCommand("INSERT INTO Advisor(ID, Designation,Salary) VALUES(@ID,@Designation,@Salary)", con);
                    cmd.Parameters.AddWithValue("@ID", ID.Text);
                    string desig = Genderr.Text.ToString();
                    int g = Designation_look(desig);
                    cmd.Parameters.AddWithValue("@Designation", g);
                    cmd.Parameters.AddWithValue("@Salary", Salary.Text);


                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data Inserted Successfully");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Design_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Edit_Click(object sender, EventArgs e)
        {
            //update_rec();
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Advisor WHERE ([ID] = @ID)", con);
            check_User_Name.Parameters.AddWithValue("ID", ID.Text);
            SqlDataReader reader = check_User_Name.ExecuteReader();
            if (reader.HasRows)
            {
                
                string desig = Genderr.Text.ToString();
                int g = Designation_look(desig);
                SqlCommand cmd = new SqlCommand("UPDATE Advisor SET Designation = '" + g + "', Salary = '" + Salary.Text + "' WHERE ID = '" + ID.Text + "'; ", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Updated Sucessfully");
            }
            else
            {
                con.Close();
                MessageBox.Show("Record does not exists.");
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AdvisorDisplay.Hide();
            panel2.Hide();
            AddAdvisor.Show();
            panel1.Show();
            panel3.Hide();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Advisor WHERE ([ID] = @ID)", con);
            check_User_Name.Parameters.AddWithValue("ID", ID.Text);
            SqlDataReader reader = check_User_Name.ExecuteReader();
            if (reader.HasRows)
            {

                string desig = Genderr.Text.ToString();
                int g = Designation_look(desig);
                SqlCommand cmd = new SqlCommand("DELETE FROM Advisor WHERE ID='" + IDdel.Text + "';", con);
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

        private void button6_Click(object sender, EventArgs e)
        {
            
            panel2.Hide();
            AddAdvisor.Hide();
            panel1.Hide();
            AdvisorDisplay.Show();
            panel3.Show();

        }
        private void Delete_rec()
        {
            con.Open();

            int index = addDis.CurrentCell.RowIndex;
            addDis.Rows[index].Selected = true;
            string id = addDis.SelectedCells[0].Value.ToString();
            try
            {

                SqlCommand cmd = new SqlCommand(" DELETE FROM Advisor WHERE Id = '" + id + "';", con);
                cmd.ExecuteNonQuery();
                addDis.Rows.RemoveAt(index);
                addDis.DataSource = dt;
                MessageBox.Show("Chal gya");

            }

            catch
            {
                MessageBox.Show("Naaaaaaaahi hua");
            }
            con.Close();
        }
        private void update_rec()
        {
            con.Open();

            int index = addDis.CurrentCell.RowIndex;
            addDis.Rows[index].Selected = true;
            string id = addDis.SelectedCells[0].Value.ToString();
            string desig = Genderr.Text.ToString();
            int g = Designation_look(desig);

            SqlCommand cmd = new SqlCommand(" Update Advisor SET ID= '" + id + "', Designation = '" + g + "', Salary= '" + Salary.Text + "';", con);
            cmd.ExecuteNonQuery();
            addDis.Rows.RemoveAt(index);
            addDis.DataSource = dt;
            MessageBox.Show("Chal gya");
            /*try
            {
                string desig = Genderr.Text.ToString();
                int g = Designation_look(desig);

                SqlCommand cmd = new SqlCommand(" Update Advisor SET ID= '"+id+"', Designation = '"+g+"', Salary= '"+Salary.Text+"';", con);
                cmd.ExecuteNonQuery();
                addDis.Rows.RemoveAt(index);
                addDis.DataSource = dt;
                MessageBox.Show("Chal gya");

            }

            catch
            {
                MessageBox.Show("Naaaaaaaahi hua");
            }
            */
            con.Close();
            
        }

        private void addDis_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Person?", "Person", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Delete_rec();
                }
                
            }
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to update this Person", "person", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    AdvisorDisplay.Hide();
                    panel2.Hide();
                    AddAdvisor.Show();
                    panel1.Show();
                    panel3.Hide();
                }
                


            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Project profm = new Project();
            profm.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProjectAdvisor proad = new ProjectAdvisor();
            proad.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Evaluation evalfm = new Evaluation();
            evalfm.Show();
        }
    }
}
