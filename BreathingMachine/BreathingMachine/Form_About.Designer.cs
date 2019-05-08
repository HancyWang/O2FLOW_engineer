namespace BreathingMachine
{
    partial class Form_About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_About));
            this.label_AboutUs_appName = new System.Windows.Forms.Label();
            this.label_AboutUs_appVersion = new System.Windows.Forms.Label();
            this.label_AboutUs_CompanyName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_AboutUs_appName
            // 
            this.label_AboutUs_appName.AutoSize = true;
            this.label_AboutUs_appName.Location = new System.Drawing.Point(179, 44);
            this.label_AboutUs_appName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_AboutUs_appName.Name = "label_AboutUs_appName";
            this.label_AboutUs_appName.Size = new System.Drawing.Size(137, 15);
            this.label_AboutUs_appName.TabIndex = 0;
            this.label_AboutUs_appName.Text = "O2FLO数据读取软件";
            // 
            // label_AboutUs_appVersion
            // 
            this.label_AboutUs_appVersion.AutoSize = true;
            this.label_AboutUs_appVersion.Location = new System.Drawing.Point(186, 88);
            this.label_AboutUs_appVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_AboutUs_appVersion.Name = "label_AboutUs_appVersion";
            this.label_AboutUs_appVersion.Size = new System.Drawing.Size(122, 15);
            this.label_AboutUs_appVersion.TabIndex = 1;
            this.label_AboutUs_appVersion.Text = "软件版本：1.0.1";
            // 
            // label_AboutUs_CompanyName
            // 
            this.label_AboutUs_CompanyName.AutoSize = true;
            this.label_AboutUs_CompanyName.Location = new System.Drawing.Point(104, 132);
            this.label_AboutUs_CompanyName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_AboutUs_CompanyName.Name = "label_AboutUs_CompanyName";
            this.label_AboutUs_CompanyName.Size = new System.Drawing.Size(311, 15);
            this.label_AboutUs_CompanyName.TabIndex = 2;
            this.label_AboutUs_CompanyName.Text = "Vincent Medical Manufacturing Co.,Ltd.";
            // 
            // Form_About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 186);
            this.Controls.Add(this.label_AboutUs_CompanyName);
            this.Controls.Add(this.label_AboutUs_appVersion);
            this.Controls.Add(this.label_AboutUs_appName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于我们";
            this.Load += new System.EventHandler(this.Form_About_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_AboutUs_appName;
        private System.Windows.Forms.Label label_AboutUs_appVersion;
        private System.Windows.Forms.Label label_AboutUs_CompanyName;
    }
}