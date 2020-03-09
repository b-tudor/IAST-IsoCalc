using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IAST
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            reference1RichTextBox.Rtf = @"{\rtf1\ansi Myers, A. L. and Prausnitz, J. M. (1965), {\i Thermodynamics of mixed-gas adsorption}. AIChE J., 11: 121-127. doi: 10.1002/aic.690110125}";
            reference2RichTextBox.Rtf = @"{\rtf1\ansi Yang, R. T. {\i Gas Separation by Adsorption Processes}. Imperial College Press, 1986.}";
            acknowledgementsRichTextBox.Rtf = @"{\rtf1\ansi Thank you to Tony Pham for technical assistance and to Shawn Low for his help in UI design and testing.}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // reference 1
            Clipboard.SetText(reference1RichTextBox.Text);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // reference 2
            Clipboard.SetText(reference2RichTextBox.Text);
        }
    }
}
