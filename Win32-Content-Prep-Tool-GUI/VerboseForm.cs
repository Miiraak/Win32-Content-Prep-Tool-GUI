namespace Win32_Content_Prep_Tool_GUI
{
    public partial class VerboseForm : Form
    {
        // Use a private readonly field to store the mode (verbose or help) for the form.
        private readonly string mode;
        public VerboseForm(string mode = "log")
        {
            InitializeComponent();

            this.mode = mode.ToLower();
            if (this.mode == "help")
                HelpForm();
        }
        /// <summary>
        /// Appends text to the rich text box in a thread-safe manner.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void AppendText(string text)
        {
            if (InvokeRequired)
                Invoke(new Action<string>(AppendText), text);
            else
                richTextBox_verbose.AppendText(text + Environment.NewLine);
        }

        /// <summary>
        /// Displays help information in the rich text box.
        /// </summary>
        private void HelpForm()
        {
            this.Text = "Win32-Content-Prep-Tool-GUI - Help";

            // Display help information about app use, IntuneWinAppUtil.exe command line arguments, and other relevant details.
            richTextBox_verbose.Text = "Win32-Content-Prep-Tool-GUI Help\n\n" +
                "This application is a GUI wrapper for the IntuneWinAppUtil.exe command line tool.\n\n" +
                "Usage:\n" +
                "1. Select the source folder containing the application files.\n" +
                "2. Specify the output folder where the .intunewin file will be created.\n" +
                "3. Enter the setup file name (the main executable or installer).\n" +
                "4. Optionally, provide any command line arguments for the setup file.\n" +
                "5. Click 'Create IntuneWin' to generate the .intunewin package.\n\n" +
                "Command Line Arguments for IntuneWinAppUtil.exe:\n" +
                "-c <source_folder> : Specifies the source folder containing the application files.\n" +
                "-s <setup_file>    : Specifies the setup file name (main executable or installer).\n" +
                "-o <output_folder> : Specifies the output folder where the .intunewin file will be created.\n" +
                "-q                 : Quiet mode, suppresses output messages.\n\n" +
                "Other Information:\n" +
                "- Ensure that the source folder contains ONLY all necessary files for the application.\n" +
                "   The app will take ALL files in the source folder and package them into the .intunewin file.\n" +
                "- The output folder must be writable and have sufficient space for the .intunewin file.\n" +
                "- The setup file must be present in the source folder and should be the main installer for the application.\n\n" +
                "For more information, refer to the official Microsoft documentation for the Intune Win32 App Packaging Tool:\n" +
                "https://learn.microsoft.com/en-us/intune/app-management/deployment/create-win32-package";
        }
    }
}