namespace ItemEditor.Forms
{
    partial class frmSMC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSMC));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSaveSMC = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.codeEditor1 = new ItemEditor.Controls.CodeEditor();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnSaveSMC);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(46, 400);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(551, 29);
            this.panel2.TabIndex = 79;
            // 
            // btnSaveSMC
            // 
            this.btnSaveSMC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSaveSMC.FlatAppearance.BorderSize = 0;
            this.btnSaveSMC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSMC.Image = global::ItemEditor.Properties.Resources.icon_save_to_file;
            this.btnSaveSMC.Location = new System.Drawing.Point(7, 5);
            this.btnSaveSMC.Name = "btnSaveSMC";
            this.btnSaveSMC.Size = new System.Drawing.Size(24, 21);
            this.btnSaveSMC.TabIndex = 70;
            this.btnSaveSMC.UseVisualStyleBackColor = true;
            this.btnSaveSMC.Click += new System.EventHandler(this.btnSaveSMC_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(526, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 48;
            this.pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.panel1.Location = new System.Drawing.Point(-1, 400);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(48, 29);
            this.panel1.TabIndex = 49;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.panel3.Location = new System.Drawing.Point(-1, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(48, 20);
            this.panel3.TabIndex = 80;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Location = new System.Drawing.Point(46, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(551, 20);
            this.panel4.TabIndex = 81;
            // 
            // codeEditor1
            // 
            this.codeEditor1.IEVersion = "10";
            this.codeEditor1.IsWebBrowserContextMenuEnabled = false;
            this.codeEditor1.Location = new System.Drawing.Point(0, 19);
            this.codeEditor1.MinimumSize = new System.Drawing.Size(20, 20);
            this.codeEditor1.Name = "codeEditor1";
            this.codeEditor1.ScrollBarsEnabled = false;
            this.codeEditor1.Size = new System.Drawing.Size(596, 381);
            this.codeEditor1.TabIndex = 80;
            this.codeEditor1.WebBrowserShortcutsEnabled = false;
            // 
            // frmSMC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 429);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.codeEditor1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "frmSMC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMC Viewer";
            this.Load += new System.EventHandler(this.frmSMC_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private Controls.CodeEditor codeEditor1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSaveSMC;
    }
}