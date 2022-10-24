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
            this.txtChatInput = new System.Windows.Forms.TextBox();
            this.lstChatView = new System.Windows.Forms.ListView();
            this.ChatCollomn = new System.Windows.Forms.ColumnHeader();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Location = new System.Drawing.Point(244, 300);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 16);
            this.textBox5.TabIndex = 12;
            this.textBox5.Text = "BPM";
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(88, 300);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 16);
            this.textBox4.TabIndex = 11;
            this.textBox4.Text = "Meter";
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(234, 158);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 16);
            this.textBox3.TabIndex = 10;
            this.textBox3.Text = "Seconds";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(88, 158);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 16);
            this.textBox2.TabIndex = 9;
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
            this.btnConnectClient.Location = new System.Drawing.Point(683, 311);
            this.btnConnectClient.Name = "btnConnectClient";
            this.btnConnectClient.Size = new System.Drawing.Size(75, 23);
            this.btnConnectClient.TabIndex = 3;
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
            this.lstClients.TabIndex = 2;
            this.lstClients.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // txtET
            // 
            this.txtET.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtET.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtET.Location = new System.Drawing.Point(207, 81);
            this.txtET.Name = "txtET";
            this.txtET.ReadOnly = true;
            this.txtET.Size = new System.Drawing.Size(137, 71);
            this.txtET.TabIndex = 6;
            this.txtET.TextChanged += new System.EventHandler(this.txtET_TextChanged);
            // 
            // txtDT
            // 
            this.txtDT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDT.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDT.Location = new System.Drawing.Point(27, 223);
            this.txtDT.Name = "txtDT";
            this.txtDT.ReadOnly = true;
            this.txtDT.Size = new System.Drawing.Size(141, 71);
            this.txtDT.TabIndex = 7;
            this.txtDT.TextChanged += new System.EventHandler(this.txtDT_TextChanged);
            // 
            // txtHR
            // 
            this.txtHR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHR.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtHR.Location = new System.Drawing.Point(207, 223);
            this.txtHR.Name = "txtHR";
            this.txtHR.ReadOnly = true;
            this.txtHR.Size = new System.Drawing.Size(97, 71);
            this.txtHR.TabIndex = 8;
            this.txtHR.TextChanged += new System.EventHandler(this.txtHR_TextChanged);
            // 
            // txtSpeed
            // 
            this.txtSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSpeed.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSpeed.Location = new System.Drawing.Point(53, 81);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.ReadOnly = true;
            this.txtSpeed.Size = new System.Drawing.Size(115, 71);
            this.txtSpeed.TabIndex = 5;
            this.txtSpeed.TextChanged += new System.EventHandler(this.txtSpeed_TextChanged);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(1, 17);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(100, 23);
            this.textBox6.TabIndex = 13;
            this.textBox6.Text = "important info:";
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(111, 17);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(451, 23);
            this.txtInfo.TabIndex = 14;
            // 
            // btnLoadData
            // 
            this.btnLoadData.BackColor = System.Drawing.Color.Red;
            this.btnLoadData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadData.Location = new System.Drawing.Point(713, 415);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(75, 23);
            this.btnLoadData.TabIndex = 4;
            this.btnLoadData.Text = "Load data";
            this.btnLoadData.UseVisualStyleBackColor = false;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // txtChatInput
            // 
            this.txtChatInput.Location = new System.Drawing.Point(394, 282);
            this.txtChatInput.Name = "txtChatInput";
            this.txtChatInput.PlaceholderText = "Message.";
            this.txtChatInput.Size = new System.Drawing.Size(168, 23);
            this.txtChatInput.TabIndex = 1;
            this.txtChatInput.TextChanged += new System.EventHandler(this.txtChatInput_TextChanged);
            // 
            // lstChatView
            // 
            this.lstChatView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstChatView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChatCollomn});
            this.lstChatView.FullRowSelect = true;
            this.lstChatView.GridLines = true;
            this.lstChatView.Location = new System.Drawing.Point(394, 46);
            this.lstChatView.Name = "lstChatView";
            this.lstChatView.Size = new System.Drawing.Size(238, 230);
            this.lstChatView.TabIndex = 0;
            this.lstChatView.TileSize = new System.Drawing.Size(1, 1);
            this.lstChatView.UseCompatibleStateImageBehavior = false;
            this.lstChatView.View = System.Windows.Forms.View.Details;
            this.lstChatView.SelectedIndexChanged += new System.EventHandler(this.lstChatView_SelectedIndexChanged);
            // 
            // ChatCollomn
            // 
            this.ChatCollomn.Text = "Messages";
            this.ChatCollomn.Width = 338;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Red;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefresh.Location = new System.Drawing.Point(683, 310);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 21;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.Lime;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoad.Location = new System.Drawing.Point(683, 281);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 21;
            this.btnLoad.Text = "Load data";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // DoctorScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lstChatView);
            this.Controls.Add(this.txtChatInput);
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
        private TextBox txtChatBox;
        private TextBox txtChatInput;
        private ListView lstChatView;
        private ColumnHeader ChatCollomn;
        private Button btnRefresh;
        private Button btnLoad;
    }
}