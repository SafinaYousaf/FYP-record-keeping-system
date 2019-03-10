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
    public partial class Display : Form
    {
        EditDelForm addstu = new EditDelForm();
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;MultipleActiveResultSets=true;Integrated Security=True");
        public Display()
        {
            InitializeComponent();
        }
        public void disp_data()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP(10) P.Id,P.FirstName,P.LastName,P.Contact,P.Email,P.DateOfBirth,P.Gender FROM Person AS P ORDER BY P.Id DESC";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            StudentGrid.DataSource = dt; //Student is name of data grid view present on form
            con.Close();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            panel1.Hide();
            panel2.Hide();
            panel4.Hide();
            panel3.Hide();
            AddStudent.Hide();
            panel5.Hide();
            panel6.Hide();
          

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            panel2.Hide();
            panel4.Hide();
            AddStudent.Hide();
            panel5.Hide();
            panel1.Show();
            panel3.Show();
            panel2.Show();
            disp_data();
        }

        private void StudentGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Add_Click(object sender, EventArgs e)
        {
            //addstu.Show();
            panel1.Hide();
            panel2.Hide();
            panel6.Hide();
            panel4.Hide();
            panel3.Hide();
            AddStudent.Hide();
            panel5.Show();



            /*
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into Student values('" + id.Text + "','" + RegNo.Text + "')";
            cmd.ExecuteNonQuery(); cmd.CommandText = "insert into Person values('" + id.Text + "','" + FName.Text + "','" + LName.Text + "','" + cont.Text + "','" + Email.Text + "','" + Dob.Text + "','" + Gender.Text + "')"
            con.Close();
            disp_data(); //display data when inserted into table
            //Message box
            MessageBox.Show("Inserted");
            */
        }

        private void AddStudent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Adds_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void adds_Click_1(object sender, EventArgs e)
        {
            /*
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into Student values('" + stdID.Text + "','" + StdReg.Text + "')";
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Inserted");
            */

        }

        private void button5_Click(object sender, EventArgs e)
        {
            disp_data();
            panel5.Hide();
            panel6.Hide();
            AddStudent.Hide();
            panel1.Show();
            panel2.Show();
            panel3.Show();
            panel4.Show();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Student WHERE ([ID] = @ID)", con);
           // SqlCommand check_User_Name = new SqlCommand("SELECT * FROM Table WHERE ([user] = @user)", conn);
            check_User_Name.Parameters.AddWithValue("ID", stuId.Text);
            SqlDataReader reader = check_User_Name.ExecuteReader();
            if (reader.HasRows)
            {
                con.Close();

                MessageBox.Show("exixts");
                

            }
            
            else
            {
                con.Close();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into Student values('" + stuId.Text + "','" + regNo.Text + "')";
                cmd.ExecuteNonQuery();
           
                con.Close();

                //Message box
                MessageBox.Show("Inserted");

                
            }
            /*
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into Student values('" + stuId.Text + "','" + regNo.Text + "')";
            cmd.ExecuteNonQuery();
            con.Close();

            //Message box
            MessageBox.Show("Inserted");
            */
        }
        int value;
        private int Gender_look(string gen)
        {
            string query;
            
            if (gen == "Male")
                query = "SELECT Id FROM Lookup where Category= 'GENDER' AND Value = 'Male'";
            else
                query = "SELECT Id FROM Lookup where Category= 'GENDER' AND Value = 'Female'";
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                value = int.Parse(reader[0].ToString());
            }
            return value;

        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Fname.Text) || string.IsNullOrWhiteSpace(email.Text))
            {
                MessageBox.Show("Firstname and email is compulsory.");
            }
            /*
             * if (!this.e.Text.Contains('@') || !this.txtEmail.Text.Contains('.'))
            {
                MessageBox.Show("Please Enter A Valid Email", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
            


            /* SqlCommand cmd = con.CreateCommand();
             cmd.CommandType = CommandType.Text;
             int g = (int)Genderr.SelectedValue;

             cmd.CommandText = "INSERT INTO Person (FirstName,LastName,Email,Contact,DateofBirth) VALUES('" + Fname.Text + "','" + Lname.Text + "','" + email.Text + "','" + Cont.Text + "','" + dob.Text + "')";

             cmd.ExecuteNonQuery();
             con.Close();
             */
            else{
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd = new SqlCommand("INSERT INTO PERSON(FirstName,LastName,Contact,Email,DateOfBirth,Gender) VALUES(@FirstName,@LastName,@Contact,@Email,@DateofBirth,@Gender)", con);
                cmd.Parameters.AddWithValue("@FirstName", Fname.Text);
                cmd.Parameters.AddWithValue("@LastName", Lname.Text);
                cmd.Parameters.AddWithValue("@Contact", Cont.Text);
                cmd.Parameters.AddWithValue("@Email", email.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", dob.Text);
                string genn = Genderr.Text.ToString();
                int g = Gender_look(genn);
                cmd.Parameters.AddWithValue("@Gender", g);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data Inserted Successfully");
            }

           //handling panels

            panel4.Hide();
            panel3.Hide();
            AddStudent.Hide();
            panel5.Hide();
            panel1.Show();
            panel2.Show();
            panel3.Show();

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Cont_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Fname_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);

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

        private void Lname_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Fname_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //EditDelForm edf = new EditDelForm();
            //edf.Show();
            
            
            panel4.Hide();
            
            AddStudent.Hide();
            panel5.Hide();
            panel1.Show();
            panel3.Show();
            panel2.Show();
            panel6.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        { 
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Student WHERE ([ID] = @ID)", con);
            // SqlCommand check_User_Name = new SqlCommand("SELECT * FROM Table WHERE ([user] = @user)", conn);
            check_User_Name.Parameters.AddWithValue("ID", DEid.Text);
            SqlDataReader reader = check_User_Name.ExecuteReader();
            if (reader.HasRows)
            {
                con.Close();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM STUDENT WHERE ([ID] = '"+DEid.Text+"')";
                cmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("DELETED");


            }

            else
            {
                con.Close();

                //Message box
                MessageBox.Show("Record does not exists exits");


            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Advisorr AdvisorForm = new Advisorr();
            AdvisorForm.Show();
        }
    }
}
