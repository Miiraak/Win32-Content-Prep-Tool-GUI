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
        }

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

        private void ButtonOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                outputFolderPath = folderBrowserDialog.SelectedPath;
                textBox_output_path.Text = outputFolderPath;
            }
        }

        private void Button_select_catalog_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string catalogFolderPath = folderBrowserDialog.SelectedPath;
                textBox_catalog_folder.Text = catalogFolderPath;
            }
        }

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
            {
                arguments += " -q";
            }

            string intuneWinAppUtilPath = Path.Combine(Path.GetTempPath(), "IntuneWinAppUtil.exe");

            // To avoid downloading the tool every time, check if it is already downloaded in the application directory.
            // But if it is not found or intuneWinAppUtil.exe is older than 1 days, ask to download the latest version from GitHub.
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
                            using HttpClient client = new();
                            var response = await client.GetAsync("https://github.com/microsoft/Microsoft-Win32-Content-Prep-Tool/raw/refs/heads/master/IntuneWinAppUtil.exe");
                            response.EnsureSuccessStatusCode();
                            using var fs = new FileStream(intuneWinAppUtilPath, FileMode.Create, FileAccess.Write, FileShare.None);
                            await response.Content.CopyToAsync(fs);
                        }
                        catch (Exception ex) {
                            MessageBox.Show($"Failed to download IntuneWinAppUtil.exe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    } 
                }
            } else {
                try
                {
                    using HttpClient client = new();
                    var response = await client.GetAsync("https://github.com/microsoft/Microsoft-Win32-Content-Prep-Tool/raw/refs/heads/master/IntuneWinAppUtil.exe");
                    response.EnsureSuccessStatusCode();
                    using var fs = new FileStream(intuneWinAppUtilPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    await response.Content.CopyToAsync(fs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to download IntuneWinAppUtil.exe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

                // Run the IntuneWinAppUtil.exe from powershell with the constructed arguments, and redirect the output to the text box if the "Verbose" checkbox is checked.
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
                    // check if produced file already exist
                    try
                    {
                        if (File.Exists(outputFilePath))
                        {
                            File.Delete(outputFilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete existing .intunewin file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    VerboseForm verboseForm = new();
                    verboseForm.Show();
                    process.OutputDataReceived += (s, ea) =>
                    {
                        if (ea.Data != null)
                        {
                            verboseForm.AppendText(ea.Data);
                        }
                    };
                    process.ErrorDataReceived += (s, ea) =>
                    {
                        if (ea.Data != null)
                        {
                            verboseForm.AppendText(ea.Data);
                        }
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

        private void Button_copy_commandline_Click(object sender, EventArgs e)
        {
            string commandLine = $"IntuneWinAppUtil.exe -c \"{sourceFolder}\" -s \"{setupFile}\" -o \"{outputFolderPath}\"";

            if (checkBox_bundeled_catalog.Checked && !string.IsNullOrEmpty(textBox_catalog_folder.Text))
            {
                commandLine += $" -a \"{textBox_catalog_folder.Text}\"";
            }

            if (!checkBox_verbose.Checked)
            {
                commandLine += " -q";
            }

            Clipboard.SetText(commandLine);
            MessageBox.Show("Command line copied to clipboard.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LinkLabel_repo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Open the link in the default browser
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
    }
}
