using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

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

        private void saveSettings()
        {
            try
            {
                // Check if registry keys exist yet or not
                if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Semrau Software Consulting\ProductionReady", "DisableUpdates", null) == null)
                {
                    Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("Semrau Software Consulting");
                    Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting", true).CreateSubKey("ProductionReady");
                    RegistryKey root = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting").OpenSubKey("ProductionReady", true);
                    root.SetValue("DisableUpdates", checkBox1.Checked.ToString());
                    root.SetValue("ChangeWallpaper", checkBox2.Checked.ToString());
                    root.SetValue("DisableSleep", checkBox3.Checked.ToString());
                    root.SetValue("DisableTimeout", checkBox4.Checked.ToString());
                    root.SetValue("DisableScreensaver", checkBox5.Checked.ToString());
                    root.SetValue("PreventShutdown", checkBox6.Checked.ToString());
                }

            }catch (Exception ex)
            {
                MessageBox.Show("Error saving settings:" + Environment.NewLine + ex.ToString());
            }
        }
        private void saveDays()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveSettings();
        }
    }
}
