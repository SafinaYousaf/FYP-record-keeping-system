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
using System.Text.RegularExpressions;

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
            cmd.CommandText = "SELECT TOP(10) A.Id, P.FirstName+' '+LastName AS [Name], A.Designation, A.Salary, P.Email, P.Gender FROM Advisor AS A JOIN Person AS P ON A.Id = P.Id ";
            cmd.ExecuteNonQuery();
          
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            addDis.DataSource = dt; 
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
            //panel3.Hide();
            AdvisorDisplay.Show();
            //disp_data();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdvisorDisplay.Hide();
            panel1.Hide();
            //panel3.Hide();
            AddAdvisor.Show();
            panel2.Show();
        }

        private void Advisorr_Load(object sender, EventArgs e)
        {
            AddAdvisor.Hide();
            AdvisorDisplay.Show();
            
            panel2.Hide();
            //panel3.Hide();
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
        private void button4_Click(object sender, EventArgs e)
        {
            con.Close();
            con.Open();
            string emailPattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$"; // Email address pattern
            string phonePattern = @"^03[0-9]{9}$";
           
            string namepat = @"^[A-Z]{1}[a-zA-Z\s\'-]*$";
            bool isEmailValid = Regex.IsMatch(email.Text, emailPattern);
            bool isPhoneValid = Regex.IsMatch(cont.Text, phonePattern);
           
            bool isFNameValid = Regex.IsMatch(fname.Text, namepat);
            bool isLNameValid = Regex.IsMatch(lname.Text, namepat);
            String Salarypat = @"^[0-9]*$";
            bool isValidSalary = Regex.IsMatch(Salary.Text, Salarypat);
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM PERSON WHERE Email = '" + email.Text + "'", con);
            SqlDataReader reader = cmd1.ExecuteReader();
            cmd1 = new SqlCommand("SELECT * FROM PERSON WHERE Contact = '" + cont.Text + "'", con);
            SqlDataReader reader2 = cmd1.ExecuteReader();
            if (reader.HasRows)
            {
                con.Close();
                MessageBox.Show("Email already exists.");

            }
            
            else if (reader2.HasRows)
            {
                con.Close();
                MessageBox.Show("Phone number should be unique.");

            }
            else
            {


                if (!isEmailValid)
                {
                    con.Close();
                    MessageBox.Show("Email is not valid");

                }
                else if (!isPhoneValid)
                {
                    con.Close();
                    MessageBox.Show("Phone number is not valid.");

                }
                else if (!isFNameValid || !isLNameValid)
                {
                    con.Close();
                    MessageBox.Show("Name is not valid.");

                }

                else if (!isValidSalary)
                {
                    con.Close();
                    MessageBox.Show("Not valid Salary.");
                }
                else
                {



                    SqlCommand cmd = con.CreateCommand();
                    cmd = new SqlCommand("INSERT INTO PERSON(FirstName,LastName,Contact,Email,DateOfBirth,Gender) VALUES(@FirstName,@LastName,@Contact,@Email,@DateofBirth,@Gender);SELECT SCOPE_IDENTITY();", con);
                    cmd.Parameters.AddWithValue("@FirstName", fname.Text);
                    cmd.Parameters.AddWithValue("@LastName", lname.Text);
                    cmd.Parameters.AddWithValue("@Contact", cont.Text);
                    cmd.Parameters.AddWithValue("@Email", email.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dobdate.Text));
                    string genn = gen.Text.ToString();
                    int g = Gender_look(genn);
                    cmd.Parameters.AddWithValue("@Gender", g);
                    //cmd.ExecuteNonQuery();
                    int modified = Convert.ToInt32(cmd.ExecuteScalar());

                    try
                    {

                        cmd = new SqlCommand("INSERT INTO Advisor(ID, Designation,Salary) VALUES('" + modified + "', @Designation,@Salary);", con);
                        string desig = desi.Text.ToString();
                        int gin = Designation_look(desig);
                        cmd.Parameters.AddWithValue("@Designation", gin);
                        cmd.Parameters.AddWithValue("@Salary", Salary.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data inserted.");
                    }

                    catch
                    {
                        con.Close();
                        MessageBox.Show("something went wrong.");
                    }
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
            con.Close();
            con.Open();
            string emailPattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$"; // Email address pattern
            string phonePattern = @"^03[0-9]{9}$";

            string namepat = @"^[A-Z]{1}[a-zA-Z\s\'-]*$";
            bool isEmailValid = Regex.IsMatch(email.Text, emailPattern);
            bool isPhoneValid = Regex.IsMatch(cont.Text, phonePattern);

            bool isFNameValid = Regex.IsMatch(fname.Text, namepat);
            bool isLNameValid = Regex.IsMatch(lname.Text, namepat);
            String Salarypat = @"^[0-9]*$";
            bool isValidSalary = Regex.IsMatch(Salary.Text, Salarypat);
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM PERSON WHERE Email = '" + email.Text + "'", con);
            SqlDataReader reader = cmd1.ExecuteReader();
            cmd1 = new SqlCommand("SELECT * FROM PERSON WHERE Contact = '" + cont.Text + "'", con);
            SqlDataReader reader2 = cmd1.ExecuteReader();
            if (reader.HasRows)
            {
                con.Close();
                MessageBox.Show("Email already exists.");

            }

            else if (reader2.HasRows)
            {
                con.Close();
                MessageBox.Show("Phone number should be unique.");

            }
            else
            {
                if (!isEmailValid)
                {
                    con.Close();
                    MessageBox.Show("Email is bnot valid");

                }
                else if (!isPhoneValid)
                {
                    con.Close();
                    MessageBox.Show("Phone number is not valid.");

                }
                else if (!isFNameValid || !isLNameValid)
                {
                    con.Close();
                    MessageBox.Show("Name is not valid.");

                }

                else if (!isValidSalary)
                {
                    con.Close();
                    MessageBox.Show("Not valid Salary.");
                }
                else
                {



                    int index = addDis.CurrentCell.RowIndex;
                    addDis.Rows[index].Selected = true;
                    string id = addDis.SelectedCells[0].Value.ToString();
                    string desig = desi.Text.ToString();
                    int g = Designation_look(desig);
                    SqlCommand cmd = new SqlCommand(" UPDATE PERSON SET FirstName = @FirstName,LastName = @LastName ,Contact = @Contact,Email = @Email,DateOfBirth = @DateOfBirth,Gender = @Gender WHERE Id ='" + id + "';", con);
                    cmd.Parameters.AddWithValue("@FirstName", fname.Text);
                    cmd.Parameters.AddWithValue("@LastName", lname.Text);
                    cmd.Parameters.AddWithValue("@Contact", cont.Text);
                    cmd.Parameters.AddWithValue("@Email", email.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dobdate.Text));
                    string genn = gen.Text.ToString();
                    int gin = Gender_look(genn);
                    cmd.Parameters.AddWithValue("@Gender", gin);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand(" Update Advisor SET Designation = '" + g + "', Salary= '" + Salary.Text + "'  WHERE Id ='" + id + "';", con);
                    cmd.ExecuteNonQuery();
                    addDis.Rows.RemoveAt(index);
                    addDis.DataSource = dt;
                    con.Close();
                    this.Hide();
                    Advisorr adv = new Advisorr();
                    adv.Show();
                    MessageBox.Show("Data updated.");
                }
            }


          

        }

        private void button5_Click(object sender, EventArgs e)
        {
            AdvisorDisplay.Hide();
            panel2.Hide();
            AddAdvisor.Show();
            panel1.Show();
           // panel3.Hide();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            panel2.Hide();
            AddAdvisor.Hide();
            panel1.Hide();
            AdvisorDisplay.Show();
            //panel3.Show();

        }
        private void Delete_rec()
        {
            con.Open();

            int index = addDis.CurrentCell.RowIndex;
            addDis.Rows[index].Selected = true;
            string id = addDis.SelectedCells[0].Value.ToString();
            SqlCommand cmdchk = new SqlCommand("SELECT AdvisorId From ProjectAdvisor WHERE AdvisorId='" + id + "'", con);
            SqlDataReader reader = cmdchk.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    con.Close();

                    MessageBox.Show("Can not Delete as data exists in ProjectAdvisor.");
                }
                else
                {
                    try
                    {
                        
                        SqlCommand cmd = new SqlCommand(" DELETE FROM Advisor WHERE Id = '" + id + "';", con);

                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand(" DELETE FROM Person WHERE Id = '" + id + "';", con);
                        cmd.ExecuteNonQuery();
                        addDis.Rows.RemoveAt(index);
                        addDis.DataSource = dt;
                        MessageBox.Show("Chal gya");

                    }
                    catch
                    {
                        MessageBox.Show("Something went wrong.");
                    }
                }


            }
            catch
            {
                MessageBox.Show("Something went wrong.");
            }
            con.Close();
        }
        

        private void addDis_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Advisor?", "Person", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Delete_rec();
                }
                
            }
            if (e.ColumnIndex == 7)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to update this Advisor", "person", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    AdvisorDisplay.Hide();
                    panel2.Hide();
                    AddAdvisor.Show();
                    panel1.Show();
                    //panel3.Hide();
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

        private void Salary_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Genderr_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void genbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Group grp = new Group();
            grp.Show();
        }

        private void fname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space)
            {


                e.Handled = false;

            }
            else
            {
                MessageBox.Show("Please Enter only Alphabets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Handled = true;

            }
        }

        private void lname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space)
            {


                e.Handled = false;

            }
            else
            {
                MessageBox.Show("Please Enter only Alphabets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Handled = true;

            }
        }

        private void cont_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
