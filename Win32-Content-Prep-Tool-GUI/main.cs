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

            if (!File.Exists(intuneWinAppUtilPath))
            {
                // Open a message box to to choose whether to download the tool from GitHub or to select the path to the tool if it is already downloaded.
                DialogResult result = MessageBox.Show("IntuneWinAppUtil.exe not found. Do you want to download it from GitHub?", "Download Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    OpenFileDialog openFileDialog = new()
                    {
                        Filter = "Executable files (*.exe)|*.exe",
                        Title = "Select IntuneWinAppUtil.exe"
                    };
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Check if the selected file is IntuneWinAppUtil.exe
                        if (Path.GetFileName(openFileDialog.FileName) != "IntuneWinAppUtil.exe")
                        {
                            MessageBox.Show("The selected file is not IntuneWinAppUtil.exe. Please select the correct file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        intuneWinAppUtilPath = openFileDialog.FileName;
                    }
                    else { return; }
                }
                else
                {
                    using HttpClient client = new();
                    var response = await client.GetAsync("https://github.com/microsoft/Microsoft-Win32-Content-Prep-Tool/raw/refs/heads/master/IntuneWinAppUtil.exe");
                    response.EnsureSuccessStatusCode();
                    using var fs = new FileStream(intuneWinAppUtilPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    await response.Content.CopyToAsync(fs);
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
                    // check if produced file already exist
                    if (File.Exists(Path.Combine(outputFolderPath, setupFile.Replace(".exe", ".intunewin"))))
                    {
                        File.Delete(Path.Combine(outputFolderPath, setupFile.Replace(".exe", ".intunewin")));
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

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open the link in the default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Miiraak/Win32-Content-Prep-Tool-GUI",
                UseShellExecute = true
            });


        }
    }
}
