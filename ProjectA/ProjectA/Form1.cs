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
    public partial class Display : Form
    {
        
        SqlDataAdapter da;
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;MultipleActiveResultSets=true;Integrated Security=True");
        public Display()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        System.Data.DataSet ds;
        public void disp_data()
        {
            con.Open();
            
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP(10) P.Id,S.RegistrationNo,P.FirstName+' '+P.LastName AS Name,P.Contact,P.Email,P.DateOfBirth,P.Gender FROM Person AS P JOIN STUDENT AS S ON S.Id = P.Id ORDER BY P.Id DESC";
            cmd.ExecuteNonQuery();
            //DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            StudentGrid.DataSource = dt; //Student is name of data grid view present on form
            con.Close();
            //delete buttons
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            StudentGrid.Columns.Add(btn);
            btn.HeaderText = "Delete";
            btn.Text = "Delete";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            //update
            DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
            StudentGrid.Columns.Add(btn1);
            btn1.HeaderText = "Update";
            btn1.Text = "Update";
            btn1.Name = "btn1";
            btn1.UseColumnTextForButtonValue = true;

        }

        private void StudentGrid_CellContentClick1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Person?", "Person", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Delete();
                }
                else if (result == DialogResult.No)
                {
                    this.Hide();
                    Display obj = new Display();
                    obj.Show();
                }
            }
            if (e.ColumnIndex == 8)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to update this Person", "person", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    panel1.Hide();
                    //panel2.Hide();
                    //panel6.Hide();
                    //panel4.Hide();
                    //panel3.Hide();
                    AddStudent.Hide();
                    panel5.Hide();
                    updatepan.Show();
                    //Update_rec();
                }
                else if (result == DialogResult.No)
                {
                    this.Hide();
                    Display obj = new Display();
                    obj.Show();
                }


            }

        }
        private void Update_rec()
        {
            con.Close();
            con.Open();

            int index = StudentGrid.CurrentCell.RowIndex;
            StudentGrid.Rows[index].Selected = true;
            string id = StudentGrid.SelectedCells[0].Value.ToString();
            try
            {
                //SqlCommand con = new SqlCommand("SELECT Id FROM STUDENT WHERE id = ", con);
                //SqlDataReader reader = con.ExecuteReader();
                SqlCommand cmd = new SqlCommand(" UPDATE STUDENT WHERE Id = '" + id + "';", con);
                cmd.ExecuteNonQuery();
                StudentGrid.Rows.RemoveAt(index);
                StudentGrid.DataSource = dt;
                MessageBox.Show("Done");
                this.Hide();
                Display obj = new Display();
                obj.Show();
                
            }

            catch
            {
                MessageBox.Show("something went wrong.");
            }
            con.Close();
        }

        private void update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Fname.Text) || string.IsNullOrWhiteSpace(email.Text))
            {
                MessageBox.Show("Firstname and email is compulsory.");
            }

            else
            {
                con.Open();
                int index = StudentGrid.CurrentCell.RowIndex;
                StudentGrid.Rows[index].Selected = true;
                string id = StudentGrid.SelectedCells[0].Value.ToString();
                try
                {
                    //SqlCommand cmd = new SqlCommand(" DELETE FROM STUDENT WHERE Id = '" + id + "';", con);
                    //cmd.ExecuteNonQuery();
                    //StudentGrid.Rows.RemoveAt(index);
                    //StudentGrid.DataSource = dt;
                    MessageBox.Show("Chal gya");
                    SqlCommand cmd = new SqlCommand("UPDATE PERSON(FirstName,LastName,Contact,Email,DateOfBirth,Gender) VALUES(@FirstName,@LastName,@Contact,@Email,@DateofBirth,@Gender)", con);
                    cmd.Parameters.AddWithValue("@FirstName", fnu.Text);
                    cmd.Parameters.AddWithValue("@LastName", lnu.Text);
                    cmd.Parameters.AddWithValue("@Contact", conu.Text);
                    cmd.Parameters.AddWithValue("@Email", mailu.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dobu.Text));
                    string genn = Genderr.Text.ToString();
                    int g = Gender_look(genn);
                    cmd.Parameters.AddWithValue("@Gender", g);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Open();
                    cmd = new SqlCommand("SELECT Id FROM PERSON WHERE (Id = '"+id+"')", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    //string id1;
                    while (reader.Read())
                    {
                        id = reader[0].ToString();
                        try
                        {
                            con.Open();
                            cmd = new SqlCommand("Update STUDENT('" + id + "','" + rno.Text + "')", con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data Inserted Successfully");
                            con.Close();
                        }

                        catch
                        {
                            MessageBox.Show("Someting went wrong.");
                        }
                    }
                    

                }
                /*
                cmd.ExecuteNonQuery();
                var val = cmd.ExecuteScalar().ToString();
                int val1 = int.Parse(val);
                con.Close();
                con.Open();
                cmd = new SqlCommand("INSERT INTO STUDENT()", con);
                */
                catch
                {
                    MessageBox.Show("Someting went wrong.");
                }
                MessageBox.Show("Data Inserted Successfully");
            }

            //handling panels

            //panel4.Hide();
            //panel3.Hide();
            AddStudent.Hide();
            panel5.Hide();
            panel1.Show();
            //panel2.Show();
           // panel3.Show();
        }
        private void Delete()
        {
            con.Close();
            con.Open();

            int index = StudentGrid.CurrentCell.RowIndex;
            StudentGrid.Rows[index].Selected = true;
            string id = StudentGrid.SelectedCells[0].Value.ToString();
            try
            {
                
                SqlCommand cmd = new SqlCommand(" DELETE FROM STUDENT WHERE Id = '" + id + "';", con);

                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(" DELETE FROM Person WHERE Id = '" + id + "';", con);

                cmd.ExecuteNonQuery();
                StudentGrid.Rows.RemoveAt(index);
                StudentGrid.DataSource = dt;
                MessageBox.Show("Student Deleted.");
                
            }

            catch
            {
                MessageBox.Show("Something went wrong.");
            }
            con.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            disp_data();
            panel1.Show();
            
            AddStudent.Hide();
            updatepan.Hide();
            panel5.Hide();

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            updatepan.Hide();
            //panel2.Hide();
            //panel4.Hide();
            AddStudent.Hide();
            panel5.Hide();
            panel1.Show();
            //panel3.Show();
            //panel2.Show();
            //disp_data();
        }

        private void StudentGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentGrid_CellContentClick1(sender, e);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            //addstu.Show();
            updatepan.Hide();
            panel1.Hide();
            //panel2.Hide();
            //panel6.Hide();
            //panel4.Hide();
            //panel3.Hide();
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
            
            panel5.Hide();
            //panel6.Hide();
            AddStudent.Hide();
            panel1.Show();
            //panel2.Show();
            //panel3.Show();
            //panel4.Show();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*
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
            con.Close();
            con.Open();
            string emailPattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$"; // Email address pattern
            string phonePattern = @"^03[0-9]{9}$";
            string RegNopat = @"^[0-9]{4}-{1}[A-Z]{2}-{1}[0-9]{3}$";
            string namepat = @"^[A-Z]{1}[a-zA-Z\s\'-]*$";
            bool isEmailValid = Regex.IsMatch(email.Text, emailPattern);
            bool isPhoneValid = Regex.IsMatch(Cont.Text, phonePattern);
            bool isRegNoValid = Regex.IsMatch(rno.Text, RegNopat);
            bool isFNameValid = Regex.IsMatch(Fname.Text, namepat);
            bool isLNameValid = Regex.IsMatch(Lname.Text, namepat);
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM PERSON WHERE Email = '"+email.Text+"'",con);
            SqlDataReader reader = cmd1.ExecuteReader();
            if(reader.HasRows)
            {
                con.Close();
                MessageBox.Show("Email alredy exists.");
                //panel4.Hide();
                //panel3.Hide();
                AddStudent.Hide();
                panel5.Show();
                panel1.Hide();
                //panel2.Hide();
            }
            
            else if (string.IsNullOrWhiteSpace(Fname.Text) || string.IsNullOrWhiteSpace(email.Text))
            {
                con.Close();
                MessageBox.Show("Firstname and email is compulsory.");
               // panel4.Hide();
                //panel3.Hide();
                AddStudent.Hide();
                panel5.Show();
                panel1.Hide();
                //panel2.Hide();
            }

            else
            {
                // Now you can check the result   
                if (!isEmailValid)
                {
                    con.Close();
                    MessageBox.Show("Please enter a valid email");
                    //panel4.Hide();
                    //panel3.Hide();
                    AddStudent.Hide();
                    panel5.Show();
                    panel1.Hide();
                    //panel2.Hide();
                    //panel3.Hide();
                }
                else if (!isRegNoValid)
                {
                    con.Close();
                    MessageBox.Show("please enter valid RegNo number");
                    //panel4.Hide();
                    //panel3.Hide();
                    AddStudent.Hide();
                    panel5.Show();
                    panel1.Hide();
                    //panel2.Hide();
                    //panel3.Hide();

                }
                else if(!isFNameValid || !isLNameValid)
                {
                    con.Close();
                    MessageBox.Show("please enter valid Name.");
                   // panel4.Hide();
                    //panel3.Hide();
                    AddStudent.Hide();
                    panel5.Show();
                    panel1.Hide();
                    //panel2.Hide();
                }
                else
                {
                    try
                    {
                        con.Close();
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd = new SqlCommand("INSERT INTO PERSON(FirstName,LastName,Contact,Email,DateOfBirth,Gender) VALUES(@FirstName,@LastName,@Contact,@Email,@DateofBirth,@Gender);SELECT SCOPE_IDENTITY();", con);
                        cmd.Parameters.AddWithValue("@FirstName", Fname.Text);
                        cmd.Parameters.AddWithValue("@LastName", Lname.Text);
                        cmd.Parameters.AddWithValue("@Contact", Cont.Text);
                        cmd.Parameters.AddWithValue("@Email", email.Text);
                        cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dob.Text));
                        string genn = Genderr.Text.ToString();
                        int g = Gender_look(genn);
                        cmd.Parameters.AddWithValue("@Gender", g);
                        //cmd.ExecuteNonQuery();
                        int modified = Convert.ToInt32(cmd.ExecuteScalar());
                        try
                        {

                            cmd = new SqlCommand("INSERT INTO STUDENT(Id, RegistrationNo) values('" + modified + "','" + rno.Text + "');", con);
                            cmd.ExecuteNonQuery();
                        }

                        catch {
                            con.Close();
                            MessageBox.Show("something went wrong.");
                        }
                        MessageBox.Show("Data Inserted Successfully");

                    }
                    catch
                    {
                        con.Close();
                        MessageBox.Show("Something Went wrong.");
                    }
                    
                    con.Close();
                    MessageBox.Show("Data Inserted Successfully2");
                    this.Hide();
                    Display obj1 = new Display();
                    obj1.Show();
                }
               

                /*//handling panels

                panel4.Hide();
                //panel3.Hide();
                AddStudent.Hide();
                panel5.Hide();
                panel1.Show();
                panel2.Show();
                //panel3.Show();*/
            }

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
            
            
            //panel4.Hide();
            
            AddStudent.Hide();
            panel5.Hide();
            panel1.Show();
            //panel3.Show();
            //panel2.Show();
            //panel6.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        { 
            /*con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Student WHERE ([ID] = @ID)", con);
            
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
            */
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Advisorr AdvisorForm = new Advisorr();
            AdvisorForm.Show();
        }

        private void update_Click_1(object sender, EventArgs e)
        {
            con.Close();
            
            string emailPattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$"; // Email address pattern
            string phonePattern = @"^03[0-9]{9}$";
            string RegNopat = @"^[0-9]{4}-{1}[A-Z]{2}-{1}[0-9]{3}$";
            string namepat = @"^[A-Z]{1}[a-zA-Z\s\'-]*$";
            bool isEmailValid = Regex.IsMatch(mailu.Text, emailPattern);
            bool isPhoneValid = Regex.IsMatch(conu.Text, phonePattern);
            bool isRegNoValid = Regex.IsMatch(RegTbu.Text, RegNopat);
            bool isFNameValid = Regex.IsMatch(fnu.Text, namepat);
            bool isLNameValid = Regex.IsMatch(lnu.Text, namepat);
            if (string.IsNullOrWhiteSpace(fnu.Text) || string.IsNullOrWhiteSpace(mailu.Text))
            {
                MessageBox.Show("Firstname and email is compulsory.");
                panel1.Hide();
                //panel2.Hide();
                //panel6.Hide();
                //panel4.Hide();
                //panel3.Hide();
                AddStudent.Hide();
                panel5.Hide();
                updatepan.Show();
                //Update_rec();
            }
            else if (!isEmailValid)
            {
                MessageBox.Show("Please enter a valid email");
                panel1.Hide();
                //panel2.Hide();
                //panel6.Hide();
                //panel4.Hide();
                //panel3.Hide();
                AddStudent.Hide();
                panel5.Hide();
                updatepan.Show();
                //Update_rec();

            }
            else if (!isRegNoValid)
            {
                MessageBox.Show("please enter valid RegNo number");
                panel1.Hide();
                //panel2.Hide();
                //panel6.Hide();
               // panel4.Hide();
                //panel3.Hide();
                AddStudent.Hide();
                panel5.Hide();
                updatepan.Show();
                //Update_rec();
            }
            else if (!isFNameValid || !isLNameValid)
            {
                con.Close();
                MessageBox.Show("please enter valid Name.");
                panel1.Hide();
                //panel2.Hide();
                //panel6.Hide();
                //panel4.Hide();
                //panel3.Hide();
                AddStudent.Hide();
                panel5.Hide();
                updatepan.Show();
                //Update_rec();
            }
            else
            {

                try
                {


                    con.Open();
                    int index = StudentGrid.CurrentCell.RowIndex;
                    StudentGrid.Rows[index].Selected = true;
                    string id = StudentGrid.SelectedCells[0].Value.ToString();

                    //UPDATE Advisor SET Designation = '" + g + "', Salary = '" + Salary.Text + "' WHERE ID = '" + ID.Text + "';
                    SqlCommand cmd = new SqlCommand("UPDATE PERSON SET FirstName = @FirstName,LastName = @LastName ,Contact = @Contact,Email = @Email,DateOfBirth = @DateOfBirth,Gender = @Gender WHERE Id ='" + id + "';", con);
                    cmd.Parameters.AddWithValue("@FirstName", fnu.Text);
                    cmd.Parameters.AddWithValue("@LastName", lnu.Text);
                    cmd.Parameters.AddWithValue("@Contact", conu.Text);
                    cmd.Parameters.AddWithValue("@Email", mailu.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dobu.Text));
                    string genn = Genderr.Text.ToString();
                    int g = Gender_look(genn);
                    cmd.Parameters.AddWithValue("@Gender", g);
                    cmd.ExecuteNonQuery();
                    try
                    {


                        SqlCommand cmd2 = new SqlCommand("UPDATE STUDENT SET RegistrationNo =@regno WHERE Id ='" + id + "';", con);
                        cmd2.Parameters.AddWithValue("@regno", RegTbu.Text);
                        // cmd.Parameters.AddWithValue("@id", id);
                        cmd2.ExecuteNonQuery();
                        con.Close();
                        this.Hide();
                        Display obj = new Display();
                        obj.Show();
                    }
                    catch
                    {
                        con.Close();
                    }
                }
                catch
                {
                    con.Close();
                    MessageBox.Show("Something went wrong.");
                }
            }
            con.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("yes");
            con.Open();
            int index = StudentGrid.CurrentCell.RowIndex;
            StudentGrid.Rows[index].Selected = true;
            string id = StudentGrid.SelectedCells[0].Value.ToString();
            //UPDATE Advisor SET Designation = '" + g + "', Salary = '" + Salary.Text + "' WHERE ID = '" + ID.Text + "';
            SqlCommand cmd = new SqlCommand("UPDATE PERSON SET FirstName = @FirstName,LastName = @LastName ,Contact = @Contact,Email = @Email,DateOfBirth = @DateOfBirth,Gender = @Gender ", con);
            cmd.Parameters.AddWithValue("@FirstName", fnu.Text);
            cmd.Parameters.AddWithValue("@LastName", lnu.Text);
            cmd.Parameters.AddWithValue("@Contact", conu.Text);
            cmd.Parameters.AddWithValue("@Email", mailu.Text);
            cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dobu.Text));
            string genn = Genderr.Text.ToString();
            int g = Gender_look(genn);
            cmd.Parameters.AddWithValue("@Gender", g);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void updatepan_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            Project profm = new Project();
            profm.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ProjectAdvisor proadvfm = new ProjectAdvisor();
            proadvfm.Show();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            Evaluation evalfm = new Evaluation();
            evalfm.Show();
        }

        private void conu_KeyPress(object sender, KeyPressEventArgs e)
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

        private void fnu_KeyPress(object sender, KeyPressEventArgs e)
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

        private void lnu_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
