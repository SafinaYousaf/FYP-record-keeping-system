﻿using System;
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
    }
}
