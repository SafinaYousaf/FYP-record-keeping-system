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
        
        
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;MultipleActiveResultSets=true;Integrated Security=True");
        public Display()
        {
            InitializeComponent();
        }
        //display data for student and person table
        DataTable dt = new DataTable();
        SqlDataAdapter da;
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
        //Display data for GroupStydent
        DataTable dt1 = new DataTable();
        SqlDataAdapter da1;
        public void disp_data_grpstu()
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP(10) * FROM [GROUPSTUDENT] ORDER BY GroupId DESC";
            cmd.ExecuteNonQuery();
           
            da1 = new SqlDataAdapter(cmd);
            da1.Fill(dt1);
            GrpStudisp.DataSource = dt1; //Student is name of data grid view present on form
            con.Close();
            //delete buttons
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            GrpStudisp.Columns.Add(btn);
            btn.HeaderText = "Delete";
            btn.Text = "Delete";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            //update
            DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
            GrpStudisp.Columns.Add(btn1);
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
                    
                    panel5.Hide();
                    updatepan.Show();
                    
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
                           // MessageBox.Show("Data Updated Successfully");
                            con.Close();
                        }

                        catch
                        {
                            MessageBox.Show("Someting went wrong.");
                        }
                    }
                    

                }
                
                catch
                {
                    MessageBox.Show("Someting went wrong.");
                }
                MessageBox.Show("Data Updated Successfully");
            }

            //handling panels

            
            panel5.Hide();
            panel1.Show();
            
        }





        private void Delete()
        {
            con.Close();
            con.Open();
            int index = StudentGrid.CurrentCell.RowIndex;
            StudentGrid.Rows[index].Selected = true;
            string id = StudentGrid.SelectedCells[0].Value.ToString();
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM GroupStudent WHERE StudentId = '" +id+"'",con);
            SqlDataReader reader = cmd1.ExecuteReader();
            if (reader.HasRows)
            {
                con.Close();
                MessageBox.Show("Student Is Present in group.");
            }
            else
            {
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
                    con.Close();
                    MessageBox.Show("Something went wrong.");
                }
            }
            con.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            disp_data(); //display data of Student
            upgrpstu.Hide();
            
            disp_data_grpstu();//display data of student group
            //panel2.Hide();
            
            GroupStudentPan.Hide();
            
            
            updatepan.Hide();
            panel5.Hide();
            stugrpdispan.Hide();
            panel1.Show();





        }

        private void button1_Click(object sender, EventArgs e)
        {
            updatepan.Hide();
            stugrpdispan.Hide();
            upgrpstu.Hide();
            
            panel5.Hide();
            panel1.Show();
            GroupStudentPan.Hide();
           
        }

        private void StudentGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentGrid_CellContentClick1(sender, e);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            stugrpdispan.Hide();
            upgrpstu.Hide();
            updatepan.Hide();
            panel1.Hide();
            
            panel5.Show();



            
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
           

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            panel5.Hide();
            
            panel1.Show();
            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            
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
            cmd1 = new SqlCommand("SELECT * FROM PERSON WHERE Email = '" + Cont.Text + "'", con);
            SqlDataReader reader2 = cmd1.ExecuteReader();
            SqlCommand cmd3 = new SqlCommand("SELECT * FROM PERSON WHERE Email = '" + rno.Text + "'", con);
            SqlDataReader reader3 = cmd1.ExecuteReader();
            if (reader2.HasRows)
            {
                con.Close();
                MessageBox.Show("Phone number should be unique.");

            }
            if (reader3.HasRows)
            {
                con.Close();
                MessageBox.Show("Registration no Alredy exists.");

            }
            if (reader.HasRows)
            {
                con.Close();
                MessageBox.Show("Email alredy exists.");
                //panel4.Hide();
                //panel3.Hide();
                //AddStudent.Hide();
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
                //AddStudent.Hide();
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
                    //AddStudent.Hide();
                    panel5.Show();
                    panel1.Hide();
                    //panel2.Hide();
                    //panel3.Hide();
                }
                else if (!isRegNoValid)
                {
                    con.Close();
                    MessageBox.Show("Registration Number must be entered like '####-XX-###'.");
                    //panel4.Hide();
                    //panel3.Hide();
                    //AddStudent.Hide();
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
                    //AddStudent.Hide();
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
                        

                    }
                    catch
                    {
                        con.Close();
                        MessageBox.Show("Something Went wrong.");
                    }
                    
                    con.Close();
                    MessageBox.Show("Data Inserted Successfully.");
                    this.Hide();
                    Display obj1 = new Display();
                    obj1.Show();
                }
               

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
            //e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || char.IsWhiteSpace(e.KeyChar));

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
            
            //AddStudent.Hide();
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
                //AddStudent.Hide();
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
                //AddStudent.Hide();
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
                //AddStudent.Hide();
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
                //AddStudent.Hide();
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

        private void grpstu_Click(object sender, EventArgs e)
        {
            updatepan.Hide();
            upgrpstu.Hide();
            //AddStudent.Hide();
            panel5.Hide();
            panel1.Hide();
            GroupStudentPan.Hide();
            stugrpdispan.Show();

        }

        int val;
        private int Status_look(string gen)
        {
            string query;

            if (gen == "Active")
                query = "SELECT Id FROM Lookup where Category= 'STATUS' AND Value = 'Active'";
            else
                query = "SELECT Id FROM Lookup where Category= 'STATUS' AND Value = 'InActive'";
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
        private void addstugrp_Click(object sender, EventArgs e)
        {
           con.Close();
           
                con.Open();
            string RegNopat = @"^[0-9]{4}-{1}[A-Z]{2}-{1}[0-9]{3}$";
            bool isRegNoValid = Regex.IsMatch(stdid.Text, RegNopat);
            if (!isRegNoValid)
            {
                MessageBox.Show("REgistration Number must be of form '####-XX-###'.");
            }
            else
            {


                SqlCommand cmd = con.CreateCommand();
                //int countstd = 0;
                int id = 0;
                cmd = new SqlCommand("SELECT Id FROM Student WHERE RegistrationNO = @regno;", con);
                cmd.Parameters.AddWithValue("@regno", stdid.Text);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                SqlCommand check_User_Name0 = new SqlCommand("SELECT GroupId FROM [GroupStudent]  WHERE ([GroupId] = @GID AND [StudentId]='" + id + "')", con);
                check_User_Name0.Parameters.AddWithValue("GID", grpid.Text);
                check_User_Name0.Parameters.AddWithValue("SID", stdid.Text);
                SqlDataReader reader0 = check_User_Name0.ExecuteReader();
                if (reader0.HasRows)
                {
                    MessageBox.Show("Already Exists.");
                }
                else
                {
                    try
                    {
                        SqlCommand check_User_Name = new SqlCommand("SELECT Id FROM [Group] WHERE ([Id] = @ID)", con);
                        check_User_Name.Parameters.AddWithValue("ID", grpid.Text);
                        SqlDataReader reader = check_User_Name.ExecuteReader();
                        if (reader.HasRows)
                        {
                            SqlCommand check_User_Name1 = new SqlCommand("SELECT Id FROM [Student] WHERE ([Id] = '" + id + "')", con);
                            //check_User_Name1.Parameters.AddWithValue("ID", stdid.Text);
                            SqlDataReader reader2 = check_User_Name1.ExecuteReader();
                            if (reader2.HasRows)
                            {
                                SqlCommand check_User_Name3 = new SqlCommand("SELECT GroupId FROM [GroupStudent]  WHERE ([GroupId] = @GID AND [StudentId]='" + id + "')", con);
                                check_User_Name3.Parameters.AddWithValue("GID", grpid.Text);
                                //check_User_Name3.Parameters.AddWithValue("SID", stdid.Text);
                                SqlDataReader reader3 = check_User_Name3.ExecuteReader();
                                if (reader3.HasRows)
                                {
                                    MessageBox.Show("One Student must Present in One group.");
                                }
                                else
                                {
                                    cmd = new SqlCommand("Select COUNT(1) As gid from GroupStudent WHERE GroupId = '" + grpid.Text+"';", con);
                                    object countstd1 = cmd.ExecuteScalar();
                                    int countstd = 0;
                                    if (! (countstd1 == DBNull.Value))
                                    {
                                        countstd = Convert.ToInt32(countstd1);
                                    }
                                     
                                    if (countstd == 4)
                                    {
                                        con.Close();
                                        MessageBox.Show("No More Student Can Be Added.");
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("INSERT INTO [GroupStudent](GroupId,StudentId,[Status],AssignmentDate) VALUES(@GI,@SI,@Status,@datee);", con);
                                        cmd.Parameters.AddWithValue("@GI", grpid.Text);
                                        cmd.Parameters.AddWithValue("@SI", id);
                                        //cmd.Parameters.AddWithValue("@Status", status.Text);
                                        string st = statuscom.Text.ToString();
                                        int g = Status_look(st);
                                        cmd.Parameters.AddWithValue("@Status", g);
                                        cmd.Parameters.AddWithValue("@datee", DateTime.Parse(datedpick.Text));
                                        cmd.ExecuteNonQuery();
                                        MessageBox.Show("Data inserted.");

                                        con.Close();
                                        this.Hide();
                                        Display obj = new Display();
                                        obj.Show();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Student does not exists.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Group does not exists.");
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

        private void status_KeyPress(object sender, KeyPressEventArgs e)
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

            /* if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8)
             {


                 e.Handled = false;

             }
             else
             {
                 MessageBox.Show("Please Enter only Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                 e.Handled = true;

             }*/
        }

        private void stdid_KeyPress(object sender, KeyPressEventArgs e)
        {
          
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

        private void GrpStudisp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this Group Student?", "Group Student", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        con.Close();
                        con.Open();
                        int index = GrpStudisp.CurrentCell.RowIndex;
                        GrpStudisp.Rows[index].Selected = true;
                        string id = GrpStudisp.SelectedCells[1].Value.ToString();
                        SqlCommand cmd = new SqlCommand("DELETE FROM GroupStudent WHERE StudentId = '" + id + "';", con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        GrpStudisp.Rows.RemoveAt(index);
                        GrpStudisp.DataSource = dt1;
                        MessageBox.Show("Deleted!");
                    }
                    catch
                    {
                        con.Close();
                        MessageBox.Show("Something went wrong!");
                    }
                }
                else if (result == DialogResult.No)
                {
                    updatepan.Hide();
                    upgrpstu.Hide();
                    
                    panel5.Hide();
                    panel1.Hide();
                    GroupStudentPan.Hide();
                    stugrpdispan.Show();
                }
            }
            if (e.ColumnIndex == 5)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to Update this Group Student?", "GroupStudent", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {


                    
                    panel5.Hide();
                    panel1.Hide();
                    GroupStudentPan.Hide();
                    stugrpdispan.Hide();
                    upgrpstu.Show();
                }
                else
                {
                    this.Hide();
                    Display obj = new Display();
                    obj.Show();

                }

            }
        }

    

        private void grpstuadd_Click(object sender, EventArgs e)
        {
            updatepan.Hide();
            stugrpdispan.Hide();
           // AddStudent.Hide();
            panel5.Hide();
            panel1.Hide();
            GroupStudentPan.Show();
            
        }

        private void grpstuadd_Click_1(object sender, EventArgs e)
        {
           
            updatepan.Hide();
            stugrpdispan.Hide();
           
            panel5.Hide();
            panel1.Hide();
            
            GroupStudentPan.Show();
        }

        private void upgrpstu_Click(object sender, EventArgs e)
        {

        }

        private void stugrpdispan_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GrpStudisp_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            GrpStudisp_CellContentClick(sender, e);
        }

        private void upgrdbt_Click(object sender, EventArgs e)
        {
            con.Close();
            con.Open();
            


                try
                {
                    SqlCommand cmd = con.CreateCommand();

                    cmd = new SqlCommand("Select COUNT(1) As gid from GroupStudent WHERE GroupId = '" + GItxt.Text + "';", con);
                    object countstd1 = cmd.ExecuteScalar();
                    int countstd = 0;
                    if (!(countstd1 == DBNull.Value))
                    {
                        countstd = Convert.ToInt32(countstd1);
                    }

                    if (countstd >= 4)
                    {
                        con.Close();
                        MessageBox.Show("No More Student Can Be Added.");
                    }
                    else
                    {
                        int index = GrpStudisp.CurrentCell.RowIndex;
                        GrpStudisp.Rows[index].Selected = true;
                        string id = GrpStudisp.SelectedCells[1].Value.ToString();
                        cmd = new SqlCommand("Select COUNT(1) As gid from GroupStudent WHERE GroupId = '" + grpid.Text + "';", con);
                        cmd = new SqlCommand("UPDATE [GroupStudent] SET GroupId=@GI,StudentId =@SI,Status=@Status,AssignmentDate=@datee WHERE StudentId = '" + id + "';", con);
                        cmd.Parameters.AddWithValue("@GI", GItxt.Text);
                        cmd.Parameters.AddWithValue("@SI", id);
                        cmd.Parameters.AddWithValue("@Status", Stxt.Text);
                        cmd.Parameters.AddWithValue("@datee", DateTime.Parse(datetx.Text));
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Data updated.");

                        this.Hide();
                        Display obj = new Display();
                        obj.Show();

                    }
                }
                catch
                {
                    con.Close();
                    MessageBox.Show("Something went wrong.");
                }
            

        }

        private void GItxtad_KeyPress(object sender, KeyPressEventArgs e)
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

        private void addGE_Click(object sender, EventArgs e)
        {

        }

        private void SItxtad_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Group grp = new Group();
            grp.Show();

        }
       

        private void Display_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
