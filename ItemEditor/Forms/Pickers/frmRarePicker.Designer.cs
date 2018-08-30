namespace ItemEditor.Forms.Pickers
{
    partial class frmRarePicker
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRarePicker));
            this.lbRares = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbInfo2 = new System.Windows.Forms.Label();
            this.lbInfo1 = new System.Windows.Forms.Label();
            this.dgOptions = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnPick = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOptions)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lbRares
            // 
            this.lbRares.FormattingEnabled = true;
            this.lbRares.Location = new System.Drawing.Point(12, 12);
            this.lbRares.Name = "lbRares";
            this.lbRares.Size = new System.Drawing.Size(222, 355);
            this.lbRares.TabIndex = 0;
            this.lbRares.SelectedIndexChanged += new System.EventHandler(this.lbRares_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbInfo2);
            this.groupBox1.Controls.Add(this.lbInfo1);
            this.groupBox1.Location = new System.Drawing.Point(240, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 91);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informations";
            // 
            // lbInfo2
            // 
            this.lbInfo2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbInfo2.Location = new System.Drawing.Point(3, 45);
            this.lbInfo2.Name = "lbInfo2";
            this.lbInfo2.Size = new System.Drawing.Size(526, 29);
            this.lbInfo2.TabIndex = 2;
            this.lbInfo2.Text = "Grade: {0} | Type: {1}";
            this.lbInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbInfo1
            // 
            this.lbInfo1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbInfo1.Location = new System.Drawing.Point(3, 16);
            this.lbInfo1.Name = "lbInfo1";
            this.lbInfo1.Size = new System.Drawing.Size(526, 29);
            this.lbInfo1.TabIndex = 1;
            this.lbInfo1.Text = "Attack: {0} | Defense: {1} | Magic: {2} | Resist: {3}";
            this.lbInfo1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgOptions
            // 
            this.dgOptions.AllowUserToAddRows = false;
            this.dgOptions.AllowUserToDeleteRows = false;
            this.dgOptions.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgOptions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgOptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.dataGridViewImageColumn2,
            this.Value,
            this.Column1});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgOptions.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgOptions.Enabled = false;
            this.dgOptions.EnableHeadersVisualStyles = false;
            this.dgOptions.Location = new System.Drawing.Point(240, 109);
            this.dgOptions.Name = "dgOptions";
            this.dgOptions.ReadOnly = true;
            this.dgOptions.RowHeadersVisible = false;
            this.dgOptions.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgOptions.RowTemplate.Height = 25;
            this.dgOptions.RowTemplate.ReadOnly = true;
            this.dgOptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgOptions.Size = new System.Drawing.Size(532, 258);
            this.dgOptions.TabIndex = 42;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.BurlyWood;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnPick);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(12, 373);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(760, 31);
            this.panel2.TabIndex = 79;
            // 
            // btnPick
            // 
            this.btnPick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPick.Location = new System.Drawing.Point(-1, -1);
            this.btnPick.Name = "btnPick";
            this.btnPick.Size = new System.Drawing.Size(222, 32);
            this.btnPick.TabIndex = 49;
            this.btnPick.Text = "PICK CURRENT";
            this.btnPick.UseVisualStyleBackColor = true;
            this.btnPick.Click += new System.EventHandler(this.btnPick_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(732, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 48;
            this.pictureBox2.TabStop = false;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 280;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "Level";
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewImageColumn2.Width = 60;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Chance";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 90;
            // 
            // frmRarePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 417);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dgOptions);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbRares);
            this.Name = "frmRarePicker";
            this.Text = "frmRarePicker";
            this.Load += new System.EventHandler(this.frmRarePicker_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgOptions)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbRares;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbInfo1;
        private System.Windows.Forms.Label lbInfo2;
        private System.Windows.Forms.DataGridView dgOptions;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnPick;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}