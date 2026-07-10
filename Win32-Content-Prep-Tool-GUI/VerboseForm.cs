using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Win32_Content_Prep_Tool_GUI
{
    public partial class VerboseForm : Form
    {
        public VerboseForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Appends text to the rich text box in a thread-safe manner.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void AppendText(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendText), text);
            }
            else
            {
                richTextBox_verbose.AppendText(text + Environment.NewLine);
            }
        }
    }
}
