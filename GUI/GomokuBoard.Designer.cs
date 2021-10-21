
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
            this.gameStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.closeAfterCheck = new System.Windows.Forms.CheckBox();
            this.gameStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameStatus
            // 
            this.gameStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameStatusLabel,
            this.toolStripStatusLabel1});
            this.gameStatus.Location = new System.Drawing.Point(0, 48);
            this.gameStatus.Name = "gameStatus";
            this.gameStatus.Size = new System.Drawing.Size(214, 22);
            this.gameStatus.TabIndex = 0;
            this.gameStatus.Text = "statusStrip1";
            // 
            // gameStatusLabel
            // 
            this.gameStatusLabel.Name = "gameStatusLabel";
            this.gameStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPanel.Location = new System.Drawing.Point(0, 0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(214, 27);
            this.buttonPanel.TabIndex = 1;
            // 
            // closeAfterCheck
            // 
            this.closeAfterCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.closeAfterCheck.AutoSize = true;
            this.closeAfterCheck.Location = new System.Drawing.Point(4, 30);
            this.closeAfterCheck.Name = "closeAfterCheck";
            this.closeAfterCheck.Size = new System.Drawing.Size(105, 17);
            this.closeAfterCheck.TabIndex = 2;
            this.closeAfterCheck.Text = "Close after game";
            this.closeAfterCheck.UseVisualStyleBackColor = true;
            this.closeAfterCheck.CheckedChanged += new System.EventHandler(this.closeAfterCheck_CheckedChanged);
            // 
            // GomokuBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 70);
            this.Controls.Add(this.closeAfterCheck);
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
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox closeAfterCheck;
    }
}