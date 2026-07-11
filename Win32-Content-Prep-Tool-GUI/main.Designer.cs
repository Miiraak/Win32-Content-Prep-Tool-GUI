namespace Win32_Content_Prep_Tool_GUI
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            button_select_file = new Button();
            textBox_exec_path = new TextBox();
            label_win32Exec = new Label();
            button_convert = new Button();
            checkBox_bundeled_catalog = new CheckBox();
            label_catalog = new Label();
            textBox_catalog_folder = new TextBox();
            button_select_catalog = new Button();
            label_outputfolder = new Label();
            textBox_output_path = new TextBox();
            buttonOutputFolder = new Button();
            button_copy_args = new Button();
            checkBox_verbose = new CheckBox();
            linkLabel_repo = new LinkLabel();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem_options = new ToolStripMenuItem();
            runAsAdministratorToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem_help = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button_select_file
            // 
            button_select_file.Location = new Point(434, 55);
            button_select_file.Name = "button_select_file";
            button_select_file.Size = new Size(35, 36);
            button_select_file.TabIndex = 1;
            button_select_file.Text = "...";
            button_select_file.UseVisualStyleBackColor = true;
            button_select_file.Click += Button_select_file_Click;
            // 
            // textBox_exec_path
            // 
            textBox_exec_path.Location = new Point(12, 60);
            textBox_exec_path.Name = "textBox_exec_path";
            textBox_exec_path.ReadOnly = true;
            textBox_exec_path.Size = new Size(416, 27);
            textBox_exec_path.TabIndex = 2;
            // 
            // label_win32Exec
            // 
            label_win32Exec.AutoSize = true;
            label_win32Exec.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label_win32Exec.Location = new Point(12, 37);
            label_win32Exec.Name = "label_win32Exec";
            label_win32Exec.Size = new Size(142, 20);
            label_win32Exec.TabIndex = 3;
            label_win32Exec.Text = "Win32 executable :";
            // 
            // button_convert
            // 
            button_convert.Location = new Point(292, 246);
            button_convert.Name = "button_convert";
            button_convert.Size = new Size(177, 29);
            button_convert.TabIndex = 4;
            button_convert.Text = "Convert to .Intunewin";
            button_convert.UseVisualStyleBackColor = true;
            button_convert.Click += Button_convert_Click;
            // 
            // checkBox_bundeled_catalog
            // 
            checkBox_bundeled_catalog.AutoSize = true;
            checkBox_bundeled_catalog.Location = new Point(12, 199);
            checkBox_bundeled_catalog.Name = "checkBox_bundeled_catalog";
            checkBox_bundeled_catalog.Size = new Size(263, 24);
            checkBox_bundeled_catalog.TabIndex = 5;
            checkBox_bundeled_catalog.Text = "Bundel catalog files into .intunewin";
            checkBox_bundeled_catalog.UseVisualStyleBackColor = true;
            checkBox_bundeled_catalog.CheckedChanged += CheckBox_bundeled_catalog_CheckedChanged;
            // 
            // label_catalog
            // 
            label_catalog.AutoSize = true;
            label_catalog.Enabled = false;
            label_catalog.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label_catalog.Location = new Point(12, 144);
            label_catalog.Name = "label_catalog";
            label_catalog.Size = new Size(116, 20);
            label_catalog.TabIndex = 6;
            label_catalog.Text = "Catalog folder :";
            // 
            // textBox_catalog_folder
            // 
            textBox_catalog_folder.Location = new Point(12, 166);
            textBox_catalog_folder.Name = "textBox_catalog_folder";
            textBox_catalog_folder.ReadOnly = true;
            textBox_catalog_folder.Size = new Size(416, 27);
            textBox_catalog_folder.TabIndex = 7;
            // 
            // button_select_catalog
            // 
            button_select_catalog.Enabled = false;
            button_select_catalog.Location = new Point(434, 161);
            button_select_catalog.Name = "button_select_catalog";
            button_select_catalog.Size = new Size(35, 36);
            button_select_catalog.TabIndex = 8;
            button_select_catalog.Text = "...";
            button_select_catalog.UseVisualStyleBackColor = true;
            button_select_catalog.Click += Button_select_catalog_Click;
            // 
            // label_outputfolder
            // 
            label_outputfolder.AutoSize = true;
            label_outputfolder.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label_outputfolder.Location = new Point(12, 90);
            label_outputfolder.Name = "label_outputfolder";
            label_outputfolder.Size = new Size(113, 20);
            label_outputfolder.TabIndex = 11;
            label_outputfolder.Text = "Output folder :";
            // 
            // textBox_output_path
            // 
            textBox_output_path.Location = new Point(12, 112);
            textBox_output_path.Name = "textBox_output_path";
            textBox_output_path.ReadOnly = true;
            textBox_output_path.Size = new Size(416, 27);
            textBox_output_path.TabIndex = 10;
            // 
            // buttonOutputFolder
            // 
            buttonOutputFolder.Location = new Point(434, 108);
            buttonOutputFolder.Name = "buttonOutputFolder";
            buttonOutputFolder.Size = new Size(35, 36);
            buttonOutputFolder.TabIndex = 9;
            buttonOutputFolder.Text = "...";
            buttonOutputFolder.UseVisualStyleBackColor = true;
            buttonOutputFolder.Click += ButtonOutputFolder_Click;
            // 
            // button_copy_args
            // 
            button_copy_args.Location = new Point(343, 211);
            button_copy_args.Name = "button_copy_args";
            button_copy_args.Size = new Size(126, 29);
            button_copy_args.TabIndex = 14;
            button_copy_args.Text = "Copy args";
            button_copy_args.UseVisualStyleBackColor = true;
            button_copy_args.Click += Button_copy_commandline_Click;
            // 
            // checkBox_verbose
            // 
            checkBox_verbose.AutoSize = true;
            checkBox_verbose.Location = new Point(12, 229);
            checkBox_verbose.Name = "checkBox_verbose";
            checkBox_verbose.Size = new Size(84, 24);
            checkBox_verbose.TabIndex = 15;
            checkBox_verbose.Text = "Verbose";
            checkBox_verbose.UseVisualStyleBackColor = true;
            // 
            // linkLabel_repo
            // 
            linkLabel_repo.AutoSize = true;
            linkLabel_repo.Font = new Font("Segoe UI", 7.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            linkLabel_repo.LinkColor = Color.DimGray;
            linkLabel_repo.Location = new Point(12, 261);
            linkLabel_repo.Name = "linkLabel_repo";
            linkLabel_repo.Size = new Size(158, 17);
            linkLabel_repo.TabIndex = 16;
            linkLabel_repo.TabStop = true;
            linkLabel_repo.Text = "Copyright (c) 2026 Miiraak";
            linkLabel_repo.LinkClicked += LinkLabel_repo_LinkClicked;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_options, toolStripMenuItem_help });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(483, 28);
            menuStrip1.TabIndex = 17;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem_options
            // 
            toolStripMenuItem_options.DropDownItems.AddRange(new ToolStripItem[] { runAsAdministratorToolStripMenuItem });
            toolStripMenuItem_options.Name = "toolStripMenuItem_options";
            toolStripMenuItem_options.Size = new Size(75, 24);
            toolStripMenuItem_options.Text = "Options";
            // 
            // runAsAdministratorToolStripMenuItem
            // 
            runAsAdministratorToolStripMenuItem.Name = "runAsAdministratorToolStripMenuItem";
            runAsAdministratorToolStripMenuItem.Size = new Size(230, 26);
            runAsAdministratorToolStripMenuItem.Text = "Run as Administrator";
            runAsAdministratorToolStripMenuItem.Click += runAsAdministratorToolStripMenuItem_Click;
            // 
            // toolStripMenuItem_help
            // 
            toolStripMenuItem_help.Name = "toolStripMenuItem_help";
            toolStripMenuItem_help.Size = new Size(55, 24);
            toolStripMenuItem_help.Text = "Help";
            toolStripMenuItem_help.Click += toolStripMenuItem_help_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(483, 285);
            Controls.Add(linkLabel_repo);
            Controls.Add(checkBox_verbose);
            Controls.Add(button_copy_args);
            Controls.Add(label_outputfolder);
            Controls.Add(textBox_output_path);
            Controls.Add(buttonOutputFolder);
            Controls.Add(button_select_catalog);
            Controls.Add(textBox_catalog_folder);
            Controls.Add(label_catalog);
            Controls.Add(checkBox_bundeled_catalog);
            Controls.Add(button_convert);
            Controls.Add(label_win32Exec);
            Controls.Add(textBox_exec_path);
            Controls.Add(button_select_file);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Win32-Content-Prep-Tool-GUI";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button_select_file;
        private TextBox textBox_exec_path;
        private Label label_win32Exec;
        private Button button_convert;
        private CheckBox checkBox_bundeled_catalog;
        private Label label_catalog;
        private TextBox textBox_catalog_folder;
        private Button button_select_catalog;
        private Label label_outputfolder;
        private TextBox textBox_output_path;
        private Button buttonOutputFolder;
        private Button button_copy_args;
        private CheckBox checkBox_verbose;
        private LinkLabel linkLabel_repo;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem_options;
        private ToolStripMenuItem runAsAdministratorToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem_help;
    }
}
