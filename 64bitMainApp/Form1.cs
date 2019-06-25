using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Utilities;

namespace AltInjector
{
    public partial class Form1 : Form
    {
        private List<KeyValuePair<string, int>> processList = null;
        private globalKeyboardHook keyboardHook = null;
        private bool keyAlt = false,
                     keyX = false,
                     keyCombo = false;

        public Form1()
        {
            InitializeComponent();

            toolStripMenuItemHotkey.Checked = Properties.Settings.Default.keyboardShortcut;
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
        private void toolStripMenuItemHotkey_CheckedChanged(object sender, EventArgs e)
        {
            if(toolStripMenuItemHotkey.Checked == true)
            {
                if(keyboardHook == null)
                {
                    keyboardHook = new globalKeyboardHook();
                    keyboardHook.HookedKeys.Add(Keys.LMenu); // Left Alt
                    keyboardHook.HookedKeys.Add(Keys.X); // X
                    keyboardHook.KeyUp += new KeyEventHandler(keyboardHook_KeyUp);
                    keyboardHook.KeyDown += new KeyEventHandler(keyboardHook_KeyDown);
                } else
                {
                    keyboardHook.hook();
                }
            } else
            {
                keyboardHook.unhook();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.keyboardShortcut = toolStripMenuItemHotkey.Checked;
            Properties.Settings.Default.Save();
        }

        private void keyboardHook_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.LMenu:
                    this.keyAlt = false;
                    break;
                case Keys.X:
                    this.keyX = false;
                    break;
            }

            if(this.keyCombo && !this.keyAlt && !this.keyX)
            {
                this.keyCombo = false; // Resets the state when all keys have been let go.
            }

        }

        private void keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.LMenu:
                    this.keyAlt = true;
                    break;
                case Keys.X:
                    this.keyX = true;
                    break;
            }

            if(this.keyAlt && this.keyX && !this.keyCombo)
            {
                this.keyCombo = true; // Prevents this section from executing more than once per key press combo
                NativeMethods.InjectDLLIntoActiveWindow();
            }
        }
    }
}
