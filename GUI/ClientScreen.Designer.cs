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
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.txtDistanceTravelled = new System.Windows.Forms.TextBox();
            this.txtHeartRate = new System.Windows.Forms.TextBox();
            this.txtElapsedTime = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(161, 107);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.ReadOnly = true;
            this.txtSpeed.Size = new System.Drawing.Size(100, 20);
            this.txtSpeed.TabIndex = 0;
            this.txtSpeed.TextChanged += new System.EventHandler(this.txtSpeed_TextChanged);
            // 
            // txtDistanceTravelled
            // 
            this.txtDistanceTravelled.Location = new System.Drawing.Point(161, 247);
            this.txtDistanceTravelled.Name = "txtDistanceTravelled";
            this.txtDistanceTravelled.ReadOnly = true;
            this.txtDistanceTravelled.Size = new System.Drawing.Size(100, 20);
            this.txtDistanceTravelled.TabIndex = 1;
            this.txtDistanceTravelled.TextChanged += new System.EventHandler(this.txtDistanceTravelled_TextChanged);
            // 
            // txtHeartRate
            // 
            this.txtHeartRate.Location = new System.Drawing.Point(427, 247);
            this.txtHeartRate.Name = "txtHeartRate";
            this.txtHeartRate.ReadOnly = true;
            this.txtHeartRate.Size = new System.Drawing.Size(100, 20);
            this.txtHeartRate.TabIndex = 3;
            this.txtHeartRate.TextChanged += new System.EventHandler(this.txtHeartRate_TextChanged);
            // 
            // txtElapsedTime
            // 
            this.txtElapsedTime.Location = new System.Drawing.Point(427, 107);
            this.txtElapsedTime.Name = "txtElapsedTime";
            this.txtElapsedTime.ReadOnly = true;
            this.txtElapsedTime.Size = new System.Drawing.Size(100, 20);
            this.txtElapsedTime.TabIndex = 2;
            this.txtElapsedTime.TextChanged += new System.EventHandler(this.txtElapsedTime_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(267, 110);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 13);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "m/s";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(267, 250);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 13);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Meter";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(533, 110);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 13);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "Seconds";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(533, 250);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 13);
            this.textBox4.TabIndex = 7;
            this.textBox4.Text = "BPM";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // ClientScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtHeartRate);
            this.Controls.Add(this.txtElapsedTime);
            this.Controls.Add(this.txtDistanceTravelled);
            this.Controls.Add(this.txtSpeed);
            this.Name = "ClientScreen";
            this.Text = "ClientScreen";
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
    }
}