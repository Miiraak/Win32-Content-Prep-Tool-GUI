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
