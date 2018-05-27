using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionReady
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                groupBox1.Visible = true;
            }else
            {
                if (!(checkBox6.Checked))
                {
                    groupBox1.Visible = false;
                }
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                groupBox1.Visible = true;
            }else
            {
                if (!(checkBox1.Checked))
                {
                    groupBox1.Visible = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 3)
            {
                MessageBox.Show("Error: you can not select more than 3 production days.  Updates and reboots are an important part of keeping your computer safe and running smoothly.");
            }
        }
    }
}
