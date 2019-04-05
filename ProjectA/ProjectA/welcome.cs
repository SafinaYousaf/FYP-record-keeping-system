using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectA
{
    public partial class welcome : Form
    {
        public welcome()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Display fm1 = new Display();
            fm1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Advisorr AdvisorForm = new Advisorr();
            AdvisorForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Project pro = new Project();
            pro.Show();
        }

        private void prorep_Click(object sender, EventArgs e)
        {
            Report prorepp = new Report();
            prorepp.Show();
        }

        private void evalrep_Click(object sender, EventArgs e)
        {
            Report2 prorepp2 = new Report2();
            prorepp2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Projcount pro = new Projcount();
            pro.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AdvisorName pro = new AdvisorName();
            pro.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Projcount pro = new Projcount();
            pro.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Evaluation eval = new Evaluation();
            eval.Show();
        }
    }
}
