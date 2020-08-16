using System;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace Compiler
{
    public partial class Compiler : Form
    {
        private OpenFileDialog _openFileDialog;
        private string _fileText = string.Empty;

        public Compiler()
        {
            InitializeComponent();
            _openFileDialog = new OpenFileDialog()
            {
                FileName = "Select a text file",
                Filter = "Text files (*.txt)|*.txt",
                Title = "Open text file"
            };
        }

        private void compileButton_Click(object sender, EventArgs e)
        {
            if (optionsTabControl.SelectedTab == editorTab)
            {
                var output = new Output(codeTextBox.Text);
                outputTextBox.Text = output.FormattedValue;
            }
            else
            {
                var output = new Output(_fileText);
                outputTextBox.Text = output.FormattedValue;
            }
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filePath = _openFileDialog.FileName;
                    _fileText = File.ReadAllText(filePath);
                }
                catch (SecurityException)
                {
                    MessageBox.Show("Security error");
                }
            }
        }
    }
}
