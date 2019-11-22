namespace Hyperwave.Browser
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.cUrl = new System.Windows.Forms.TextBox();
            this.cBrowserContainer = new System.Windows.Forms.Panel();
            this.cWaiter = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // cUrl
            // 
            this.cUrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.cUrl.Location = new System.Drawing.Point(0, 0);
            this.cUrl.Name = "cUrl";
            this.cUrl.ReadOnly = true;
            this.cUrl.Size = new System.Drawing.Size(284, 20);
            this.cUrl.TabIndex = 0;
            // 
            // cBrowserContainer
            // 
            this.cBrowserContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cBrowserContainer.Location = new System.Drawing.Point(0, 20);
            this.cBrowserContainer.Name = "cBrowserContainer";
            this.cBrowserContainer.Size = new System.Drawing.Size(284, 241);
            this.cBrowserContainer.TabIndex = 1;
            // 
            // cWaiter
            // 
            this.cWaiter.DoWork += new System.ComponentModel.DoWorkEventHandler(this.cWaiter_DoWork);
            this.cWaiter.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.cWaiter_RunWorkerCompleted);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.cBrowserContainer);
            this.Controls.Add(this.cUrl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Sign In:";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox cUrl;
        private System.Windows.Forms.Panel cBrowserContainer;
        private System.ComponentModel.BackgroundWorker cWaiter;
    }
}

