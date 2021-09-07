namespace LinkRepository
{
    partial class PreferencesEditorForm
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
            this.LinkTableFontSizeBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ThumbnailMaxHeightBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.LinkTableFontSizeBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailMaxHeightBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LinkTableFontSizeBox
            // 
            this.LinkTableFontSizeBox.Location = new System.Drawing.Point(136, 7);
            this.LinkTableFontSizeBox.Name = "LinkTableFontSizeBox";
            this.LinkTableFontSizeBox.Size = new System.Drawing.Size(76, 20);
            this.LinkTableFontSizeBox.TabIndex = 0;
            this.LinkTableFontSizeBox.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.LinkTableFontSizeBox.ValueChanged += new System.EventHandler(this.LinkTableFontSizeBox_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Table font size:";
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(15, 168);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(197, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save and exit";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Thumbnail max height:";
            // 
            // ThumbnailMaxHeightBox
            // 
            this.ThumbnailMaxHeightBox.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ThumbnailMaxHeightBox.Location = new System.Drawing.Point(136, 33);
            this.ThumbnailMaxHeightBox.Maximum = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            this.ThumbnailMaxHeightBox.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.ThumbnailMaxHeightBox.Name = "ThumbnailMaxHeightBox";
            this.ThumbnailMaxHeightBox.Size = new System.Drawing.Size(76, 20);
            this.ThumbnailMaxHeightBox.TabIndex = 3;
            this.ThumbnailMaxHeightBox.Value = new decimal(new int[] {
            320,
            0,
            0,
            0});
            // 
            // PreferencesEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 203);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ThumbnailMaxHeightBox);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LinkTableFontSizeBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PreferencesEditorForm";
            this.Text = "Preferences";
            ((System.ComponentModel.ISupportInitialize)(this.LinkTableFontSizeBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailMaxHeightBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown LinkTableFontSizeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ThumbnailMaxHeightBox;
    }
}