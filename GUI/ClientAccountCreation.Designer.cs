using System;

namespace RemoteHealthCare.GUI
{
    partial class ClientAccountCreation
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
            this.txtPasswordAccountCreationClient = new System.Windows.Forms.TextBox();
            this.txtAccountNameAccountCreationClient = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.txtPasswordConfirmAccountCreationClient = new System.Windows.Forms.TextBox();
            this.txtConfirmPaswordAccountCreationClient = new System.Windows.Forms.TextBox();
            this.btnCreateAccountCreationClient = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtErrorMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtPasswordAccountCreationClient
            // 
            this.txtPasswordAccountCreationClient.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtPasswordAccountCreationClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPasswordAccountCreationClient.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtPasswordAccountCreationClient.Location = new System.Drawing.Point(39, 199);
            this.txtPasswordAccountCreationClient.Name = "txtPasswordAccountCreationClient";
            this.txtPasswordAccountCreationClient.Size = new System.Drawing.Size(158, 13);
            this.txtPasswordAccountCreationClient.TabIndex = 7;
            // 
            // txtAccountNameAccountCreationClient
            // 
            this.txtAccountNameAccountCreationClient.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtAccountNameAccountCreationClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAccountNameAccountCreationClient.Location = new System.Drawing.Point(39, 111);
            this.txtAccountNameAccountCreationClient.Name = "txtAccountNameAccountCreationClient";
            this.txtAccountNameAccountCreationClient.Size = new System.Drawing.Size(158, 13);
            this.txtAccountNameAccountCreationClient.TabIndex = 6;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(39, 180);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(158, 13);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Password";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(39, 92);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(158, 13);
            this.textBox3.TabIndex = 4;
            this.textBox3.Text = "Account name:";
            // 
            // txtPasswordConfirmAccountCreationClient
            // 
            this.txtPasswordConfirmAccountCreationClient.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtPasswordConfirmAccountCreationClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPasswordConfirmAccountCreationClient.Location = new System.Drawing.Point(39, 290);
            this.txtPasswordConfirmAccountCreationClient.Name = "txtPasswordConfirmAccountCreationClient";
            this.txtPasswordConfirmAccountCreationClient.Size = new System.Drawing.Size(158, 13);
            this.txtPasswordConfirmAccountCreationClient.TabIndex = 9;
            // 
            // txtConfirmPaswordAccountCreationClient
            // 
            this.txtConfirmPaswordAccountCreationClient.BackColor = System.Drawing.Color.PapayaWhip;
            this.txtConfirmPaswordAccountCreationClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConfirmPaswordAccountCreationClient.Location = new System.Drawing.Point(39, 271);
            this.txtConfirmPaswordAccountCreationClient.Name = "txtConfirmPaswordAccountCreationClient";
            this.txtConfirmPaswordAccountCreationClient.ReadOnly = true;
            this.txtConfirmPaswordAccountCreationClient.Size = new System.Drawing.Size(158, 13);
            this.txtConfirmPaswordAccountCreationClient.TabIndex = 8;
            this.txtConfirmPaswordAccountCreationClient.Text = "Confirm password";
            // 
            // btnCreateAccountCreationClient
            // 
            this.btnCreateAccountCreationClient.BackColor = System.Drawing.Color.PaleGreen;
            this.btnCreateAccountCreationClient.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreateAccountCreationClient.Location = new System.Drawing.Point(78, 369);
            this.btnCreateAccountCreationClient.Name = "btnCreateAccountCreationClient";
            this.btnCreateAccountCreationClient.Size = new System.Drawing.Size(75, 23);
            this.btnCreateAccountCreationClient.TabIndex = 10;
            this.btnCreateAccountCreationClient.Text = "Create";
            this.btnCreateAccountCreationClient.UseVisualStyleBackColor = false;
            this.btnCreateAccountCreationClient.Click += new System.EventHandler(this.btnCreateAccountCreationClient_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBack.Location = new System.Drawing.Point(12, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 11;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(97, 405);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(140, 15);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "Client account creator v1.0";
            // 
            // txtErrorMsg
            // 
            this.txtErrorMsg.BackColor = System.Drawing.Color.PapayaWhip;
            this.txtErrorMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtErrorMsg.Location = new System.Drawing.Point(12, 42);
            this.txtErrorMsg.Name = "txtErrorMsg";
            this.txtErrorMsg.ReadOnly = true;
            this.txtErrorMsg.Size = new System.Drawing.Size(212, 13);
            this.txtErrorMsg.TabIndex = 13;
            // 
            // ClientAccountCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(236, 423);
            this.Controls.Add(this.txtErrorMsg);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnCreateAccountCreationClient);
            this.Controls.Add(this.txtPasswordConfirmAccountCreationClient);
            this.Controls.Add(this.txtConfirmPaswordAccountCreationClient);
            this.Controls.Add(this.txtPasswordAccountCreationClient);
            this.Controls.Add(this.txtAccountNameAccountCreationClient);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Name = "ClientAccountCreation";
            this.Text = "ClientAccountCreation";
            this.Load += new System.EventHandler(this.ClientAccountCreation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.TextBox txtPasswordAccountCreationClient;
        private System.Windows.Forms.TextBox txtAccountNameAccountCreationClient;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox txtPasswordConfirmAccountCreationClient;
        private System.Windows.Forms.TextBox txtConfirmPaswordAccountCreationClient;
        private System.Windows.Forms.Button btnCreateAccountCreationClient;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtErrorMsg;
    }
}