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
               _tmpDownloadRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Mods\\SpecialK\\Archives";
        bool ActiveOperation = false;
        private static readonly NLog.Logger AppLog = NLog.LogManager.GetCurrentClassLogger();

        public Manage()
        {
            InitializeComponent();
            Directory.CreateDirectory(_tmpDownloadRoot);

            Log("**WARNING**\r\nThis is heavily work in progress, and isn't recommended for mainstream use yet!\r\nUse at your own risk!\r\n");

            Log("Downloading repository data...");
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                wc.DownloadFileAsync(new System.Uri("https://raw.githubusercontent.com/Idearum/SK-AltInjector/master/64bitMainApp/repository.json"), "repository.json");
            }
        }

        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                Log("Reading repository data...");
                _products = JObject.Parse(File.ReadAllText("repository.json"));

                foreach (System.Collections.Generic.KeyValuePair<string, JToken> product in _products)
                {
                    cbProducts.Items.Add(product.Key);
                }

                Log("Ready to be used!");
                tsProgress.Visible = false;

                lSelectProduct.Enabled = true;
                lSelectBranch.Enabled = true;
                lSelectVersion.Enabled = true;
                cbProducts.Enabled = true;
                cbBranches.Enabled = true;
                cbVersions.Enabled = true;
                tbSelectedProduct.Enabled = true;
                tbSelectedBranch.Enabled = true;
                tbSelectedVersion.Enabled = true;

            }
            catch { }
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //Log("Downloading..." + e.ProgressPercentage.ToString() + "% complete (" + e.BytesReceived + " of " + e.TotalBytesToReceive + ")");
            tsProgress.Value = e.ProgressPercentage;
            tsProgress.Visible = true;
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
            catch (System.Exception Ex)
            {
                //handle error
                Log("Error during extraction! " + Ex.Message);
                throw Ex;
            }

            Log("Extraction finished!");
        }

        private void Log(string message)
        {
            tsStatus.Text = message;
            tbLog.Text += message + "\r\n";
            AppLog.Info(message);
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

            bool fileExists = File.Exists(_tmpDownloadPath);
            bool cancel = false;

            if (fileExists)
            {
                Log("A file called " + _tmpArchiveName + " was found in the downloads folder. Prompting user on how to proceed.");
                switch (MessageBox.Show("A file called " + _tmpArchiveName + " was found in the downloads folder. Do you want to use that one? Clicking 'No' will redownload the file from the Internet.", "Local file was found", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
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
                        Log("User canceled the install process.");
                        break;
                }
            }

            if (cancel == false)
            {
                if (fileExists == false)
                {
                    Log("Downloading " + _tmpDownloadURL);
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                        wc.DownloadFileCompleted += Wc_DownloadGlobalCompleted;
                        wc.DownloadFileAsync(new System.Uri(_tmpDownloadURL), _tmpDownloadPath);
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
            ExtractFile(_tmpDownloadPath, _SpecialKRoot);

            Directory.CreateDirectory(_SpecialKRoot + "\\Version");

            string tmpBranchName = (_branchSelected == "Main") ? "Latest" : _branchSelected;
            string contents = "[Version.Local]\r\nInstallPackage=" + _versionSelected["InstallPackage"].ToString() + "\r\nBranch=" + tmpBranchName + "\r\n\r\n[Update.User]\r\nFrequency=never\r\n";

            File.WriteAllText(_SpecialKRoot + "\\Version\\installed.ini", contents);

            Log("\r\nInstallation finished!\r\n");

            ActiveOperation = false;
        }

        private void PerformSteamInstall()
        {
            bool fileExists = File.Exists(_tmpDownloadPath);
            bool cancel = false;

            if (fileExists)
            {
                Log("A file called " + _tmpArchiveName + " was found in the downloads folder. Prompting user on how to proceed.");
                switch (MessageBox.Show("A file called " + _tmpArchiveName + " was found in the downloads folder. Do you want to use that one? Clicking 'No' will redownload the file from the Internet.", "Local file was found", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
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
                        Log("User canceled the install process.");
                        break;
                }
            }

            if (cancel == false)
            {
                if (fileExists == false)
                {
                    Log("Downloading " + _tmpDownloadURL);
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                        wc.DownloadFileCompleted += Wc_DownloadSteamCompleted;
                        wc.DownloadFileAsync(new System.Uri(_tmpDownloadURL), _tmpDownloadPath);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void Wc_DownloadSteamCompleted(object sender, AsyncCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Wc_DownloadGlobalCompleted(object sender, AsyncCompletedEventArgs e)
        {
            tsProgress.Visible = false;
            Log("Download completed!");
            FinalizeGlobalInstall();
        }
    }
}
