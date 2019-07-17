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
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string SpecialKPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Mods\\SpecialK",
                                       SpecialKGlobalPath = SpecialKPath + "\\Global",
                                       SpecialKWhitelistPath = SpecialKGlobalPath + "\\whitelist.ini";
        private static List<string> WhitelistedExecutables = new List<string>(),
                                    BlacklistedExecutables = new List<string>();

        private readonly List<KeyValuePair<string, int>> InjectableWindows = new List<KeyValuePair<string, int>>();
        private globalKeyboardHook keyboardHook = null;
        private static bool keyAlt = false,
                            keyX = false,
                            keyCombo = false;
        public static bool WhitelistAutomatically = false;

        private static Manage ManageForm = null;

        public TrayIconApp()
        {
            InitializeComponent();

            string fileVersion = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;

            menuAbout.Text = "About (" + fileVersion + ")";
            Log.Info("Running version {FileVersion}", fileVersion);


            if (File.Exists(SpecialKWhitelistPath))
            {
                WhitelistedExecutables.AddRange(File.ReadAllLines(SpecialKWhitelistPath));
            } else if (Directory.Exists(SpecialKPath))
            {
                Directory.CreateDirectory(SpecialKGlobalPath);

                using (FileStream fs = File.Create(SpecialKWhitelistPath))
                {
                    fs.Close();
                }
            }

            BlacklistedExecutables.AddRange(File.ReadAllLines("blacklist.ini"));

            menuHotkey.Checked = Properties.Settings.Default.keyboardShortcut;
            WhitelistAutomatically = menuWhitelistAuto.Checked = Properties.Settings.Default.autoWhitelist;
        }

        private void PopulateProcessList()
        {
            menuInject.DropDownItems.Clear();
            InjectableWindows.Clear();
            
            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        if (!BlacklistedExecutables.Contains(p.ProcessName.ToLower() + ".exe"))
                        {
                            ToolStripMenuItem newMenuItem = new ToolStripMenuItem(p.MainWindowTitle, null);
                            newMenuItem.Click += new EventHandler(this.ManualInjection_Click);
                            menuInject.DropDownItems.Add(newMenuItem);

                            InjectableWindows.Add(new KeyValuePair<string, int>(p.MainWindowTitle, p.Id));
                        }
                        else
                        {
                            Log.Warn("Skipped window from blacklisted process {ProcessName}: {WindowTitle}", p.ProcessName, p.MainWindowTitle);
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
            int processID = (from window in InjectableWindows where window.Key == sender.ToString() select window.Value).FirstOrDefault();

            if(processID > 0)
            {
                Log.Info("Trying to manually inject into process {PID}", processID);
                NativeMethods.InjectDLL(processID);
            }
        }

        private void ClosingTrayIconWindow(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.keyboardShortcut = menuHotkey.Checked;
            Properties.Settings.Default.autoWhitelist = menuWhitelistAuto.Checked;
            Properties.Settings.Default.Save();
        }

        private void OpeningContextMenu(object sender, System.ComponentModel.CancelEventArgs e)
        {
            menuSKIM64.Enabled = File.Exists(SpecialKPath + "\\SKIM64.exe");
            PopulateProcessList();
        }

        private void ClickedExit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClickedLog(object sender, EventArgs e)
        {
            NLog.Targets.FileTarget fileTarget = NLog.LogManager.Configuration.FindTargetByName<NLog.Targets.FileTarget>("logfile");
            if (fileTarget != null)
            {
                string fileName = fileTarget.FileName.ToString().Replace("'", "");
                Process.Start(fileName);
            }
        }

        private void ClickedBrowse(object sender, EventArgs e)
        {
            Process.Start(SpecialKPath);
        }

        private void ChangedHotkeyChecked(object sender, EventArgs e)
        {
            if (menuHotkey.Checked)
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

        private void ChangedWhitelistAutomaticallyChecked(object sender, EventArgs e)
        {
            WhitelistAutomatically = menuWhitelistAuto.Checked;
        }

        private void ClickedManage(object sender, EventArgs e)
        {
            if (ManageForm != null && !ManageForm.IsDisposed)
            {
                ManageForm.Show();
                ManageForm.BringToFront();
            } else
            {
                ManageForm = new Manage();
                ManageForm.FormClosing += (object formSender, FormClosingEventArgs formEvent) =>
                {
                    if (!ManageForm.ActiveOperation)
                    {
                        formEvent.Cancel = true;
                        ManageForm.Hide();
                    }
                    else
                    {
                        ManageForm.Cancel();
                        //MessageBox.Show("Cannot close the window while an operation is in progress!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                ManageForm.Show();
            }
        }

        private void ClickedSKIM64(object sender, EventArgs e)
        {
            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = SpecialKPath + "\\SKIM64.exe";
            process.WorkingDirectory = SpecialKPath;
            Process.Start(process);
        }

        private void ClickedAbout(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Idearum/SK-AltInjector");
        }

        private void MenuWhitelistEdit_Click(object sender, EventArgs e)
        {
            Process.Start(SpecialKWhitelistPath);
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
            if (!WhitelistedExecutables.Contains(processName + ".exe"))
            {
                Log.Info("Adding {processName}.exe to the whitelist.ini of Special K.", processName);
                WhitelistedExecutables.Add(processName + ".exe");
                File.WriteAllLines(SpecialKWhitelistPath, WhitelistedExecutables.ToArray());
            } else
            {
                Log.Info("{processName} already exists in the whitelist.ini of Special K.", processName + ".exe");
            }
        }
    }
}
