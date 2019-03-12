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
    public partial class ProjectAdvisor : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=SAVIRAYOUSAF;Initial Catalog=ProjectA;MultipleActiveResultSets=true;Integrated Security=True");
        public ProjectAdvisor()
        {
            InitializeComponent();
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
            cmd.CommandText = "SELECT * FROM ProjectAdvisor;";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ProAdvGrid.DataSource = dt; //ProjectGrid is name of data grid view present on form
            con.Close();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Project Pro = new Project();
            panel2.Hide();
            panel1.Show();
            disp_data();
            Pro.Show();
        }

        private void ProjectAdvisor_Load(object sender, EventArgs e)
        {
            panel2.Hide();
            addpan.Hide();
            editpan.Hide();
            panel1.Show();
            disp_data();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Display studfrm = new Display();
            studfrm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Advisorr advisorfrm = new Advisorr();
            advisorfrm.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Project Pro = new Project();

            Pro.Show();
            

        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            addpan.Hide();
            editpan.Hide();
            panel1.Show();
            disp_data();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            editpan.Hide();
            panel2.Show();
            addpan.Show();
        }

        int value;
        private int Advisor_look(string gen)
        {
            string query;

            if (gen == "Main Advisor")
                query = "SELECT Id FROM Lookup where Category= 'ADVISOR_ROLE' AND Value = 'Main Advisor'";
            if(gen == "Co-Advisror")
                query = "SELECT Id FROM Lookup where Category= 'ADVISOR_ROLE' AND Value = 'Co-Advisror'";
            else
                query = "SELECT Id FROM Lookup where Category= 'ADVISOR_ROLE' AND Value = 'Industry Advisor'";
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
        private void button8_Click(object sender, EventArgs e)
        {
            //outer if check if Project Id and advisor Id exists or not.
            //inner if check if there is any kind of primary key violation inside table
            con.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT ID FROM Advisor WHERE ([ID] = @ID)", con);
            check_User_Name.Parameters.AddWithValue("ID", AdvId.Text);
            SqlDataReader reader1 = check_User_Name.ExecuteReader();
            check_User_Name = new SqlCommand("SELECT ID FROM Project WHERE ([ID] = @ID)", con);
            check_User_Name.Parameters.AddWithValue("ID", ProId.Text);
            SqlDataReader reader2 = check_User_Name.ExecuteReader();
            if (reader1.HasRows && reader2.HasRows)
            {

                con.Close();
                con.Open();
                SqlCommand check_User_Name3 = new SqlCommand("SELECT ProjectId,AdvisorId  FROM ProjectAdvisor WHERE ([AdvisorId] = @AdvisorId  and [ProjectId] = @ProjectId)", con);
                check_User_Name.Parameters.AddWithValue("AdvisorId", AdvId.Text);
                check_User_Name.Parameters.AddWithValue("ProjectId", ProId.Text);
                SqlDataReader reader3 = check_User_Name.ExecuteReader();
                if (reader3.HasRows)
                {
                    con.Close();
                    MessageBox.Show("There must exists one advisor of one type for 1 project.");
                }
                else
                {
                    con.Close();
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd = new SqlCommand("INSERT INTO ProjectAdvisor(AdvisorId, ProjectId,AdvisorRole,AssignmentDate) VALUES(@AdvisorId,@ProjectId,@AdvisorRole,@AssignmentDate)", con);
                    cmd.Parameters.AddWithValue("@ProjectId", ProId.Text);
                    cmd.Parameters.AddWithValue("@AdvisorId", AdvId.Text);
                    string desig = AdvR.Text.ToString();
                    int g = Advisor_look(desig);
                    cmd.Parameters.AddWithValue("@AdvisorRole", g);
                    cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Parse(Assgdate.Text));


                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data Inserted Successfully");
                }
            }
            
            
            else
            {
                con.Close();
                
                MessageBox.Show("Record does not exists.");
            }
            con.Close();
        }

        private void AsgDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void addpan_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            panel1.Hide();
            
            addpan.Hide();
            editpan.Show();
            panel2.Show();


        }

        private void button10_Click(object sender, EventArgs e)
        {
            Evaluation eval = new Evaluation();
            eval.Show();
        }
    }
}
