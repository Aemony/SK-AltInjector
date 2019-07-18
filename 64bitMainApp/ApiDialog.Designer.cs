namespace AltInjector
{
    partial class ApiDialog
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
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.rbDXGI = new System.Windows.Forms.RadioButton();
            this.rbD3D9 = new System.Windows.Forms.RadioButton();
            this.rbOpenGL32 = new System.Windows.Forms.RadioButton();
            this.rbDInput8 = new System.Windows.Forms.RadioButton();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.rbD3D11 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // ButtonOK
            // 
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(252, 217);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(130, 40);
            this.ButtonOK.TabIndex = 4;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ClickedOK);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(398, 217);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(130, 40);
            this.ButtonCancel.TabIndex = 5;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ClickedCancel);
            // 
            // rbDXGI
            // 
            this.rbDXGI.AutoSize = true;
            this.rbDXGI.Location = new System.Drawing.Point(94, 95);
            this.rbDXGI.Name = "rbDXGI";
            this.rbDXGI.Size = new System.Drawing.Size(180, 24);
            this.rbDXGI.TabIndex = 0;
            this.rbDXGI.TabStop = true;
            this.rbDXGI.Text = "Direct3D 11 (dxgi.dll)";
            this.rbDXGI.UseVisualStyleBackColor = true;
            // 
            // rbD3D9
            // 
            this.rbD3D9.AutoSize = true;
            this.rbD3D9.Location = new System.Drawing.Point(94, 175);
            this.rbD3D9.Name = "rbD3D9";
            this.rbD3D9.Size = new System.Drawing.Size(110, 24);
            this.rbD3D9.TabIndex = 2;
            this.rbD3D9.TabStop = true;
            this.rbD3D9.Text = "Direct3D 9";
            this.rbD3D9.UseVisualStyleBackColor = true;
            // 
            // rbOpenGL32
            // 
            this.rbOpenGL32.AutoSize = true;
            this.rbOpenGL32.Location = new System.Drawing.Point(316, 95);
            this.rbOpenGL32.Name = "rbOpenGL32";
            this.rbOpenGL32.Size = new System.Drawing.Size(95, 24);
            this.rbOpenGL32.TabIndex = 1;
            this.rbOpenGL32.TabStop = true;
            this.rbOpenGL32.Text = "OpenGL";
            this.rbOpenGL32.UseVisualStyleBackColor = true;
            // 
            // rbDInput8
            // 
            this.rbDInput8.AutoSize = true;
            this.rbDInput8.Location = new System.Drawing.Point(316, 135);
            this.rbDInput8.Name = "rbDInput8";
            this.rbDInput8.Size = new System.Drawing.Size(126, 24);
            this.rbDInput8.TabIndex = 3;
            this.rbDInput8.TabStop = true;
            this.rbDInput8.Text = "DirectInput 8";
            this.rbDInput8.UseVisualStyleBackColor = true;
            // 
            // richTextBox
            // 
            this.richTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox.Location = new System.Drawing.Point(12, 12);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.Size = new System.Drawing.Size(516, 63);
            this.richTextBox.TabIndex = 7;
            this.richTextBox.Text = "Please select the appropriate API the game is using. Please consult the PCGamingW" +
    "iki article of the game for assistance if you are uncertain: http://www.pcgaming" +
    "wiki.com/";
            this.richTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.ClickedLink);
            // 
            // rbD3D11
            // 
            this.rbD3D11.AutoSize = true;
            this.rbD3D11.Location = new System.Drawing.Point(94, 135);
            this.rbD3D11.Name = "rbD3D11";
            this.rbD3D11.Size = new System.Drawing.Size(197, 24);
            this.rbD3D11.TabIndex = 8;
            this.rbD3D11.TabStop = true;
            this.rbD3D11.Text = "Direct3D 11 (d3d11.dll)";
            this.rbD3D11.UseVisualStyleBackColor = true;
            // 
            // ApiDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(544, 269);
            this.Controls.Add(this.rbD3D11);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.rbDInput8);
            this.Controls.Add(this.rbOpenGL32);
            this.Controls.Add(this.rbD3D9);
            this.Controls.Add(this.rbDXGI);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "ApiDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select API to use as injection point";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.RadioButton rbDXGI;
        private System.Windows.Forms.RadioButton rbD3D9;
        private System.Windows.Forms.RadioButton rbOpenGL32;
        private System.Windows.Forms.RadioButton rbDInput8;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.RadioButton rbD3D11;
    }
}