﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serial
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public static  int p_num = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (p_num == 0)
            {
                Form1 form1 = new Form1();
                form1.Show();
                p_num++;
            }
        }
    }
}