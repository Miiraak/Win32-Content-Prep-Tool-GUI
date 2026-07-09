namespace Win32_Content_Prep_Tool_GUI
{
    partial class VerboseForm
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
            richTextBox_verbose = new RichTextBox();
            SuspendLayout();
            // 
            // richTextBox_verbose
            // 
            richTextBox_verbose.Location = new Point(12, 12);
            richTextBox_verbose.Name = "richTextBox_verbose";
            richTextBox_verbose.ReadOnly = true;
            richTextBox_verbose.Size = new Size(776, 426);
            richTextBox_verbose.TabIndex = 0;
            richTextBox_verbose.Text = "";
            // 
            // VerboseForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(richTextBox_verbose);
            Name = "VerboseForm";
            Text = "VerboseForm";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox_verbose;
    }
}