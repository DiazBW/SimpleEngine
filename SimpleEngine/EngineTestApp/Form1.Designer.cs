namespace EngineTestApp
{
    partial class MainForm
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
            this.gbWatch = new System.Windows.Forms.GroupBox();
            this.boardPanel = new System.Windows.Forms.Panel();
            this.gbControls = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // gbWatch
            // 
            this.gbWatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbWatch.Location = new System.Drawing.Point(12, 12);
            this.gbWatch.Name = "gbWatch";
            this.gbWatch.Size = new System.Drawing.Size(200, 754);
            this.gbWatch.TabIndex = 0;
            this.gbWatch.TabStop = false;
            this.gbWatch.Text = "gbWatch";
            // 
            // boardPanel
            // 
            this.boardPanel.Location = new System.Drawing.Point(218, 12);
            this.boardPanel.Name = "boardPanel";
            this.boardPanel.Size = new System.Drawing.Size(755, 754);
            this.boardPanel.TabIndex = 1;
            // 
            // gbControls
            // 
            this.gbControls.Location = new System.Drawing.Point(979, 12);
            this.gbControls.Name = "gbControls";
            this.gbControls.Size = new System.Drawing.Size(200, 566);
            this.gbControls.TabIndex = 2;
            this.gbControls.TabStop = false;
            this.gbControls.Text = "gbControls";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 778);
            this.Controls.Add(this.gbControls);
            this.Controls.Add(this.boardPanel);
            this.Controls.Add(this.gbWatch);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbWatch;
        private System.Windows.Forms.Panel boardPanel;
        private System.Windows.Forms.GroupBox gbControls;
    }
}

