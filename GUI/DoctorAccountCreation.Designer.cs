namespace RemoteHealthCare.GUI
{
    partial class DoctorAccountCreation
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.txtAccountNameAccountCreationDoctor = new System.Windows.Forms.TextBox();
            this.txtPasswordAccountCreationDoctor = new System.Windows.Forms.TextBox();
            this.txtPasswordConfirmAccountCreationPassword = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(101, 404);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(12, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // txtAccountNameAccountCreationDoctor
            // 
            this.txtAccountNameAccountCreationDoctor.Location = new System.Drawing.Point(87, 139);
            this.txtAccountNameAccountCreationDoctor.Name = "txtAccountNameAccountCreationDoctor";
            this.txtAccountNameAccountCreationDoctor.Size = new System.Drawing.Size(100, 20);
            this.txtAccountNameAccountCreationDoctor.TabIndex = 3;
            // 
            // txtPasswordAccountCreationDoctor
            // 
            this.txtPasswordAccountCreationDoctor.Location = new System.Drawing.Point(87, 220);
            this.txtPasswordAccountCreationDoctor.Name = "txtPasswordAccountCreationDoctor";
            this.txtPasswordAccountCreationDoctor.Size = new System.Drawing.Size(100, 20);
            this.txtPasswordAccountCreationDoctor.TabIndex = 4;
            // 
            // txtPasswordConfirmAccountCreationPassword
            // 
            this.txtPasswordConfirmAccountCreationPassword.Location = new System.Drawing.Point(87, 303);
            this.txtPasswordConfirmAccountCreationPassword.Name = "txtPasswordConfirmAccountCreationPassword";
            this.txtPasswordConfirmAccountCreationPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPasswordConfirmAccountCreationPassword.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(87, 120);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 13);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Account Name:";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(87, 201);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 13);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "Password:";
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(87, 284);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 13);
            this.textBox3.TabIndex = 8;
            this.textBox3.Text = "Password confirm:";
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(131, 456);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(143, 13);
            this.textBox4.TabIndex = 9;
            this.textBox4.Text = "Doctor account creator v1.0";
            // 
            // DoctorAccountCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 481);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtPasswordConfirmAccountCreationPassword);
            this.Controls.Add(this.txtPasswordAccountCreationDoctor);
            this.Controls.Add(this.txtAccountNameAccountCreationDoctor);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnCreate);
            this.Name = "DoctorAccountCreation";
            this.Text = "DoctorAccountCreation";
            this.Load += new System.EventHandler(this.DoctorAccountCreation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TextBox txtAccountNameAccountCreationDoctor;
        private System.Windows.Forms.TextBox txtPasswordAccountCreationDoctor;
        private System.Windows.Forms.TextBox txtPasswordConfirmAccountCreationPassword;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
    }
}