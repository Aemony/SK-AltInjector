using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace AltInjector
{
    public partial class Manage : Form
    {
        JObject _products = null;
        JObject _versionSelected = null;
        string _productSelected = "",
               _branchSelected = "",
               _tmpDownloadURL = "",
               _tmpDownloadPath = "",
               _tmpArchiveName = "",
               _SpecialKRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Mods\\SpecialK",
               _tmpDownloadRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Mods\\SpecialK_Archives";
        private static readonly NLog.Logger AppLog = NLog.LogManager.GetCurrentClassLogger();
        WebClient GlobalDownload = null;
        private bool CancelOperation = false;
        public bool ActiveOperation { get; private set; }

        public Manage()
        {
            InitializeComponent();
            Icon = Properties.Resources.pokeball;

            Directory.CreateDirectory(_tmpDownloadRoot);

            Log("**WARNING**\r\nThis is still a work in progress, and isn't recommended for mainstream use yet!\r\nUse at your own risk!\r\n");

            Log("Downloading repository data...");
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += OnDownloadProgressChanged;
                wc.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
                {
                    Log("Reading repository data...");
                    _products = JObject.Parse(File.ReadAllText("repository.json"));

                    foreach (KeyValuePair<string, JToken> product in _products)
                    {
                        cbProducts.Items.Add(product.Key);
                    }

                    Log("Ready to be used!\r\n");
                    tsProgress.Visible = false;

                    tbLog.Enabled = true;
                    lSelectProduct.Enabled = true;
                    lSelectBranch.Enabled = true;
                    lSelectVersion.Enabled = true;
                    cbProducts.Enabled = true;
                    cbBranches.Enabled = true;
                    cbVersions.Enabled = true;
                    tbSelectedProduct.Enabled = true;
                    tbSelectedBranch.Enabled = true;
                    tbSelectedVersion.Enabled = true;
                };
                wc.DownloadFileAsync(new Uri("https://raw.githubusercontent.com/Idearum/SK-AltInjector/master/64bitMainApp/repository.json"), "repository.json");
            }
        }

        private void Log(string message)
        {
            tsStatus.Text = message;
            tbLog.Text += message + "\r\n";
            AppLog.Info(message.Replace("\r\n\r\n", " ").Replace("\r\n", " "));
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (!tsProgress.IsDisposed)
            {
                tsProgress.Value = e.ProgressPercentage;
                tsProgress.Visible = true;
            }
        }

        private void CbProducts_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            bAutomatic.Enabled = false;
            bManual.Enabled = false;
            cbVersions.Items.Clear(); // Clear versions first
            cbBranches.Items.Clear(); // Next clear branches
            _productSelected = cbProducts.SelectedItem.ToString();

            /* Populate Branches */
            foreach (JToken branch in _products[_productSelected]["Branches"])
            {
                string branchKey = branch.ToObject<JProperty>().Name;
                cbBranches.Items.Add(branchKey);
            }
            if (cbBranches.Items.Count > 0)
            {
                cbBranches.SelectedIndex = 0;
            }
            /* Populate Branches END */

            tbSelectedProduct.Text = _products[_productSelected]["Description"].ToString();
        }

        private void CbBranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            _versionSelected = null; // Reset selected version
            cbVersions.Items.Clear(); // Clear versions
            tbSelectedVersion.Text = "";
            _branchSelected = cbBranches.SelectedItem.ToString();

            tbSelectedBranch.Text = _products[_productSelected]["Branches"][_branchSelected].ToString();

            /* Populate Versions */
            string token = "$.['" + _productSelected + "'].Versions[?(@.Branches[?(@ == '" + _branchSelected + "')])]"; // Fetches all versions applicable for the selected branch
            IEnumerable<JToken> versions = _products.SelectTokens(token);

            foreach (JToken item in versions)
            {
                cbVersions.Items.Add(item["Name"]);
            }

            if (cbVersions.Items.Count > 0)
            {
                cbVersions.SelectedIndex = 0;

                /* Enable Buttons */
                foreach (JToken method in _products[_productSelected]["Install"])
                {
                    switch (method.ToString())
                    {
                        case "Global":
                            bAutomatic.Enabled = true;
                            bAutomatic.Text = "Automatic";
                            break;
                        case "Steam":
                            bAutomatic.Enabled = true;
                            bAutomatic.Text = "Automatic (Steam)";
                            break;
                        case "Manual":
                            bManual.Enabled = true;
                            break;
                        default:
                            Log("Unsupported install method!");
                            break;
                    }
                }
                /* Enable Buttons END */
            }
            else
            {
                bAutomatic.Enabled = false;
                bManual.Enabled = false;
            }
            /* Populate Versions END */
        }

        private void CbVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            string token = "$.['" + _productSelected + "'].Versions[?(@.Name == '" + cbVersions.SelectedItem.ToString() + "')]"; // Fetches the specified version 
            _versionSelected = _products.SelectToken(token).ToObject<JObject>();
            tbSelectedVersion.Text = _versionSelected["Description"].ToString();

            if (_versionSelected["ReleaseNotes"].ToString() != "")
            {
                linkReleaseNotes.Enabled = true;
            }
            else
            {
                linkReleaseNotes.Enabled = false;
            }
        }

        private void LinkReleaseNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(_versionSelected["ReleaseNotes"].ToString());
        }

        public void ExtractFile(string sourceArchive, string destination)
        {
            string zPath = @"7za.exe";
            Log("Extracting using: 7za.exe " + string.Format("x \"{0}\" -y -o\"{1}\"", sourceArchive, destination));
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.FileName = zPath;
                pro.Arguments = string.Format("x \"{0}\" -y -o\"{1}\"", sourceArchive, destination);
                Process x = Process.Start(pro);
                x.WaitForExit();
            }
            catch (Exception Ex)
            {
                //handle error
                Log("Error during extraction! " + Ex.Message);
                throw Ex;
            }

            Log("Extraction finished!");
        }

        private void BAutomatic_Click(object sender, EventArgs e)
        {
            _tmpDownloadURL = _versionSelected["Archive"].ToString();
            _tmpArchiveName = _tmpDownloadURL.Split('/').Last();
            _tmpDownloadPath = _tmpDownloadRoot + "\\" + _tmpArchiveName;

            switch (bAutomatic.Text)
            {
                case "Automatic":
                    PerformGlobalInstall();
                    break;
                case "Automatic (Steam)":
                    break;
                case "Manual":
                    break;
                default:
                    Log("Unsupported install method!");
                    break;
            }
        }

        private void PerformGlobalInstall()
        {
            if (ActiveOperation)
                return;

            ActiveOperation = true;
            string _SpecialKRenamed = _SpecialKRoot + DateTime.Now.ToString("_yyyy-MM-dd_HH.mm.ss");
            bool folderRenamed = false;

            if (cbCleanInstall.Checked && Directory.Exists(_SpecialKRoot))
            {
                DialogResult failedDialogResult = DialogResult.None;

                switch (MessageBox.Show("Performing a clean install will rename the Special K folder to:\r\n\r\n" + _SpecialKRenamed + "\r\n\r\nThis will disable any texture mods or game-specific profiles that might be installed until they're manually moved back.\r\n\r\nAre you sure you want to continue with the clean install?", "Perform clean install?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
                {
                    case DialogResult.Yes:
                        Log("User chose to perform a clean install. Renaming " + _SpecialKRoot + " to " + _SpecialKRenamed);
                        do
                        {
                            failedDialogResult = DialogResult.None;
                            try
                            {
                                Directory.Move(_SpecialKRoot, _SpecialKRenamed);
                                Log("Successfully renamed the folder.");
                                folderRenamed = true;
                            }
                            catch
                            {
                                Log("Failed to rename the folder!");
                                failedDialogResult = MessageBox.Show("Can not rename Special K folder to:\r\n\r\n" + _SpecialKRenamed + "\r\n\r\nThis is most likely because SKIM is currently running, the folder or its parent folder is opened in File Explorer, or a file therein is currently in-use by another application.", "Failed to move Special K folder", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                            }
                        } while (failedDialogResult == DialogResult.Retry);
                        if (failedDialogResult == DialogResult.Cancel)
                        {
                            ActiveOperation = false;
                            return;
                        }
                        break;
                    case DialogResult.No:
                        ActiveOperation = false;
                        Log("User chose to abort the install.");
                        return;
                }
            }

            bool fileExists = File.Exists(_tmpDownloadPath);
            bool cancel = false;

            if (fileExists)
            {
                Log("A file called " + _tmpArchiveName + " was found in the downloads folder. Prompting user on how to proceed.");
                switch (MessageBox.Show("A file called " + _tmpArchiveName + " was found in the downloads folder.\r\nDo you want to use that one?\r\n\r\nClicking 'No' will redownload the file from the Internet.", "Found local file for selected version", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3))
                {
                    case DialogResult.Yes:
                        Log("User chose to reuse the local copy found.");
                        break;
                    case DialogResult.No:
                        fileExists = false;
                        Log("User chose to redownload the archive from the Internet.");
                        break;
                    case DialogResult.Cancel:
                        cancel = true;
                        ActiveOperation = false;
                        Log("User canceled the install process.");
                        break;
                }
            }

            if (cancel == false)
            {
                if (fileExists == false)
                {
                    Log("Downloading " + _tmpDownloadURL);
                    using (GlobalDownload = new WebClient())
                    {
                        GlobalDownload.DownloadProgressChanged += OnDownloadProgressChanged;
                        GlobalDownload.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
                        {
                            if (!tsProgress.IsDisposed)
                            {
                                tsProgress.Visible = false;
                            }

                            if (e.Cancelled)
                            {
                                Log("Download cancelled!");
                                Log("Cleaning up incomplete file " + _tmpDownloadPath);
                                if(File.Exists(_tmpDownloadPath))
                                    File.Delete(_tmpDownloadPath);
                                Log("Restoring original folder name");
                                if (folderRenamed)
                                    Directory.Move(_SpecialKRenamed, _SpecialKRoot);
                                ActiveOperation = false;
                                CancelOperation = true;
                                GlobalDownload.Dispose();
                                GlobalDownload = null;
                                return;
                            }

                            if (e.Error != null)
                            {
                                Log("Download failed!");
                                ActiveOperation = false;
                                CancelOperation = true;
                                GlobalDownload.Dispose();
                                GlobalDownload = null;
                                return;
                            }

                            Log("Download completed!");
                            FinalizeGlobalInstall();
                            GlobalDownload.Dispose();
                            GlobalDownload = null;
                        };
                        GlobalDownload.DownloadFileAsync(new Uri(_tmpDownloadURL), _tmpDownloadPath);
                    }
                }
                else
                {
                    FinalizeGlobalInstall();
                }
            }
        }

        private void FinalizeGlobalInstall()
        {
            if (CancelOperation)
            {
                ActiveOperation = CancelOperation = false;
                return;
            }

            try
            {
                ExtractFile(_tmpDownloadPath, _SpecialKRoot);
            } catch {
                ActiveOperation = CancelOperation = false;
                return;
            }

            Directory.CreateDirectory(_SpecialKRoot + "\\Version");

            Log("Downloading repository.ini...");
            string versionINI = "https://raw.githubusercontent.com/Kaldaien/SpecialK/0.10.x/version.ini";
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += OnDownloadProgressChanged;
                wc.DownloadFile(versionINI, _SpecialKRoot + "\\Version\\repository.ini");
            }

            string tmpBranchName = "";
            switch (_branchSelected)
            {
                case "Main":
                    tmpBranchName = "Latest";
                    break;
                case "0.9.x":
                    tmpBranchName = "Compatibility";
                    break;
                case "0.8.x":
                    tmpBranchName = "Ancient";
                    break;
                default:
                    tmpBranchName = _branchSelected;
                    break;
            }

            string updateFrequency = "\r\nReminder=0";
            DialogResult dialogResult = MessageBox.Show("Do you want to enable the internal auto-update checks of Special K? This may trigger an update prompt on the next injection of Special K into a game.", "Enable auto-updates for Special K?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.No)
                updateFrequency = "never";

            string installedINI = "[Version.Local]\r\nInstallPackage=" + _versionSelected["InstallPackage"].ToString() + "\r\nBranch=" + tmpBranchName + "\r\n\r\n[Update.User]\r\nFrequency=" + updateFrequency + "\r\n";

            Log("Creating appropriate installed.ini...");
            File.WriteAllText(_SpecialKRoot + "\\Version\\installed.ini", installedINI);

            if (File.Exists(_SpecialKRoot + "\\SKIM64.exe"))
            {
                Log("Creating shortcut to SKIM64...");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\Special K");
                string shortcutLocation = Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\Special K\\SKIM64.lnk";
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutLocation);

                shortcut.WorkingDirectory = _SpecialKRoot;
                shortcut.Description = "Special K Install Manager";
                shortcut.IconLocation = _SpecialKRoot + "\\SKIM64.exe";
                shortcut.TargetPath = _SpecialKRoot + "\\SKIM64.exe";
                shortcut.Save();
            }

            Log("\r\nInstallation finished!\r\n");

            ActiveOperation = CancelOperation = false;
        }

        public void Cancel()
        {
            CancelOperation = true;
            if (GlobalDownload != null)
               GlobalDownload.CancelAsync();
        }
    }
}
