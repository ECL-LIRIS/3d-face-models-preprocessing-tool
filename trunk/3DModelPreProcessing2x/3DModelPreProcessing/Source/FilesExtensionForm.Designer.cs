namespace ModelPreProcessing
{
    partial class FilesExtensionForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxExtensions = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Supported Files:";
            // 
            // groupBoxExtensions
            // 
            this.groupBoxExtensions.Location = new System.Drawing.Point(12, 25);
            this.groupBoxExtensions.Name = "groupBoxExtensions";
            this.groupBoxExtensions.Size = new System.Drawing.Size(168, 374);
            this.groupBoxExtensions.TabIndex = 1;
            this.groupBoxExtensions.TabStop = false;
            this.groupBoxExtensions.Text = "Extensions:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FilesExtensionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 411);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBoxExtensions);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilesExtensionForm";
            this.ShowIcon = false;
            this.Text = "FilesExtensions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxExtensions;
        private System.Windows.Forms.Button button1;
    }
}