using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace AltInjector
{
    public partial class Form1 : Form
    {
        private List<KeyValuePair<string, int>> processList;

        public Form1()
        {
            InitializeComponent();
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopulateProcessList()
        {
            this.toolStripMenuItemManual.DropDownItems.Clear();
            processList = new List<KeyValuePair<string, int>>();

            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0 && NativeMethods.GetProcessUser(p) == Environment.UserName)
                    {
                        System.Windows.Forms.ToolStripMenuItem newMenuItem = new System.Windows.Forms.ToolStripMenuItem(p.MainWindowTitle, null);
                        newMenuItem.Click += new System.EventHandler(this.ManualInjection_Click);
                        this.toolStripMenuItemManual.DropDownItems.Add(newMenuItem);

                        processList.Add(new KeyValuePair<string, int>(p.MainWindowTitle, p.Id));
                    }
                }
                catch
                {
                }
            }
        }

        private void ManualInjection_Click(object sender, EventArgs e)
        {
            int processID = (from process in processList where process.Key == sender.ToString() select process.Value).FirstOrDefault();

            if(processID > 0)
            {
                NativeMethods.InjectDLL(processID);
            }
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            PopulateProcessList();
        }
    }
}
