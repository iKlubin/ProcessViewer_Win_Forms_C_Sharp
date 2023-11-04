using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private Process selectedProcess;
        Timer timer;
        public Form1()
        {
            InitializeComponent();
            UpdateProcessList();
            timer = new Timer();
            timer.Interval = int.Parse(textBox1.Text) * 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.Columns.Add("Process Name", 100);
            listView1.Columns.Add("Process ID", 70);
            listView1.Columns.Add("Start Time", 150);
            listView1.Columns.Add("Total Processor Time", 150);
            listView1.Columns.Add("Number of Threads", 120);
            listView1.Columns.Add("Number of Instances", 150);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Interval = int.Parse(textBox1.Text) * 1000;
            UpdateProcessList();
        }

        private void UpdateProcessList()
        {
            Process[] processes = Process.GetProcesses();
            List<ListViewItem> listPr = new List<ListViewItem>();
            foreach (Process process in processes)
            {
                ListViewItem item = new ListViewItem(process.ProcessName);
                item.SubItems.Add(process.Id.ToString());
                try
                {
                    item.SubItems.Add(process.StartTime.ToString());
                    item.SubItems.Add(process.TotalProcessorTime.ToString());
                    item.SubItems.Add(process.Threads.Count.ToString());
                    item.SubItems.Add(Process.GetProcessesByName(process.ProcessName).Length.ToString());
                }
                catch (Win32Exception ex)
                {
                    item.SubItems.Add("N/A");
                    item.SubItems.Add("N/A");
                    item.SubItems.Add("N/A");
                    item.SubItems.Add("N/A");
                }
                listPr.Add(item);
            }
            listView1.Items.Clear();
            foreach (ListViewItem item in listPr)
            {
                listView1.Items.Add(item);
            }
        }

        private void buttonSetInterval_Click(object sender, EventArgs e)
        {
            int interval;
            if (int.TryParse(textBox1.Text, out interval))
            {
                timer.Interval = interval;
            }
        }

        private void listViewProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                string processName = item.SubItems[0].Text;
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    selectedProcess = processes[0];
                    textBox2.Text = $"Process ID: {selectedProcess.Id}\r\n" +
                        $"Start Time: {selectedProcess.StartTime}\r\n" +
                        $"Total Processor Time: {selectedProcess.TotalProcessorTime}\r\n" +
                        $"Number of Threads: {selectedProcess.Threads.Count}\r\n" +
                        $"Number of Instances: {processes.Length}\r\n";
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                string processName = item.SubItems[0].Text;
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    selectedProcess = processes[0];
                    string[] data = new string[5];
                    try
                    {
                        data[0] = selectedProcess.Id.ToString();
                        data[1] = selectedProcess.StartTime.ToString();
                        data[2] = selectedProcess.TotalProcessorTime.ToString();
                        data[3] = selectedProcess.Threads.Count.ToString();
                        data[4] = Process.GetProcessesByName(selectedProcess.ProcessName).Length.ToString();
                    }
                    catch (Win32Exception ex)
                    {
                        data[0] = selectedProcess.Id.ToString();
                        data[1] = "N/A";
                        data[2] = "N/A";
                        data[3] = "N/A";
                        data[4] = "N/A";
                    }
                    textBox2.Text =
                        "ID: " + data[0] +
                        "\r\nStartTime: " + data[1] +
                        "\r\nTotalProccessorTime: " + data[2] +
                        "\r\nNumThreads: " + data[3] +
                        "\r\nNumInstance: " + data[4];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProcess != null)
                {
                    selectedProcess.Kill();
                    MessageBox.Show("Процесс успешно завершен.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Выбранный процесс отсутствует.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось завершить процесс: " + ex.Message);
            }
        }
    }
}
