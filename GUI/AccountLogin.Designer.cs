namespace RemoteHealthCare.GUI
{
    partial class AccountLogin
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.txtAccountNameLogin = new System.Windows.Forms.TextBox();
            this.textPasswordLogin = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCreateAccount = new System.Windows.Forms.Button();
            this.txtLoginInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(12, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(158, 13);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Account name:";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(12, 133);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(158, 13);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "Password:";
            // 
            // txtAccountNameLogin
            // 
            this.txtAccountNameLogin.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtAccountNameLogin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAccountNameLogin.Location = new System.Drawing.Point(12, 64);
            this.txtAccountNameLogin.Name = "txtAccountNameLogin";
            this.txtAccountNameLogin.Size = new System.Drawing.Size(158, 13);
            this.txtAccountNameLogin.TabIndex = 2;
            // 
            // textPasswordLogin
            // 
            this.textPasswordLogin.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.textPasswordLogin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textPasswordLogin.Location = new System.Drawing.Point(12, 152);
            this.textPasswordLogin.Name = "textPasswordLogin";
            this.textPasswordLogin.PasswordChar = '*';
            this.textPasswordLogin.Size = new System.Drawing.Size(158, 13);
            this.textPasswordLogin.TabIndex = 3;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLogin.Location = new System.Drawing.Point(52, 212);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnCreateAccount
            // 
            this.btnCreateAccount.BackColor = System.Drawing.Color.PaleGreen;
            this.btnCreateAccount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreateAccount.Location = new System.Drawing.Point(52, 252);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new System.Drawing.Size(75, 23);
            this.btnCreateAccount.TabIndex = 5;
            this.btnCreateAccount.Text = "Create account";
            this.btnCreateAccount.UseVisualStyleBackColor = false;
            this.btnCreateAccount.Click += new System.EventHandler(this.btnCreateAccount_Click);
            // 
            // txtLoginInfo
            // 
            this.txtLoginInfo.BackColor = System.Drawing.Color.PapayaWhip;
            this.txtLoginInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLoginInfo.Location = new System.Drawing.Point(12, 12);
            this.txtLoginInfo.Name = "txtLoginInfo";
            this.txtLoginInfo.ReadOnly = true;
            this.txtLoginInfo.Size = new System.Drawing.Size(158, 13);
            this.txtLoginInfo.TabIndex = 6;
            // 
            // AccountLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(189, 289);
            this.Controls.Add(this.txtLoginInfo);
            this.Controls.Add(this.btnCreateAccount);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.textPasswordLogin);
            this.Controls.Add(this.txtAccountNameLogin);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "AccountLogin";
            this.Text = "AccountLogin";
            this.Load += new System.EventHandler(this.AccountLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox txtAccountNameLogin;
        private System.Windows.Forms.TextBox textPasswordLogin;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCreateAccount;
        private System.Windows.Forms.TextBox txtLoginInfo;
    }
}