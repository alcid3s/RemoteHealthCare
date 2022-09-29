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
            this.textPasswordAccountCreationClient = new System.Windows.Forms.TextBox();
            this.txtAccountNameAccountCreationClient = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.txtPasswordConfirmAccountCreationClient = new System.Windows.Forms.TextBox();
            this.txtConfirmPaswordAccountCreationClient = new System.Windows.Forms.TextBox();
            this.btnCreateAccountCreationClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textPasswordAccountCreationClient
            // 
            this.textPasswordAccountCreationClient.Location = new System.Drawing.Point(43, 168);
            this.textPasswordAccountCreationClient.Name = "textPasswordAccountCreationClient";
            this.textPasswordAccountCreationClient.Size = new System.Drawing.Size(158, 20);
            this.textPasswordAccountCreationClient.TabIndex = 7;
            this.textPasswordAccountCreationClient.TextChanged += new System.EventHandler(this.textPasswordAccountCreationClient_TextChanged);
            // 
            // txtAccountNameAccountCreationClient
            // 
            this.txtAccountNameAccountCreationClient.Location = new System.Drawing.Point(43, 80);
            this.txtAccountNameAccountCreationClient.Name = "txtAccountNameAccountCreationClient";
            this.txtAccountNameAccountCreationClient.Size = new System.Drawing.Size(158, 20);
            this.txtAccountNameAccountCreationClient.TabIndex = 6;
            this.txtAccountNameAccountCreationClient.TextChanged += new System.EventHandler(this.txtAccountNameAccountCreationClient_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(43, 149);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(158, 13);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Password";
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(43, 61);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(158, 13);
            this.textBox3.TabIndex = 4;
            this.textBox3.Text = "Account name:";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // txtPasswordConfirmAccountCreationClient
            // 
            this.txtPasswordConfirmAccountCreationClient.Location = new System.Drawing.Point(43, 259);
            this.txtPasswordConfirmAccountCreationClient.Name = "txtPasswordConfirmAccountCreationClient";
            this.txtPasswordConfirmAccountCreationClient.Size = new System.Drawing.Size(158, 20);
            this.txtPasswordConfirmAccountCreationClient.TabIndex = 9;
            this.txtPasswordConfirmAccountCreationClient.TextChanged += new System.EventHandler(this.txtPasswordConfirmAccountCreationClient_TextChanged);
            // 
            // txtConfirmPaswordAccountCreationClient
            // 
            this.txtConfirmPaswordAccountCreationClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConfirmPaswordAccountCreationClient.Location = new System.Drawing.Point(43, 240);
            this.txtConfirmPaswordAccountCreationClient.Name = "txtConfirmPaswordAccountCreationClient";
            this.txtConfirmPaswordAccountCreationClient.ReadOnly = true;
            this.txtConfirmPaswordAccountCreationClient.Size = new System.Drawing.Size(158, 13);
            this.txtConfirmPaswordAccountCreationClient.TabIndex = 8;
            this.txtConfirmPaswordAccountCreationClient.Text = "Confirm password";
            // 
            // btnCreateAccountCreationClient
            // 
            this.btnCreateAccountCreationClient.Location = new System.Drawing.Point(82, 338);
            this.btnCreateAccountCreationClient.Name = "btnCreateAccountCreationClient";
            this.btnCreateAccountCreationClient.Size = new System.Drawing.Size(75, 23);
            this.btnCreateAccountCreationClient.TabIndex = 10;
            this.btnCreateAccountCreationClient.Text = "Create";
            this.btnCreateAccountCreationClient.UseVisualStyleBackColor = true;
            this.btnCreateAccountCreationClient.Click += new System.EventHandler(this.btnCreateAccountCreationClient_Click);
            // 
            // ClientAccountCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 432);
            this.Controls.Add(this.btnCreateAccountCreationClient);
            this.Controls.Add(this.txtPasswordConfirmAccountCreationClient);
            this.Controls.Add(this.txtConfirmPaswordAccountCreationClient);
            this.Controls.Add(this.textPasswordAccountCreationClient);
            this.Controls.Add(this.txtAccountNameAccountCreationClient);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Name = "ClientAccountCreation";
            this.Text = "ClientAccountCreation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.TextBox textPasswordAccountCreationClient;
        private System.Windows.Forms.TextBox txtAccountNameAccountCreationClient;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox txtPasswordConfirmAccountCreationClient;
        private System.Windows.Forms.TextBox txtConfirmPaswordAccountCreationClient;
        private System.Windows.Forms.Button btnCreateAccountCreationClient;
    }
}