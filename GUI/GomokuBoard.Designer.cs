
namespace gomoku.GUI
{
    partial class GomokuBoard
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
            this.gameStatus = new System.Windows.Forms.StatusStrip();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.gameStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.gameStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameStatus
            // 
            this.gameStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameStatusLabel});
            this.gameStatus.Location = new System.Drawing.Point(0, 195);
            this.gameStatus.Name = "gameStatus";
            this.gameStatus.Size = new System.Drawing.Size(604, 22);
            this.gameStatus.TabIndex = 0;
            this.gameStatus.Text = "statusStrip1";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPanel.Location = new System.Drawing.Point(0, 0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(604, 195);
            this.buttonPanel.TabIndex = 1;
            // 
            // gameStatusLabel
            // 
            this.gameStatusLabel.Name = "gameStatusLabel";
            this.gameStatusLabel.Size = new System.Drawing.Size(0, 17);
            this.gameStatusLabel.Click += new System.EventHandler(this.gameStatusLabel_Click);
            // 
            // GomokuBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 217);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.gameStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "GomokuBoard";
            this.Text = "GomokuBoard";
            this.Load += new System.EventHandler(this.GomokuBoard_Load);
            this.gameStatus.ResumeLayout(false);
            this.gameStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip gameStatus;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.ToolStripStatusLabel gameStatusLabel;
    }
}