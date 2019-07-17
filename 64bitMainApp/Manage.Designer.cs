namespace AltInjector
{
    partial class Manage
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
            this.cbProducts = new System.Windows.Forms.ComboBox();
            this.lSelectProduct = new System.Windows.Forms.Label();
            this.cbBranches = new System.Windows.Forms.ComboBox();
            this.cbVersions = new System.Windows.Forms.ComboBox();
            this.lSelectBranch = new System.Windows.Forms.Label();
            this.lSelectVersion = new System.Windows.Forms.Label();
            this.tbSelectedProduct = new System.Windows.Forms.TextBox();
            this.tbSelectedBranch = new System.Windows.Forms.TextBox();
            this.tbSelectedVersion = new System.Windows.Forms.TextBox();
            this.bAutomatic = new System.Windows.Forms.Button();
            this.bManual = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkReleaseNotes = new System.Windows.Forms.LinkLabel();
            this.cbCleanInstall = new System.Windows.Forms.CheckBox();
            this.lCurrentBranch = new System.Windows.Forms.Label();
            this.lCurrentVersion = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbProducts
            // 
            this.cbProducts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProducts.Enabled = false;
            this.cbProducts.FormattingEnabled = true;
            this.cbProducts.Location = new System.Drawing.Point(27, 62);
            this.cbProducts.Name = "cbProducts";
            this.cbProducts.Size = new System.Drawing.Size(408, 28);
            this.cbProducts.TabIndex = 0;
            this.cbProducts.SelectedIndexChanged += new System.EventHandler(this.CbProducts_SelectedIndexChanged);
            // 
            // lSelectProduct
            // 
            this.lSelectProduct.AutoSize = true;
            this.lSelectProduct.Enabled = false;
            this.lSelectProduct.Location = new System.Drawing.Point(23, 29);
            this.lSelectProduct.Name = "lSelectProduct";
            this.lSelectProduct.Size = new System.Drawing.Size(129, 20);
            this.lSelectProduct.TabIndex = 1;
            this.lSelectProduct.Text = "1. Select product";
            // 
            // cbBranches
            // 
            this.cbBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBranches.Enabled = false;
            this.cbBranches.FormattingEnabled = true;
            this.cbBranches.Location = new System.Drawing.Point(27, 279);
            this.cbBranches.Name = "cbBranches";
            this.cbBranches.Size = new System.Drawing.Size(408, 28);
            this.cbBranches.TabIndex = 2;
            this.cbBranches.SelectedIndexChanged += new System.EventHandler(this.CbBranches_SelectedIndexChanged);
            // 
            // cbVersions
            // 
            this.cbVersions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVersions.Enabled = false;
            this.cbVersions.FormattingEnabled = true;
            this.cbVersions.Location = new System.Drawing.Point(27, 513);
            this.cbVersions.Name = "cbVersions";
            this.cbVersions.Size = new System.Drawing.Size(408, 28);
            this.cbVersions.TabIndex = 3;
            this.cbVersions.SelectedIndexChanged += new System.EventHandler(this.CbVersions_SelectedIndexChanged);
            // 
            // lSelectBranch
            // 
            this.lSelectBranch.AutoSize = true;
            this.lSelectBranch.Enabled = false;
            this.lSelectBranch.Location = new System.Drawing.Point(23, 246);
            this.lSelectBranch.Name = "lSelectBranch";
            this.lSelectBranch.Size = new System.Drawing.Size(124, 20);
            this.lSelectBranch.TabIndex = 4;
            this.lSelectBranch.Text = "2. Select branch";
            // 
            // lSelectVersion
            // 
            this.lSelectVersion.AutoSize = true;
            this.lSelectVersion.Enabled = false;
            this.lSelectVersion.Location = new System.Drawing.Point(23, 480);
            this.lSelectVersion.Name = "lSelectVersion";
            this.lSelectVersion.Size = new System.Drawing.Size(125, 20);
            this.lSelectVersion.TabIndex = 5;
            this.lSelectVersion.Text = "3. Select version";
            // 
            // tbSelectedProduct
            // 
            this.tbSelectedProduct.BackColor = System.Drawing.SystemColors.Control;
            this.tbSelectedProduct.Enabled = false;
            this.tbSelectedProduct.Location = new System.Drawing.Point(27, 106);
            this.tbSelectedProduct.Multiline = true;
            this.tbSelectedProduct.Name = "tbSelectedProduct";
            this.tbSelectedProduct.ReadOnly = true;
            this.tbSelectedProduct.Size = new System.Drawing.Size(408, 95);
            this.tbSelectedProduct.TabIndex = 6;
            // 
            // tbSelectedBranch
            // 
            this.tbSelectedBranch.BackColor = System.Drawing.SystemColors.Control;
            this.tbSelectedBranch.Enabled = false;
            this.tbSelectedBranch.Location = new System.Drawing.Point(27, 346);
            this.tbSelectedBranch.Multiline = true;
            this.tbSelectedBranch.Name = "tbSelectedBranch";
            this.tbSelectedBranch.ReadOnly = true;
            this.tbSelectedBranch.Size = new System.Drawing.Size(408, 95);
            this.tbSelectedBranch.TabIndex = 7;
            // 
            // tbSelectedVersion
            // 
            this.tbSelectedVersion.BackColor = System.Drawing.SystemColors.Control;
            this.tbSelectedVersion.Enabled = false;
            this.tbSelectedVersion.Location = new System.Drawing.Point(27, 584);
            this.tbSelectedVersion.Multiline = true;
            this.tbSelectedVersion.Name = "tbSelectedVersion";
            this.tbSelectedVersion.ReadOnly = true;
            this.tbSelectedVersion.Size = new System.Drawing.Size(408, 95);
            this.tbSelectedVersion.TabIndex = 8;
            // 
            // bAutomatic
            // 
            this.bAutomatic.Enabled = false;
            this.bAutomatic.Location = new System.Drawing.Point(245, 785);
            this.bAutomatic.Name = "bAutomatic";
            this.bAutomatic.Size = new System.Drawing.Size(190, 35);
            this.bAutomatic.TabIndex = 9;
            this.bAutomatic.Text = "Automatic";
            this.bAutomatic.UseVisualStyleBackColor = true;
            this.bAutomatic.Click += new System.EventHandler(this.BAutomatic_Click);
            // 
            // bManual
            // 
            this.bManual.Enabled = false;
            this.bManual.Location = new System.Drawing.Point(27, 785);
            this.bManual.Name = "bManual";
            this.bManual.Size = new System.Drawing.Size(100, 35);
            this.bManual.TabIndex = 10;
            this.bManual.Text = "Manual";
            this.bManual.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgress,
            this.tsStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 853);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1230, 32);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsProgress
            // 
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(200, 24);
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(60, 25);
            this.tsStatus.Text = "Status";
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.SystemColors.Window;
            this.tbLog.Enabled = false;
            this.tbLog.Location = new System.Drawing.Point(468, 29);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(732, 791);
            this.tbLog.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(23, 752);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "4. Select install method";
            // 
            // linkReleaseNotes
            // 
            this.linkReleaseNotes.AutoSize = true;
            this.linkReleaseNotes.Enabled = false;
            this.linkReleaseNotes.Location = new System.Drawing.Point(323, 686);
            this.linkReleaseNotes.Name = "linkReleaseNotes";
            this.linkReleaseNotes.Size = new System.Drawing.Size(112, 20);
            this.linkReleaseNotes.TabIndex = 16;
            this.linkReleaseNotes.TabStop = true;
            this.linkReleaseNotes.Text = "Release notes";
            this.linkReleaseNotes.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkReleaseNotes_LinkClicked);
            // 
            // cbCleanInstall
            // 
            this.cbCleanInstall.AutoSize = true;
            this.cbCleanInstall.Location = new System.Drawing.Point(27, 685);
            this.cbCleanInstall.Name = "cbCleanInstall";
            this.cbCleanInstall.Size = new System.Drawing.Size(190, 24);
            this.cbCleanInstall.TabIndex = 17;
            this.cbCleanInstall.Text = "Perform a clean install";
            this.cbCleanInstall.UseVisualStyleBackColor = true;
            // 
            // lCurrentBranch
            // 
            this.lCurrentBranch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lCurrentBranch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCurrentBranch.Location = new System.Drawing.Point(323, 310);
            this.lCurrentBranch.Name = "lCurrentBranch";
            this.lCurrentBranch.Size = new System.Drawing.Size(112, 20);
            this.lCurrentBranch.TabIndex = 18;
            this.lCurrentBranch.Text = "current branch";
            this.lCurrentBranch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lCurrentVersion
            // 
            this.lCurrentVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lCurrentVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCurrentVersion.Location = new System.Drawing.Point(323, 544);
            this.lCurrentVersion.Name = "lCurrentVersion";
            this.lCurrentVersion.Size = new System.Drawing.Size(113, 20);
            this.lCurrentVersion.TabIndex = 19;
            this.lCurrentVersion.Text = "current version";
            this.lCurrentVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Manage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1230, 885);
            this.Controls.Add(this.lCurrentVersion);
            this.Controls.Add(this.lCurrentBranch);
            this.Controls.Add(this.cbCleanInstall);
            this.Controls.Add(this.linkReleaseNotes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.bManual);
            this.Controls.Add(this.bAutomatic);
            this.Controls.Add(this.tbSelectedVersion);
            this.Controls.Add(this.tbSelectedBranch);
            this.Controls.Add(this.tbSelectedProduct);
            this.Controls.Add(this.lSelectVersion);
            this.Controls.Add(this.lSelectBranch);
            this.Controls.Add(this.cbVersions);
            this.Controls.Add(this.cbBranches);
            this.Controls.Add(this.lSelectProduct);
            this.Controls.Add(this.cbProducts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Manage";
            this.Text = "Manage Special K";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbProducts;
        private System.Windows.Forms.Label lSelectProduct;
        private System.Windows.Forms.ComboBox cbBranches;
        private System.Windows.Forms.ComboBox cbVersions;
        private System.Windows.Forms.Label lSelectBranch;
        private System.Windows.Forms.Label lSelectVersion;
        private System.Windows.Forms.TextBox tbSelectedProduct;
        private System.Windows.Forms.TextBox tbSelectedBranch;
        private System.Windows.Forms.TextBox tbSelectedVersion;
        private System.Windows.Forms.Button bAutomatic;
        private System.Windows.Forms.Button bManual;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar tsProgress;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkReleaseNotes;
        private System.Windows.Forms.CheckBox cbCleanInstall;
        private System.Windows.Forms.Label lCurrentBranch;
        private System.Windows.Forms.Label lCurrentVersion;
    }
}