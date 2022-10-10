namespace DoctorApllication
{
    partial class DoctorScreen
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
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnConnectClient = new System.Windows.Forms.Button();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.txtET = new System.Windows.Forms.TextBox();
            this.txtDT = new System.Windows.Forms.TextBox();
            this.txtHR = new System.Windows.Forms.TextBox();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Location = new System.Drawing.Point(302, 354);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 16);
            this.textBox5.TabIndex = 24;
            this.textBox5.Text = "BPM";
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(146, 354);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 16);
            this.textBox4.TabIndex = 23;
            this.textBox4.Text = "Meter";
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(292, 212);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 16);
            this.textBox3.TabIndex = 22;
            this.textBox3.Text = "Seconds";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(146, 212);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 16);
            this.textBox2.TabIndex = 21;
            this.textBox2.Text = "M/S";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(638, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 16);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = "clients:";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnConnectClient
            // 
            this.btnConnectClient.BackColor = System.Drawing.Color.Red;
            this.btnConnectClient.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnectClient.Location = new System.Drawing.Point(683, 281);
            this.btnConnectClient.Name = "btnConnectClient";
            this.btnConnectClient.Size = new System.Drawing.Size(75, 23);
            this.btnConnectClient.TabIndex = 19;
            this.btnConnectClient.Text = "Connect";
            this.btnConnectClient.UseVisualStyleBackColor = false;
            this.btnConnectClient.Click += new System.EventHandler(this.btnConnectClient_Click);
            // 
            // lstClients
            // 
            this.lstClients.FormattingEnabled = true;
            this.lstClients.ItemHeight = 15;
            this.lstClients.Location = new System.Drawing.Point(638, 46);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(120, 229);
            this.lstClients.TabIndex = 18;
            this.lstClients.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // txtET
            // 
            this.txtET.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtET.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtET.Location = new System.Drawing.Point(265, 135);
            this.txtET.Name = "txtET";
            this.txtET.ReadOnly = true;
            this.txtET.Size = new System.Drawing.Size(137, 71);
            this.txtET.TabIndex = 17;
            this.txtET.TextChanged += new System.EventHandler(this.txtET_TextChanged);
            // 
            // txtDT
            // 
            this.txtDT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDT.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDT.Location = new System.Drawing.Point(85, 277);
            this.txtDT.Name = "txtDT";
            this.txtDT.ReadOnly = true;
            this.txtDT.Size = new System.Drawing.Size(141, 71);
            this.txtDT.TabIndex = 16;
            this.txtDT.TextChanged += new System.EventHandler(this.txtDT_TextChanged);
            // 
            // txtHR
            // 
            this.txtHR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHR.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtHR.Location = new System.Drawing.Point(265, 277);
            this.txtHR.Name = "txtHR";
            this.txtHR.ReadOnly = true;
            this.txtHR.Size = new System.Drawing.Size(97, 71);
            this.txtHR.TabIndex = 15;
            this.txtHR.TextChanged += new System.EventHandler(this.txtHR_TextChanged);
            // 
            // txtSpeed
            // 
            this.txtSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSpeed.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSpeed.Location = new System.Drawing.Point(111, 135);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.ReadOnly = true;
            this.txtSpeed.Size = new System.Drawing.Size(115, 71);
            this.txtSpeed.TabIndex = 14;
            this.txtSpeed.TextChanged += new System.EventHandler(this.txtSpeed_TextChanged);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(1, 17);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(100, 23);
            this.textBox6.TabIndex = 25;
            this.textBox6.Text = "important info:";
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(111, 17);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(451, 23);
            this.txtInfo.TabIndex = 26;
            // 
            // btnLoadData
            // 
            this.btnLoadData.BackColor = System.Drawing.Color.Red;
            this.btnLoadData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadData.Location = new System.Drawing.Point(713, 415);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(75, 23);
            this.btnLoadData.TabIndex = 27;
            this.btnLoadData.Text = "Load data";
            this.btnLoadData.UseVisualStyleBackColor = false;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // DoctorScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLoadData);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnConnectClient);
            this.Controls.Add(this.lstClients);
            this.Controls.Add(this.txtET);
            this.Controls.Add(this.txtDT);
            this.Controls.Add(this.txtHR);
            this.Controls.Add(this.txtSpeed);
            this.Name = "DoctorScreen";
            this.Text = "DoctorScreen";
            this.Load += new System.EventHandler(this.DoctorScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBox5;
        private TextBox textBox4;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button btnConnectClient;
        private ListBox lstClients;
        private TextBox txtET;
        private TextBox txtDT;
        private TextBox txtHR;
        private TextBox txtSpeed;
        private TextBox textBox6;
        private TextBox txtInfo;
        private Button btnLoadData;
    }
}