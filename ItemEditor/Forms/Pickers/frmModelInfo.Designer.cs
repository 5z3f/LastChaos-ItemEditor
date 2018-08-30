namespace ItemEditor.Forms.Pickers
{
    partial class frmModelInfo
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
            this.gbInformations = new System.Windows.Forms.GroupBox();
            this.lbHeight = new System.Windows.Forms.Label();
            this.lbWidth = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbFormat = new System.Windows.Forms.Label();
            this.gbFlag = new System.Windows.Forms.GroupBox();
            this.tbFlag = new System.Windows.Forms.TextBox();
            this.clbTextureFlag = new System.Windows.Forms.CheckedListBox();
            this.pbTexture = new System.Windows.Forms.PictureBox();
            this.gbInformations.SuspendLayout();
            this.gbFlag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTexture)).BeginInit();
            this.SuspendLayout();
            // 
            // gbInformations
            // 
            this.gbInformations.Controls.Add(this.lbHeight);
            this.gbInformations.Controls.Add(this.lbWidth);
            this.gbInformations.Controls.Add(this.lbVersion);
            this.gbInformations.Controls.Add(this.lbFormat);
            this.gbInformations.Location = new System.Drawing.Point(567, 12);
            this.gbInformations.Name = "gbInformations";
            this.gbInformations.Size = new System.Drawing.Size(221, 71);
            this.gbInformations.TabIndex = 26;
            this.gbInformations.TabStop = false;
            this.gbInformations.Text = "Texture Informations";
            // 
            // lbHeight
            // 
            this.lbHeight.AutoSize = true;
            this.lbHeight.Location = new System.Drawing.Point(143, 44);
            this.lbHeight.Name = "lbHeight";
            this.lbHeight.Size = new System.Drawing.Size(72, 13);
            this.lbHeight.TabIndex = 3;
            this.lbHeight.Text = "Height: XXXX";
            // 
            // lbWidth
            // 
            this.lbWidth.AutoSize = true;
            this.lbWidth.Location = new System.Drawing.Point(146, 25);
            this.lbWidth.Name = "lbWidth";
            this.lbWidth.Size = new System.Drawing.Size(69, 13);
            this.lbWidth.TabIndex = 2;
            this.lbWidth.Text = "Width: XXXX";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(11, 44);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(76, 13);
            this.lbVersion.TabIndex = 1;
            this.lbVersion.Text = "Version: XXXX";
            // 
            // lbFormat
            // 
            this.lbFormat.AutoSize = true;
            this.lbFormat.Location = new System.Drawing.Point(13, 25);
            this.lbFormat.Name = "lbFormat";
            this.lbFormat.Size = new System.Drawing.Size(73, 13);
            this.lbFormat.TabIndex = 0;
            this.lbFormat.Text = "Format: XXXX";
            // 
            // gbFlag
            // 
            this.gbFlag.Controls.Add(this.tbFlag);
            this.gbFlag.Controls.Add(this.clbTextureFlag);
            this.gbFlag.Location = new System.Drawing.Point(567, 89);
            this.gbFlag.Name = "gbFlag";
            this.gbFlag.Size = new System.Drawing.Size(221, 347);
            this.gbFlag.TabIndex = 25;
            this.gbFlag.TabStop = false;
            this.gbFlag.Text = "Texture Flags";
            // 
            // tbFlag
            // 
            this.tbFlag.BackColor = System.Drawing.SystemColors.Control;
            this.tbFlag.Enabled = false;
            this.tbFlag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tbFlag.Location = new System.Drawing.Point(1, 321);
            this.tbFlag.Multiline = true;
            this.tbFlag.Name = "tbFlag";
            this.tbFlag.Size = new System.Drawing.Size(220, 26);
            this.tbFlag.TabIndex = 23;
            this.tbFlag.Text = "2";
            this.tbFlag.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // clbTextureFlag
            // 
            this.clbTextureFlag.BackColor = System.Drawing.SystemColors.Control;
            this.clbTextureFlag.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbTextureFlag.Enabled = false;
            this.clbTextureFlag.FormattingEnabled = true;
            this.clbTextureFlag.Items.AddRange(new object[] {
            "TEX_ALPHACHANNEL",
            "TEX_32BIT",
            "TEX_COMPRESSED",
            "TEX_TRANSPARENT",
            "TEX_EQUALIZED",
            "TEX_COMPRESSEDALPHA",
            "TEX_STATIC",
            "TEX_CONSTANT",
            "TEX_GRAY",
            "TEX_COMPRESS",
            "TEX_COMPRESSALPHA",
            "TEX_SINGLEMIPMAP",
            "TEX_PROBED",
            "TEX_DISPOSED",
            "TEX_DITHERED",
            "TEX_FILTERED",
            "TEX_COLORIZED",
            "TEX_WASOLD"});
            this.clbTextureFlag.Location = new System.Drawing.Point(6, 19);
            this.clbTextureFlag.Name = "clbTextureFlag";
            this.clbTextureFlag.Size = new System.Drawing.Size(209, 270);
            this.clbTextureFlag.TabIndex = 22;
            // 
            // pbTexture
            // 
            this.pbTexture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTexture.Location = new System.Drawing.Point(12, 18);
            this.pbTexture.Name = "pbTexture";
            this.pbTexture.Size = new System.Drawing.Size(549, 420);
            this.pbTexture.TabIndex = 27;
            this.pbTexture.TabStop = false;
            // 
            // frmModelInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbTexture);
            this.Controls.Add(this.gbInformations);
            this.Controls.Add(this.gbFlag);
            this.Name = "frmModelInfo";
            this.Text = "frmModelInformations";
            this.Load += new System.EventHandler(this.frmModelInfo_Load);
            this.gbInformations.ResumeLayout(false);
            this.gbInformations.PerformLayout();
            this.gbFlag.ResumeLayout(false);
            this.gbFlag.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTexture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbInformations;
        private System.Windows.Forms.Label lbHeight;
        private System.Windows.Forms.Label lbWidth;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label lbFormat;
        private System.Windows.Forms.GroupBox gbFlag;
        private System.Windows.Forms.TextBox tbFlag;
        private System.Windows.Forms.CheckedListBox clbTextureFlag;
        private System.Windows.Forms.PictureBox pbTexture;
    }
}