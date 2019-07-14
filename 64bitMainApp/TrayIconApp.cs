using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utilities;

namespace AltInjector
{
    public partial class TrayIconApp : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string[] Blacklist = File.ReadAllLines("blacklist.ini");
        private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                       GlobalPath = DocumentsPath + "\\My Mods\\SpecialK\\Global",
                                       WhitelistPath = GlobalPath + "\\whitelist.ini";
        private static List<string> Whitelist;

        private readonly List<KeyValuePair<string, int>> processList = new List<KeyValuePair<string, int>>();
        private globalKeyboardHook keyboardHook = null;
        private bool keyAlt = false,
                     keyX = false,
                     keyCombo = false;
        public static bool WhitelistAuto = false;

        public TrayIconApp()
        {
            InitializeComponent();
            
            if (!File.Exists(WhitelistPath))
            {
                if (!Directory.Exists(GlobalPath))
                {
                    Directory.CreateDirectory(GlobalPath);
                }

                using (FileStream fs = File.Create(WhitelistPath))
                {
                    fs.Close();
                }
            }

            Whitelist = new List<string>(File.ReadAllLines(WhitelistPath));

            menuHotkey.Checked = Properties.Settings.Default.keyboardShortcut;
            menuWhitelistAuto.Checked = Properties.Settings.Default.autoWhitelist;
            WhitelistAuto = Properties.Settings.Default.autoWhitelist;
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopulateProcessList()
        {
            menuInject.DropDownItems.Clear();
            processList.Clear();
            
            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        bool isBlacklisted = Array.Exists(Blacklist, x => x == p.ProcessName.ToLower());
                        if (isBlacklisted == false)
                        {
                            ToolStripMenuItem newMenuItem = new ToolStripMenuItem(p.MainWindowTitle, null);
                            newMenuItem.Click += new EventHandler(this.ManualInjection_Click);
                            menuInject.DropDownItems.Add(newMenuItem);

                            processList.Add(new KeyValuePair<string, int>(p.MainWindowTitle, p.Id));
                        }
                        else
                        {
                            Logger.Warn("Skipped window from blacklisted process {ProcessName}: {WindowTitle}", p.ProcessName, p.MainWindowTitle);
                        }
                    }
                }
                catch { }

                p.Dispose();
            }

            if (menuInject.DropDownItems.Count == 0)
            {
                ToolStripMenuItem newMenuItem = new ToolStripMenuItem("No compatible windows found");
                newMenuItem.Enabled = false;
                menuInject.DropDownItems.Add(newMenuItem);
            }
        }

        private void ManualInjection_Click(object sender, EventArgs e)
        {
            int processID = (from process in processList where process.Key == sender.ToString() select process.Value).FirstOrDefault();

            if(processID > 0)
            {
                Logger.Info("Trying to manually inject into process {PID}", processID);
                NativeMethods.InjectDLL(processID);
            }
        }

        private void OpenLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileTarget fileTarget = NLog.LogManager.Configuration.FindTargetByName<FileTarget>("logfile");
            if (fileTarget != null)
            {
                string fileName = fileTarget.FileName.ToString().Replace("'", "");
                MessageBox.Show(fileName);
                MessageBox.Show(Environment.CurrentDirectory);
                Process.Start(fileName);
            }
        }

        private void OpenSpecialKFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SpecialK = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Mods\\SpecialK";
            if (Directory.Exists(SpecialK))
            {
                Process.Start(SpecialK);
            }
        }

        private void TrayIconWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.keyboardShortcut = menuHotkey.Checked;
            Properties.Settings.Default.autoWhitelist = menuWhitelistAuto.Checked;
            Properties.Settings.Default.Save();
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Idearum/SK-AltInjector");
        }

        private void MenuWhitelistAuto_CheckedChanged(object sender, EventArgs e)
        {
            WhitelistAuto = menuWhitelistAuto.Checked;
        }

        private void MenuHotkey_CheckedChanged(object sender, EventArgs e)
        {
            if (menuHotkey.Checked == true)
            {
                if (keyboardHook == null)
                {
                    keyboardHook = new globalKeyboardHook();
                    keyboardHook.HookedKeys.Add(Keys.LMenu); // Left Alt
                    keyboardHook.HookedKeys.Add(Keys.X); // X
                    keyboardHook.KeyUp += new KeyEventHandler(KeyboardHook_KeyUp);
                    keyboardHook.KeyDown += new KeyEventHandler(KeyboardHook_KeyDown);
                }
                else
                {
                    keyboardHook.hook();
                }
            }
            else
            {
                if (keyboardHook != null)
                {
                    keyboardHook.unhook();
                }
            }
        }

        private void ContextMenuTrayIcon_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PopulateProcessList();
        }

        private void MenuWhitelistEdit_Click(object sender, EventArgs e)
        {
            string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                   GlobalPath = DocumentsPath + "\\My Mods\\SpecialK\\Global",
                   whitelistPath = GlobalPath + "\\whitelist.ini";

            if (!File.Exists(whitelistPath))
            {
                if (!Directory.Exists(GlobalPath))
                {
                    Directory.CreateDirectory(GlobalPath);
                }

                using (FileStream fs = File.Create(whitelistPath))
                {
                    fs.Close();
                }
            }

            Process.Start(whitelistPath);
        }

        private void KeyboardHook_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.LMenu:
                    keyAlt = false;
                    break;
                case Keys.X:
                    keyX = false;
                    break;
            }

            if(keyCombo && !keyAlt && !keyX)
            {
                keyCombo = !keyCombo; // Resets the state when all keys have been let go.
            }
        }

        private void KeyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.LMenu:
                    keyAlt = true;
                    break;
                case Keys.X:
                    keyX = true;
                    break;
            }

            if(keyAlt && keyX && !keyCombo)
            {
                keyCombo = !keyCombo; // Prevents this section from executing more than once per key press combo
                NativeMethods.InjectDLLIntoActiveWindow();
            }
        }

        public static void AddProcessToWhitelist(string processName)
        {
            processName = processName.ToLower();
            if (!Whitelist.Contains(processName + ".exe"))
            {
                Logger.Info("Adding {processName}.exe to the whitelist.ini of Special K.", processName);
                Whitelist.Add(processName + ".exe");
                File.WriteAllLines(WhitelistPath, Whitelist.ToArray());
            } else
            {
                Logger.Info("{processName} already exists in the whitelist.ini of Special K.", processName + ".exe");
            }
        }
    }
}
