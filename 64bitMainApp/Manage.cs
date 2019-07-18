using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace AltInjector
{
    public partial class Manage : Form
    {
        private JObject JsonRepostiory = null;
        private JObject _versionSelected = null;
        private string _productSelected = "",
               _branchSelected = "",
               _tmpDownloadURL = "",
               _tmpDownloadPath = "",
               _tmpArchiveName = "",
               _tmpExtractionPath = Path.GetTempPath() + "\\SK-TinyInjector",
               _SpecialKRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Mods\\SpecialK",
               _tmpDownloadRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Mods\\SpecialK_Archives",
               InstallingVersion = "",
               InstallingBranch = "",
               LocalInstallPath = "";
        private bool LocalInstall64bit = false;
        private ApiDialogResult LocalInstallAPI = ApiDialogResult.None;
        private static readonly NLog.Logger AppLog = NLog.LogManager.GetCurrentClassLogger();
        private WebClient OngoingDownload = null;
        private bool CancelOperation = false;
        public bool ActiveOperation { get; private set; }

        public Manage()
        {
            InitializeComponent();
            lCurrentBranch.Text = "";
            lCurrentVersion.Text = "";
            Icon = Properties.Resources.pokeball;

            Directory.CreateDirectory(_tmpDownloadRoot);

            Log("**WARNING**\r\nThis is still a work in progress, and isn't recommended for mainstream use yet!\r\nUse at your own risk!\r\n");

            Log("Downloading repository data...");
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += OnDownloadProgressChanged;
                wc.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
                {
                    tsProgress.Visible = false;
                    if (e.Cancelled || e.Error != null)
                    {
                        if (e.Cancelled)
                        {
                            Log("Download cancelled!");
                        }
                        else if (e.Error != null)
                        {
                            Log("Download failed!");
                            Log("Error message: " + e.Error.Message + "\r\n");
                        }

                        Log("Cleaning up incomplete file repository_new.json");
                        if (File.Exists("repository_new.json"))
                            File.Delete("repository_new.json");

                        Log("Download failed. Falling back to local repository copy.");
                        Log("Error message: " + e.Error.Message + "\r\n");
                    } else
                    {
                        File.Delete("repository.json");
                        File.Move("repository_new.json", "repository.json");
                    }

                    Log("Reading repository data...");
                    JsonRepostiory = JObject.Parse(File.ReadAllText("repository.json"));

                    foreach (KeyValuePair<string, JToken> product in JsonRepostiory)
                    {
                        cbProducts.Items.Add(product.Key);
                    }

                    Log("Ready to be used!\r\n");

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
                wc.DownloadFileAsync(new Uri("https://raw.githubusercontent.com/Idearum/SK-AltInjector/master/64bitMainApp/repository.json"), "repository_new.json");
            }
        }

        /* From https://stackoverflow.com/questions/197951/how-can-i-determine-for-which-platform-an-executable-is-compiled */
        // the enum of known pe file types
        public enum FilePEType : ushort
        {
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
            IMAGE_FILE_MACHINE_AM33 = 0x1d3,
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            IMAGE_FILE_MACHINE_ARM = 0x1c0,
            IMAGE_FILE_MACHINE_EBC = 0xebc,
            IMAGE_FILE_MACHINE_I386 = 0x14c,
            IMAGE_FILE_MACHINE_IA64 = 0x200,
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            IMAGE_FILE_MACHINE_MIPS16 = 0x266,
            IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
            IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
            IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
            IMAGE_FILE_MACHINE_R4000 = 0x166,
            IMAGE_FILE_MACHINE_SH3 = 0x1a2,
            IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
            IMAGE_FILE_MACHINE_SH4 = 0x1a6,
            IMAGE_FILE_MACHINE_SH5 = 0x1a8,
            IMAGE_FILE_MACHINE_THUMB = 0x1c2,
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
        }

        // pass the path to the file and check the return
        public static FilePEType GetFilePE(string path)
        {
            FilePEType pe = new FilePEType();
            pe = FilePEType.IMAGE_FILE_MACHINE_UNKNOWN;
            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] data = new byte[4096];
                    fs.Read(data, 0, 4096);
                    ushort result = BitConverter.ToUInt16(data, BitConverter.ToInt32(data, 60) + 4);
                    try
                    {
                        pe = (FilePEType)result;
                    }
                    catch (Exception)
                    {
                        pe = FilePEType.IMAGE_FILE_MACHINE_UNKNOWN;
                    }
                }
            }
            return pe;
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
            foreach (JToken branch in JsonRepostiory[_productSelected]["Branches"])
            {
                string branchKey = branch.ToObject<JProperty>().Name;
                cbBranches.Items.Add(branchKey);
            }
            /* Populate Branches END */
            
            // If global install, try to detect current installed branch
            if (_productSelected == "Global install" && File.Exists(_SpecialKRoot + "\\Version\\installed.ini"))
            {
                string[] InstalledINI = File.ReadAllLines(_SpecialKRoot + "\\Version\\installed.ini");

                if (InstalledINI.Length >= 3)
                {
                    string installedBranch = (InstalledINI[2].Length >= 7) ? InstalledINI[2].Substring(7) : "";
                    string installedPackage = (InstalledINI[1].Length >= 15) ? InstalledINI[1].Substring(15) : "";

                    switch (installedBranch)
                    {
                        case "Latest":
                            installedBranch = "Main";
                            break;
                        case "Compatibility":
                            installedBranch = "0.9.x";
                            break;
                        case "Ancient":
                            installedBranch = "0.8.x";
                            break;
                    }

                    Log("Detecting existing install of the global injector:");
                    Log("Branch: " + installedBranch);
                    lCurrentBranch.Text = installedBranch;

                    if (installedPackage != "")
                    {
                        JToken version = JsonRepostiory.SelectToken("$.['" + _productSelected + "'].Versions[?(@.InstallPackage == '" + installedPackage + "')]");
                        if (version != null)
                        {
                            lCurrentVersion.Text = version["Name"].ToString();
                            Log("Version: " + lCurrentVersion.Text + "\r\n");
                        }

                        if (installedBranch != "")
                        cbBranches.SelectedItem = installedBranch;
                    } else
                    {
                        Log("Version: Could not detect version.\r\n");
                    }
                }
            } else
            {
                lCurrentBranch.Text = "";
                lCurrentVersion.Text = "";
                if (cbBranches.Items.Count > 0)
                {
                    cbBranches.SelectedIndex = 0;
                }
            }

            tbSelectedProduct.Text = JsonRepostiory[_productSelected]["Description"].ToString();
        }

        private void ClickedManual(object sender, EventArgs e)
        {
            if (ActiveOperation)
                return;

            _tmpDownloadURL = _versionSelected["Archive"].ToString();
            _tmpArchiveName = _tmpDownloadURL.Split('/').Last();
            _tmpDownloadPath = _tmpDownloadRoot + "\\" + _tmpArchiveName;

            switch (bManual.Text)
            {
                case "Local (game-specific)":
                    PerformLocalInstall();
                    break;
                case "Manual":
                    break;
                default:
                    Log("Unsupported install method!");
                    break;
            }
        }

        private void PerformLocalInstall()
        {
            ActiveOperation = true;

            /*
             * 1. Browse for appropriate executable.
             * 2. Download and extract Special K archive to a dummy folder.
             * 3. Detect 32-bit or 64-bit from the file executable.
             * 4. Prompt user about API support (might be able to be automated in the future?)
             * 5. Move appropriate SpecialK32/64.dll files to the target folder.
             * 
             */

            ApiDialog apiDialog = new ApiDialog();
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Game executables (*.exe)|*.exe",
                Title = "Browse for game executable",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                ValidateNames = true,
                RestoreDirectory = true
            };

            if(fileDialog.ShowDialog() == DialogResult.OK && apiDialog.ShowDialog() == DialogResult.OK)
            {
                LocalInstallAPI = apiDialog.Api;
                Log("Attempting to install local (game-specific) wrapper DLLs for " + fileDialog.FileName);

                FilePEType type = GetFilePE(fileDialog.FileName);

                if (type == FilePEType.IMAGE_FILE_MACHINE_AMD64)
                    LocalInstall64bit = true; // 64-bit
                else if (type == FilePEType.IMAGE_FILE_MACHINE_I386)
                    LocalInstall64bit = false; // 32-bit, or AnyCPU for .NET-based games
                else
                    Log("Unknown executable architecture."); // Unknown

                LocalInstallPath = Path.GetDirectoryName(fileDialog.FileName);

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
                        using (OngoingDownload = new WebClient())
                        {
                            OngoingDownload.DownloadProgressChanged += OnDownloadProgressChanged;
                            OngoingDownload.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
                            {
                                if (!tsProgress.IsDisposed)
                                {
                                    tsProgress.Visible = false;
                                }

                                if (e.Cancelled || e.Error != null)
                                {
                                    if (e.Cancelled)
                                    {
                                        Log("Download cancelled!");
                                    }
                                    else if (e.Error != null)
                                    {
                                        Log("Download failed!");
                                        Log("Error message: " + e.Error.Message + "\r\n");
                                    }

                                    Log("Cleaning up incomplete file " + _tmpDownloadPath);
                                    if (File.Exists(_tmpDownloadPath))
                                        File.Delete(_tmpDownloadPath);

                                    ActiveOperation = false;
                                    CancelOperation = true;
                                    OngoingDownload.Dispose();
                                    OngoingDownload = null;
                                    return;
                                }

                                Log("Download completed!");
                                FinalizeLocalInstall();
                                OngoingDownload.Dispose();
                                OngoingDownload = null;
                            };
                            OngoingDownload.DownloadFileAsync(new Uri(_tmpDownloadURL), _tmpDownloadPath);
                        }
                    }
                    else
                    {
                        FinalizeLocalInstall();
                    }
                }


            }
            ActiveOperation = false;
        }

        private void FinalizeLocalInstall()
        {
            if (CancelOperation)
            {
                ActiveOperation = CancelOperation = false;
                return;
            }

            try
            {
                string DLLFileName = (LocalInstall64bit) ? "SpecialK64.dll" : "SpecialK32.dll";
                string TargetFileName = LocalInstallAPI.ToString().ToLower() + ".dll";

                Directory.CreateDirectory(_tmpExtractionPath);
                Log("Extracting temporary files to " + _tmpExtractionPath);
                ExtractFile(_tmpDownloadPath, _tmpExtractionPath, DLLFileName);

                if (File.Exists(_tmpExtractionPath + "\\" + DLLFileName))
                {
                    if (File.Exists(LocalInstallPath + "\\" + TargetFileName))
                    {
                        Log(LocalInstallPath + "\\" + TargetFileName + " already exists. Prompting user on how to proceed.");

                        if (MessageBox.Show(LocalInstallPath + "\\" + TargetFileName + " already exists.\r\n\r\nAre you sure you want to overwrite it?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            Log("User decided to overwrite the existing file");
                            File.Copy(_tmpExtractionPath + "\\" + DLLFileName, LocalInstallPath + "\\" + TargetFileName, true);
                            Log("Copied " + DLLFileName + " to " + LocalInstallPath + "\\" + TargetFileName);
                        }
                    }
                    else
                    {
                        File.Copy(_tmpExtractionPath + "\\" + DLLFileName, LocalInstallPath + "\\" + TargetFileName);
                        Log("Copied " + _tmpExtractionPath + "\\" + DLLFileName + " to " + LocalInstallPath + "\\" + TargetFileName);
                    }

                    // Remove existing install if 'Clean Install' is checked
                    if (cbCleanInstall.Checked && File.Exists(LocalInstallPath + "\\" + LocalInstallAPI.ToString().ToLower() + ".ini"))
                    {
                        Log("Removing existing config file...");
                        File.Delete(LocalInstallPath + "\\" + LocalInstallAPI.ToString().ToLower() + ".ini");
                    }
                } else
                {
                    Log("Failed to locate appropriate DLL file in the extracted files!");
                }
            }
            catch
            {
            }
            
            Log("Cleaning up temporary files...");
            Directory.Delete(_tmpExtractionPath, true);
            
            LocalInstallAPI = ApiDialogResult.None;
            LocalInstall64bit = false;
            LocalInstallPath = "";
            ActiveOperation = CancelOperation = false;

            Log("\r\nInstallation finished!\r\n");
        }

        private void ClickedAutomatic(object sender, EventArgs e)
        {
            if (ActiveOperation)
                return;

            _tmpDownloadURL = _versionSelected["Archive"].ToString();
            _tmpArchiveName = _tmpDownloadURL.Split('/').Last();
            _tmpDownloadPath = _tmpDownloadRoot + "\\" + _tmpArchiveName;

            switch (bAutomatic.Text)
            {
                case "Global":
                    PerformGlobalInstall();
                    break;
                case "Automatic (Steam)":
                    break;
                default:
                    Log("Unsupported install method!");
                    break;
            }
        }

        private void CbBranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            _versionSelected = null; // Reset selected version
            cbVersions.Items.Clear(); // Clear versions
            tbSelectedVersion.Text = "";
            _branchSelected = cbBranches.SelectedItem.ToString();

            tbSelectedBranch.Text = JsonRepostiory[_productSelected]["Branches"][_branchSelected].ToString();

            /* Populate Versions */
            string token = "$.['" + _productSelected + "'].Versions[?(@.Branches[?(@ == '" + _branchSelected + "')])]"; // Fetches all versions applicable for the selected branch
            IEnumerable<JToken> versions = JsonRepostiory.SelectTokens(token);

            foreach (JToken item in versions)
            {
                cbVersions.Items.Add(item["Name"].ToString());
            }

            if (cbVersions.Items.Count > 0)
            {
                /* Enable Buttons */
                foreach (JToken method in JsonRepostiory[_productSelected]["Install"])
                {
                    switch (method.ToString())
                    {
                        case "Global":
                            // If global install, try to select current installed version
                            if (lCurrentVersion.Text != "")
                            {
                                cbVersions.SelectedItem = lCurrentVersion.Text;
                            }

                            bAutomatic.Enabled = true;
                            bAutomatic.Text = "Global";
                            bManual.Enabled = true;
                            bManual.Text = "Local (game-specific)";
                            break;
                        case "Steam":
                            bAutomatic.Enabled = true;
                            bAutomatic.Text = "Automatic (Steam)";
                            break;
                        case "Manual":
                            bManual.Enabled = true;
                            bManual.Text = "Manual";
                            break;
                        default:
                            Log("Unsupported install method!");
                            break;
                    }
                }
                /* Enable Buttons END */
                if(cbVersions.SelectedIndex == -1)
                    cbVersions.SelectedIndex = 0;
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
            string token = "$.['" + _productSelected + "'].Versions[?(@.Name == '" + cbVersions.SelectedItem + "')]"; // Fetches the specified version 
            _versionSelected = JsonRepostiory.SelectToken(token).ToObject<JObject>();
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
        public void ExtractFile(string sourceArchive, string destination, string fileFilter)
        {
            string zPath = @"7za.exe";
            Log("Extracting using: 7za.exe " + string.Format("x \"{0}\" -y -o\"{1}\"", sourceArchive, destination));
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.FileName = zPath;
                pro.Arguments = string.Format("x \"{0}\" -y -o\"{1}\" {2}", sourceArchive, destination, fileFilter);
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

        private void PerformGlobalInstall()
        {
            ActiveOperation = true;
            InstallingVersion = cbVersions.SelectedItem.ToString();
            InstallingBranch = cbBranches.SelectedItem.ToString();
            string _SpecialKRenamed = _SpecialKRoot + DateTime.Now.ToString("_yyyy-MM-dd_HH.mm.ss");
            bool folderRenamed = false;

            if (lCurrentVersion.Text == InstallingVersion)
            {
                if (lCurrentBranch.Text == InstallingBranch && !cbCleanInstall.Checked)
                {
                    switch(MessageBox.Show("The specified version seems to already be installed. Do you want to perform a clean install?", "Version already installed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            cbCleanInstall.Checked = true;
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            ActiveOperation = false;
                            InstallingVersion = "";
                            Log("User chose to abort the install.");
                            return;
                    }
                } else if (lCurrentBranch.Text != InstallingBranch && !cbCleanInstall.Checked)
                {
                    switch (MessageBox.Show("Do you want to perform a quick branch migration, without reinstalling Special K?", "Perform a quick branch migration?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            string[] InstalledINI = File.ReadAllLines(_SpecialKRoot + "\\Version\\installed.ini");

                            if (InstalledINI.Length >= 3)
                            {
                                string targetBranch = (InstallingBranch == "Main") ? "Latest" : "Testing";
                                InstalledINI[2] = "Branch=" + targetBranch;
                                File.WriteAllLines(_SpecialKRoot + "\\Version\\installed.ini", InstalledINI);
                            }
                            ActiveOperation = false;
                            InstallingVersion = "";
                            Log("Performed a quick branch migration from " + lCurrentBranch.Text + " to " + InstallingBranch + "!\r\n");
                            lCurrentBranch.Text = InstallingBranch;
                            return;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            ActiveOperation = false;
                            InstallingVersion = "";
                            Log("User chose to abort the install.");
                            return;
                    }
                }
            }

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
                        InstallingVersion = "";
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
                    using (OngoingDownload = new WebClient())
                    {
                        OngoingDownload.DownloadProgressChanged += OnDownloadProgressChanged;
                        OngoingDownload.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
                        {
                            if (!tsProgress.IsDisposed)
                            {
                                tsProgress.Visible = false;
                            }

                            if (e.Cancelled || e.Error != null)
                            {
                                if(e.Cancelled)
                                {
                                    Log("Download cancelled!");
                                } else if(e.Error != null)
                                {
                                    Log("Download failed!");
                                    Log("Error message: " + e.Error.Message + "\r\n");
                                }

                                Log("Cleaning up incomplete file " + _tmpDownloadPath);
                                if (File.Exists(_tmpDownloadPath))
                                    File.Delete(_tmpDownloadPath);

                                if (folderRenamed)
                                {
                                    Log("Restoring original folder name");
                                    Directory.Move(_SpecialKRenamed, _SpecialKRoot);
                                }

                                ActiveOperation = false;
                                CancelOperation = true;
                                OngoingDownload.Dispose();
                                OngoingDownload = null;
                                return;
                            }

                            Log("Download completed!");
                            FinalizeGlobalInstall();
                            OngoingDownload.Dispose();
                            OngoingDownload = null;
                        };
                        OngoingDownload.DownloadFileAsync(new Uri(_tmpDownloadURL), _tmpDownloadPath);
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
            switch (InstallingBranch)
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
                    tmpBranchName = InstallingBranch;
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

            lCurrentBranch.Text = InstallingBranch;
            InstallingBranch = "";
            lCurrentVersion.Text = InstallingVersion;
            InstallingVersion = "";

            Log("\r\nInstallation finished!\r\n");

            ActiveOperation = CancelOperation = false;
        }

        public void Cancel()
        {
            CancelOperation = true;
            if (OngoingDownload != null)
                OngoingDownload.CancelAsync();
        }
    }
}
