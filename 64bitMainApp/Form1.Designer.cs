namespace AltInjector
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemManual = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editWhitelistiniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHotkey = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.openLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSpecialKFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "SK-TinyInjector";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemManual,
            this.settingsToolStripMenuItem,
            this.toolStripMenuItemExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(241, 133);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // toolStripMenuItemManual
            // 
            this.toolStripMenuItemManual.Name = "toolStripMenuItemManual";
            this.toolStripMenuItemManual.Size = new System.Drawing.Size(240, 32);
            this.toolStripMenuItemManual.Text = "Inject";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editWhitelistiniToolStripMenuItem,
            this.toolStripMenuItemHotkey,
            this.openLogFileToolStripMenuItem,
            this.openSpecialKFolderToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(240, 32);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // editWhitelistiniToolStripMenuItem
            // 
            this.editWhitelistiniToolStripMenuItem.Name = "editWhitelistiniToolStripMenuItem";
            this.editWhitelistiniToolStripMenuItem.Size = new System.Drawing.Size(320, 34);
            this.editWhitelistiniToolStripMenuItem.Text = "Edit whitelist.ini";
            this.editWhitelistiniToolStripMenuItem.Click += new System.EventHandler(this.EditWhitelistiniToolStripMenuItem_Click);
            // 
            // toolStripMenuItemHotkey
            // 
            this.toolStripMenuItemHotkey.CheckOnClick = true;
            this.toolStripMenuItemHotkey.Name = "toolStripMenuItemHotkey";
            this.toolStripMenuItemHotkey.Size = new System.Drawing.Size(320, 34);
            this.toolStripMenuItemHotkey.Text = "Keyboard shortcut (Alt+X)";
            this.toolStripMenuItemHotkey.CheckedChanged += new System.EventHandler(this.toolStripMenuItemHotkey_CheckedChanged);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(240, 32);
            this.toolStripMenuItemExit.Text = "Exit";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItem3_Click);
            // 
            // openLogFileToolStripMenuItem
            // 
            this.openLogFileToolStripMenuItem.Name = "openLogFileToolStripMenuItem";
            this.openLogFileToolStripMenuItem.Size = new System.Drawing.Size(320, 34);
            this.openLogFileToolStripMenuItem.Text = "Open log file";
            this.openLogFileToolStripMenuItem.Click += new System.EventHandler(this.OpenLogFileToolStripMenuItem_Click);
            // 
            // openSpecialKFolderToolStripMenuItem
            // 
            this.openSpecialKFolderToolStripMenuItem.Name = "openSpecialKFolderToolStripMenuItem";
            this.openSpecialKFolderToolStripMenuItem.Size = new System.Drawing.Size(320, 34);
            this.openSpecialKFolderToolStripMenuItem.Text = "Open Special K folder";
            this.openSpecialKFolderToolStripMenuItem.Click += new System.EventHandler(this.OpenSpecialKFolderToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 422);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SK-TinyInjector";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemManual;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHotkey;
        private System.Windows.Forms.ToolStripMenuItem editWhitelistiniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSpecialKFolderToolStripMenuItem;
    }
}