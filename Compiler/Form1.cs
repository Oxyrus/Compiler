using System;
using System.Windows.Forms;

namespace Compiler
{
    public partial class Compiler : Form
    {
        public Compiler()
        {
            InitializeComponent();
        }

        private void compileButton_Click(object sender, EventArgs e)
        {
            var output = new Output(codeTextBox.Text);
            outputTextBox.Text = output.FormattedValue;
        }
    }
}
