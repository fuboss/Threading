namespace Threading
{
    partial class View
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
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.FolderButton = new System.Windows.Forms.Button();
            this.TreeView = new System.Windows.Forms.TreeView();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.FolderButton);
            this.BottomPanel.Location = new System.Drawing.Point(0, 427);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(389, 31);
            this.BottomPanel.TabIndex = 0;
            // 
            // FolderButton
            // 
            this.FolderButton.Location = new System.Drawing.Point(12, 5);
            this.FolderButton.Name = "FolderButton";
            this.FolderButton.Size = new System.Drawing.Size(363, 23);
            this.FolderButton.TabIndex = 0;
            this.FolderButton.Text = "Choose Folder";
            this.FolderButton.UseVisualStyleBackColor = true;
            this.FolderButton.Click += new System.EventHandler(this.ChooseButton_Click);
            // 
            // TreeView
            // 
            this.TreeView.Location = new System.Drawing.Point(0, -1);
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(389, 427);
            this.TreeView.TabIndex = 1;
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 457);
            this.Controls.Add(this.TreeView);
            this.Controls.Add(this.BottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "View";
            this.Text = "View";
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Button FolderButton;
        private System.Windows.Forms.TreeView TreeView;
    }
}

