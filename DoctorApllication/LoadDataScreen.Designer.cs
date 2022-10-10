namespace DoctorApllication
{
    partial class LoadDataScreen
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
            this.lstAccounts = new System.Windows.Forms.ListBox();
            this.lstSessions = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstAccounts
            // 
            this.lstAccounts.FormattingEnabled = true;
            this.lstAccounts.ItemHeight = 15;
            this.lstAccounts.Location = new System.Drawing.Point(39, 47);
            this.lstAccounts.Name = "lstAccounts";
            this.lstAccounts.Size = new System.Drawing.Size(154, 334);
            this.lstAccounts.TabIndex = 0;
            // 
            // lstSessions
            // 
            this.lstSessions.FormattingEnabled = true;
            this.lstSessions.ItemHeight = 15;
            this.lstSessions.Location = new System.Drawing.Point(208, 47);
            this.lstSessions.Name = "lstSessions";
            this.lstSessions.Size = new System.Drawing.Size(209, 334);
            this.lstSessions.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(39, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 16);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Accounts:";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(208, 25);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 16);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "Sessions: ";
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.Red;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoad.Location = new System.Drawing.Point(423, 358);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // LoadDataScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 418);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lstSessions);
            this.Controls.Add(this.lstAccounts);
            this.Name = "LoadDataScreen";
            this.Text = "LoadDataScreen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox lstAccounts;
        private ListBox lstSessions;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button btnLoad;
    }
}