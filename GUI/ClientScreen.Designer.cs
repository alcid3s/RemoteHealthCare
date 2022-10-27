using System.Windows.Forms;

namespace RemoteHealthCare.GUI
{
    partial class ClientScreen
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
            this.components = new System.ComponentModel.Container();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.txtDistanceTravelled = new System.Windows.Forms.TextBox();
            this.txtHeartRate = new System.Windows.Forms.TextBox();
            this.txtElapsedTime = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.lstBikes = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.lstChatView = new System.Windows.Forms.ListView();
            this.ChatCollomn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtChatInput = new System.Windows.Forms.TextBox();
            this.txtInfoBox = new System.Windows.Forms.TextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtSpeed
            // 
            this.txtSpeed.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.txtSpeed.Location = new System.Drawing.Point(251, 84);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.ReadOnly = true;
            this.txtSpeed.Size = new System.Drawing.Size(116, 61);
            this.txtSpeed.TabIndex = 0;
            // 
            // txtDistanceTravelled
            // 
            this.txtDistanceTravelled.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtDistanceTravelled.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDistanceTravelled.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.txtDistanceTravelled.Location = new System.Drawing.Point(237, 224);
            this.txtDistanceTravelled.Name = "txtDistanceTravelled";
            this.txtDistanceTravelled.ReadOnly = true;
            this.txtDistanceTravelled.Size = new System.Drawing.Size(147, 61);
            this.txtDistanceTravelled.TabIndex = 1;
            // 
            // txtHeartRate
            // 
            this.txtHeartRate.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtHeartRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHeartRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.txtHeartRate.Location = new System.Drawing.Point(461, 224);
            this.txtHeartRate.Name = "txtHeartRate";
            this.txtHeartRate.ReadOnly = true;
            this.txtHeartRate.Size = new System.Drawing.Size(113, 61);
            this.txtHeartRate.TabIndex = 3;
            // 
            // txtElapsedTime
            // 
            this.txtElapsedTime.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtElapsedTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtElapsedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.txtElapsedTime.Location = new System.Drawing.Point(448, 84);
            this.txtElapsedTime.Name = "txtElapsedTime";
            this.txtElapsedTime.ReadOnly = true;
            this.txtElapsedTime.Size = new System.Drawing.Size(144, 61);
            this.txtElapsedTime.TabIndex = 2;
            this.txtElapsedTime.TextChanged += new System.EventHandler(this.txtElapsedTime_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.textBox1.Location = new System.Drawing.Point(297, 158);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 14);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "m/s";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.textBox2.Location = new System.Drawing.Point(297, 298);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 14);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Meter";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.textBox3.Location = new System.Drawing.Point(492, 158);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 14);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "Seconds";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.textBox4.Location = new System.Drawing.Point(501, 298);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 14);
            this.textBox4.TabIndex = 7;
            this.textBox4.Text = "BPM";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // lstBikes
            // 
            this.lstBikes.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.lstBikes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.lstBikes.FormattingEnabled = true;
            this.lstBikes.ItemHeight = 15;
            this.lstBikes.Location = new System.Drawing.Point(12, 76);
            this.lstBikes.Name = "lstBikes";
            this.lstBikes.Size = new System.Drawing.Size(120, 169);
            this.lstBikes.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.PaleGreen;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.button1.Location = new System.Drawing.Point(57, 251);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.textBox5.Location = new System.Drawing.Point(12, 57);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 14);
            this.textBox5.TabIndex = 11;
            this.textBox5.Text = "Select bike:";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Red;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBack.Location = new System.Drawing.Point(12, 340);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 12;
            this.btnBack.Text = "Log out";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lstChatView
            // 
            this.lstChatView.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.lstChatView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstChatView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChatCollomn});
            this.lstChatView.FullRowSelect = true;
            this.lstChatView.GridLines = true;
            this.lstChatView.HideSelection = false;
            this.lstChatView.Location = new System.Drawing.Point(641, 57);
            this.lstChatView.Name = "lstChatView";
            this.lstChatView.Size = new System.Drawing.Size(223, 230);
            this.lstChatView.TabIndex = 0;
            this.lstChatView.TileSize = new System.Drawing.Size(1, 1);
            this.lstChatView.UseCompatibleStateImageBehavior = false;
            this.lstChatView.View = System.Windows.Forms.View.Details;
            // 
            // ChatCollomn
            // 
            this.ChatCollomn.Text = "Messages";
            this.ChatCollomn.Width = 338;
            // 
            // txtChatInput
            // 
            this.txtChatInput.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtChatInput.Location = new System.Drawing.Point(641, 293);
            this.txtChatInput.Name = "txtChatInput";
            this.txtChatInput.Size = new System.Drawing.Size(168, 20);
            this.txtChatInput.TabIndex = 1;
            this.txtChatInput.TextChanged += new System.EventHandler(this.txtChatInput_TextChanged_1);
            // 
            // txtInfoBox
            // 
            this.txtInfoBox.BackColor = System.Drawing.Color.PapayaWhip;
            this.txtInfoBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInfoBox.Location = new System.Drawing.Point(214, 381);
            this.txtInfoBox.Name = "txtInfoBox";
            this.txtInfoBox.ReadOnly = true;
            this.txtInfoBox.Size = new System.Drawing.Size(100, 13);
            this.txtInfoBox.TabIndex = 13;
            this.txtInfoBox.Text = "important info:";
            // 
            // txtInfo
            // 
            this.txtInfo.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInfo.Location = new System.Drawing.Point(282, 381);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(451, 20);
            this.txtInfo.TabIndex = 14;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Location = new System.Drawing.Point(214, 57);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(388, 315);
            this.textBox6.TabIndex = 15;
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.textBox7.Location = new System.Drawing.Point(12, 12);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 22);
            this.textBox7.TabIndex = 16;
            this.textBox7.Text = "Menu";
            // 
            // ClientScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(934, 435);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.txtChatInput);
            this.Controls.Add(this.lstChatView);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstBikes);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtHeartRate);
            this.Controls.Add(this.txtElapsedTime);
            this.Controls.Add(this.txtDistanceTravelled);
            this.Controls.Add(this.txtSpeed);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.txtInfoBox);
            this.Controls.Add(this.textBox6);
            this.Name = "ClientScreen";
            this.Text = "ClientScreen";
            this.Load += new System.EventHandler(this.ClientScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.TextBox txtDistanceTravelled;
        private System.Windows.Forms.TextBox txtHeartRate;
        private System.Windows.Forms.TextBox txtElapsedTime;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ListBox lstBikes;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.ListView lstChatView;
        private System.Windows.Forms.TextBox txtChatInput;
        private TextBox txtInfoBox;
        private TextBox txtInfo;
        private ColumnHeader ChatCollomn;
        private TextBox textBox6;
        private TextBox textBox7;
    }
}