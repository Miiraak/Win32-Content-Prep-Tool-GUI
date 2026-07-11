using System.Diagnostics;

namespace Win32_Content_Prep_Tool_GUI
{
    public partial class Main : Form
    {
        private string sourceFolder;
        private string setupFile;
        private string outputFolderPath;

        public Main()
        {
            InitializeComponent();

            if (IsAdministrator())
            {
                // change the title of the form to indicate that it is running as administrator
                this.Text = "Win32 Content Prep Tool GUI (Administrator)";
            }
        }

        /// <summary>
        /// Handles the click event for the "Select File" button. Opens a file dialog to select an executable or MSI file, and updates the source folder and setup file accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Button_select_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Executable files (*.exe;*.msi)|*.exe;*.msi|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                sourceFolder = Path.GetDirectoryName(filePath);
                setupFile = Path.GetFileName(filePath);
                textBox_exec_path.Text = filePath;
            }
        }

        /// <summary>
        /// Handles the click event for the "Select Output Folder" button. Opens a folder browser dialog to select an output folder, and updates the output folder path accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ButtonOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                outputFolderPath = folderBrowserDialog.SelectedPath;
                textBox_output_path.Text = outputFolderPath;
            }
        }

        /// <summary>
        /// Handles the click event for the "Select Catalog" button. Opens a folder browser dialog to select a catalog folder, and updates the catalog folder path accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Button_select_catalog_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string catalogFolderPath = folderBrowserDialog.SelectedPath;
                textBox_catalog_folder.Text = catalogFolderPath;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event for the "Bundled Catalog" checkbox. Enables or disables the catalog folder selection controls based on the checkbox state.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CheckBox_bundeled_catalog_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_bundeled_catalog.Checked)
            {
                textBox_catalog_folder.Enabled = true;
                label_catalog.Enabled = true;
                button_select_catalog.Enabled = true;
            }
            else
            {
                textBox_catalog_folder.Enabled = false;
                label_catalog.Enabled = false;
                button_select_catalog.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the click event for the "Convert" button. Validates the input, constructs the command line arguments, checks for the IntuneWinAppUtil.exe tool, downloads it if necessary, and executes the conversion process. Displays verbose output if enabled and shows a message when the conversion is complete.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void Button_convert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sourceFolder) || string.IsNullOrEmpty(setupFile) || string.IsNullOrEmpty(outputFolderPath))
            {
                MessageBox.Show("Please select a source file and an output folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string arguments = $"-c \"{sourceFolder}\" -s \"{setupFile}\" -o \"{outputFolderPath}\"";

            if (checkBox_bundeled_catalog.Checked)
            {
                if (string.IsNullOrEmpty(textBox_catalog_folder.Text))
                {
                    MessageBox.Show("Please select a catalog folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string catalogFolderPath = textBox_catalog_folder.Text;
                arguments += $" -a \"{catalogFolderPath}\"";
            }

            if (!checkBox_verbose.Checked)
                arguments += " -q";

            string intuneWinAppUtilPath = Path.Combine(Path.GetTempPath(), "Win32-Content-Prep-Tool-GUI", "IntuneWinAppUtil.exe");


            if (File.Exists(intuneWinAppUtilPath))
            {
                DateTime lastWriteTime = File.GetLastWriteTime(intuneWinAppUtilPath);
                if ((DateTime.Now - lastWriteTime).TotalDays > 1)
                {
                    DialogResult result = MessageBox.Show("IntuneWinAppUtil.exe is older than 1 day. Do you want to download the latest version from GitHub?", "Download Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            using HttpClient client = new() { Timeout = TimeSpan.FromSeconds(30) };
                            using var response = await client.GetAsync("https://github.com/microsoft/Microsoft-Win32-Content-Prep-Tool/raw/refs/heads/master/IntuneWinAppUtil.exe");
                            response.EnsureSuccessStatusCode();
                            using var fs = new FileStream(intuneWinAppUtilPath, FileMode.Create, FileAccess.Write, FileShare.None);
                            await response.Content.CopyToAsync(fs);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to download IntuneWinAppUtil.exe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (File.Exists(intuneWinAppUtilPath))
                            {
                                try
                                {
                                    File.Delete(intuneWinAppUtilPath);
                                }
                                catch (Exception deleteEx)
                                {
                                    MessageBox.Show($"Failed to delete the corrupted IntuneWinAppUtil.exe file: {deleteEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            return;
                        }
                    }
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("IntuneWinAppUtil.exe is not found. Do you want to download the latest version from GitHub?", "Download Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(intuneWinAppUtilPath));
                    try
                    {
                        using HttpClient client = new() { Timeout = TimeSpan.FromSeconds(30) };
                        using var response = await client.GetAsync("https://github.com/microsoft/Microsoft-Win32-Content-Prep-Tool/raw/refs/heads/master/IntuneWinAppUtil.exe");
                        response.EnsureSuccessStatusCode();
                        using var fs = new FileStream(intuneWinAppUtilPath, FileMode.Create, FileAccess.Write, FileShare.None);
                        await response.Content.CopyToAsync(fs);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to download IntuneWinAppUtil.exe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (File.Exists(intuneWinAppUtilPath))
                        {
                            try
                            {
                                File.Delete(intuneWinAppUtilPath);
                            }
                            catch (Exception deleteEx)
                            {
                                MessageBox.Show($"Failed to delete the corrupted IntuneWinAppUtil.exe file: {deleteEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("IntuneWinAppUtil.exe is required for the conversion process. Please download it manually from the GitHub repository.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Hash check for IntuneWinAppUtil.exe to ensure it is the expected version and has not been tampered with
            string expectedHash = "C1BA45B5CB939E84AF064BB7FF4B38FB3DFE33C8DC1078FD9B157672EAE671F6";
            try
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    using var stream = File.OpenRead(intuneWinAppUtilPath);
                    var hashBytes = sha256.ComputeHash(stream);
                    var actualHash = Convert.ToHexString(hashBytes).ToUpperInvariant();
                    if (actualHash != expectedHash)
                    {
                        MessageBox.Show("The downloaded IntuneWinAppUtil.exe file is corrupted, has been tampered with or is not the expected version. Please download it manually from the GitHub repository.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        try
                        {
                            File.Delete(intuneWinAppUtilPath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to delete the corrupted IntuneWinAppUtil.exe file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to verify the integrity of IntuneWinAppUtil.exe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    File.Delete(intuneWinAppUtilPath);
                }
                catch (Exception deleteEx)
                {
                    MessageBox.Show($"Failed to delete the corrupted IntuneWinAppUtil.exe file: {deleteEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            ProcessStartInfo startInfo = new()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"& '{intuneWinAppUtilPath}' {arguments}\"",
                RedirectStandardOutput = checkBox_verbose.Checked,
                RedirectStandardError = checkBox_verbose.Checked,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new() { StartInfo = startInfo })
            {
                if (checkBox_verbose.Checked)
                {
                    string outputFilePath = Path.Combine(outputFolderPath, Path.ChangeExtension(setupFile, ".intunewin"));
                    try
                    {
                        if (File.Exists(outputFilePath))
                            File.Delete(outputFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete existing .intunewin file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    VerboseForm verboseForm = new VerboseForm("log");
                    verboseForm.Show();
                    process.OutputDataReceived += (s, ea) =>
                    {
                        if (ea.Data != null)
                            verboseForm.AppendText(ea.Data);
                    };
                    process.ErrorDataReceived += (s, ea) =>
                    {
                        if (ea.Data != null)
                            verboseForm.AppendText(ea.Data);
                    };
                }
                process.Start();
                if (checkBox_verbose.Checked)
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                }
                await process.WaitForExitAsync();
            }

            MessageBox.Show("Conversion complete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start("explorer.exe", outputFolderPath);
        }

        /// <summary>
        /// Handles the click event for the "Copy Command Line" button. Constructs the command line arguments based on the current settings and copies them to the clipboard.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Button_copy_commandline_Click(object sender, EventArgs e)
        {
            string commandLine = $"IntuneWinAppUtil.exe -c \"{sourceFolder}\" -s \"{setupFile}\" -o \"{outputFolderPath}\"";

            if (checkBox_bundeled_catalog.Checked && !string.IsNullOrEmpty(textBox_catalog_folder.Text))
                commandLine += $" -a \"{textBox_catalog_folder.Text}\"";

            if (!checkBox_verbose.Checked)
                commandLine += " -q";

            Clipboard.SetText(commandLine);
            MessageBox.Show("Command line copied to clipboard.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles the LinkClicked event for the repository link label. Opens the GitHub repository URL in the default web browser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void LinkLabel_repo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/Miiraak/Win32-Content-Prep-Tool-GUI",
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the click event for the "Run as Administrator" menu item. Restarts the application with elevated privileges if not already running as administrator.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void runAsAdministratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {
                try
                {
                    ProcessStartInfo startInfo = new()
                    {
                        FileName = Application.ExecutablePath,
                        UseShellExecute = true,
                        Verb = "runas"
                    };
                    Process.Start(startInfo);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to restart as administrator: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The application is already running with administrator privileges.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Checks if the current user has administrator privileges.
        /// </summary>
        /// <returns>True if the current user has administrator privileges; otherwise, false.</returns>
        private static bool IsAdministrator()
        {
            using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Handles the click event for the "Help" menu item. Opens the help documentation or displays a message box with help information.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void toolStripMenuItem_help_Click(object sender, EventArgs e)
        {
            // Call the VerboseForm with "help" mode to display help information
            VerboseForm helpForm = new VerboseForm("help");
            helpForm.Show();
        }
    }
}
