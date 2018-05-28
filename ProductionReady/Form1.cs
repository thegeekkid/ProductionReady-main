using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;

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
            groupBox1.Visible = checkBox1.Checked;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 3)
            {
                MessageBox.Show("Error: you can not select more than 3 production days.  Updates and reboots are an important part of keeping your computer safe and running smoothly.");
            }else
            {
                saveDays();
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
                }

                // Save settings
                RegistryKey root = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting").OpenSubKey("ProductionReady", true);
                root.SetValue("DisableUpdates", checkBox1.Checked.ToString());
                root.SetValue("ChangeWallpaper", checkBox2.Checked.ToString());
                root.SetValue("DisableSleep", checkBox3.Checked.ToString());
                root.SetValue("DisableTimeout", checkBox4.Checked.ToString());
                root.SetValue("DisableScreensaver", checkBox5.Checked.ToString());

                // Alert user
                MessageBox.Show("Settings saved!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving settings:" + Environment.NewLine + ex.ToString());
            }
        }
        private void saveDays()
        {
            try
            {
                // Check if registry keys exist yet or not
                if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Semrau Software Consulting\ProductionReady", "DisableUpdates", null) == null)
                {
                    Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("Semrau Software Consulting");
                    Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting", true).CreateSubKey("ProductionReady");
                }

                // Save days

                string dayarray = "";

                RegistryKey root = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting").OpenSubKey("ProductionReady", true);
                foreach (int day in checkedListBox1.CheckedIndices)
                {
                    if (dayarray != "")
                    {
                        dayarray += ",";
                    }
                    dayarray += day.ToString();

                }

                root.SetValue("ProductionDays", dayarray);

                MessageBox.Show("Production days saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving production days:" + Environment.NewLine + ex.ToString());
            }
            
        }

        private void readGUI()
        {
            // Check if registry keys exist yet or not
            if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Semrau Software Consulting\ProductionReady", "DisableUpdates", null) != null)
            {

                //MessageBox.Show("Entering!");
                RegistryKey root = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting").OpenSubKey("ProductionReady");

                checkBox1.Checked = bool.Parse(root.GetValue("DisableUpdates").ToString());
                checkBox2.Checked = bool.Parse(root.GetValue("ChangeWallpaper").ToString());
                checkBox3.Checked = bool.Parse(root.GetValue("DisableSleep").ToString());
                checkBox4.Checked = bool.Parse(root.GetValue("DisableTimeout").ToString());
                checkBox5.Checked = bool.Parse(root.GetValue("DisableScreensaver").ToString());

            }

            // Read production days
            if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Semrau Software Consulting\ProductionReady", "ProductionDays", null) != null)
            {
                RegistryKey root = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting").OpenSubKey("ProductionReady");

                string daysarray = root.GetValue("ProductionDays").ToString();
                
                foreach(string i in daysarray.Split(','))
                {
                    checkedListBox1.SetItemChecked(int.Parse(i), true);
                }
            }


            // Check if production mode is enabled yet
            if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Semrau Software Consulting\ProductionReady", "pmode", null) != null)
            {
                Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("Semrau Software Consulting");
                Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting", true).CreateSubKey("ProductionReady");

                RegistryKey root = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting").OpenSubKey("ProductionReady");

                if (root.GetValue("pmode").ToString() == "True")
                {
                    EnablePmodeGUI();
                }else
                {
                    DisablePmodeGUI();
                }
            }else
            {
                // Production mode is disabled if this isn't set.
                DisablePmodeGUI();
            }

            

        }

        private void EnablePmode() {
            // Check if registry keys exist yet or not
            if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Semrau Software Consulting\ProductionReady", "DisableUpdates", null) == null)
            {
                Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey("Semrau Software Consulting");
                Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting", true).CreateSubKey("ProductionReady");
            }

            RegistryKey root = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting").OpenSubKey("ProductionReady", true);

            root.SetValue("pmode", "True");


            if (checkBox2.Checked)
            {
                ChangeWallpaper();
            }

            if (checkBox5.Checked)
            {
                DisableScreensaver();
            }

            if (checkBox3.Checked)
            {
                DisableSleep();
            }

            if (checkBox4.Checked)
            {
                DisableTimeout();
            }

            RestartSvc();

            EnablePmodeGUI();
        }


        private void EnablePmodeGUI()
        {
            this.button3.Enabled = false;
            this.button4.Enabled = true;
        }

        private void DisablePmode()
        {
            RegistryKey root = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Semrau Software Consulting").OpenSubKey("ProductionReady", true);
            root.SetValue("pmode", "False");

            RestartSvc();

            DisablePmodeGUI();
        }

        private void DisablePmodeGUI()
        {
            this.button3.Enabled = true;
            this.button4.Enabled = false;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            
            readGUI();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EnablePmode();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DisablePmode();
        }

        
        


        private void ChangeWallpaper()
        {

            Color color = Color.Black;

            NativeMethods.SystemParametersInfo(
                    NativeMethods.SPI_SETDESKWALLPAPER, 0, "",
                    NativeMethods.SPIF_UPDATEINIFILE | NativeMethods.SPIF_SENDWININICHANGE
                );

            int[] elements = { NativeMethods.COLOR_DESKTOP };
            int[] colors = { System.Drawing.ColorTranslator.ToWin32(color) };
            NativeMethods.SetSysColors(elements.Length, elements, colors);

            // Save value in registry so that it will persist
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Colors", true);
            key.SetValue(@"Background", string.Format("{0} {1} {2}", color.R, color.G, color.B));
        }

        private static class NativeMethods
        {
            public const int COLOR_DESKTOP = 1;
            public const int SPI_SETDESKWALLPAPER = 20;
            public const int SPIF_UPDATEINIFILE = 0x01;
            public const int SPIF_SENDWININICHANGE = 0x02;

            [DllImport("user32.dll")]
            public static extern bool SetSysColors(int cElements, int[] lpaElements, int[] lpaRgbValues);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        }



        private void DisableScreensaver()
        {
            Registry.CurrentUser.OpenSubKey("Control Panel").OpenSubKey("Desktop", true).DeleteValue("SCRNSAVE.EXE");
        }
        private void DisableSleep()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\Windows\System32\powercfg.exe";
            proc.StartInfo.Arguments = @"-x -standby-timeout-ac 0";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();
            proc.StartInfo.Arguments = @"-x -standby-timeout-dc 0";
            proc.Start();
            proc.WaitForExit();
            proc.StartInfo.Arguments = @"-x -disk-timeout-ac 0";
            proc.Start();
            proc.WaitForExit();
            proc.StartInfo.Arguments = @"-x -disk-timeout-dc 0";
            proc.Start();
            proc.WaitForExit();
            proc.StartInfo.Arguments = @"-x -hibernate-timeout-ac 0";
            proc.Start();
            proc.WaitForExit();
            proc.StartInfo.Arguments = @"-x -hibernate-timeout-dc 0";
            proc.Start();
            proc.WaitForExit();
        }
        private void DisableTimeout()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\Windows\System32\powercfg.exe";
            proc.StartInfo.Arguments = @"-x -monitor-timeout-ac 0";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();
            proc.StartInfo.Arguments = @"-x -monitor-timeout-dc 0";
            proc.Start();
            proc.WaitForExit();
        }




        private void RestartSvc()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\Windows\System32\net.exe";
            proc.StartInfo.Arguments = "stop ProductionReadyService";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();
            proc.StartInfo.Arguments = "start ProductionReadyService";
            proc.Start();
            proc.WaitForExit();
        }
    }
    

}

