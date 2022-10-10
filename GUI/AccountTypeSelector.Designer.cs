namespace RemoteHealthCare.GUI
{
    partial class AccountTypeSelector
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
            this.btnTypeClient = new System.Windows.Forms.Button();
            this.btnTypeDoctor = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTypeClient
            // 
            this.btnTypeClient.Location = new System.Drawing.Point(73, 46);
            this.btnTypeClient.Name = "btnTypeClient";
            this.btnTypeClient.Size = new System.Drawing.Size(137, 46);
            this.btnTypeClient.TabIndex = 0;
            this.btnTypeClient.Text = "Client";
            this.btnTypeClient.UseVisualStyleBackColor = true;
            this.btnTypeClient.Click += new System.EventHandler(this.btnTypeClient_Click);
            // 
            // btnTypeDoctor
            // 
            this.btnTypeDoctor.Location = new System.Drawing.Point(73, 131);
            this.btnTypeDoctor.Name = "btnTypeDoctor";
            this.btnTypeDoctor.Size = new System.Drawing.Size(137, 45);
            this.btnTypeDoctor.TabIndex = 1;
            this.btnTypeDoctor.Text = "Doctor";
            this.btnTypeDoctor.UseVisualStyleBackColor = true;
            this.btnTypeDoctor.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(12, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(64, 25);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // AccountTypeSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 225);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnTypeDoctor);
            this.Controls.Add(this.btnTypeClient);
            this.Name = "AccountTypeSelector";
            this.Text = "AccountTypeSelector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTypeClient;
        private System.Windows.Forms.Button btnTypeDoctor;
        private System.Windows.Forms.Button btnBack;
    }
}