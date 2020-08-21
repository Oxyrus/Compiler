﻿using System;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace Compiler
{
    public partial class Compiler : Form
    {
        private OpenFileDialog _openFileDialog;
        private string _fileText = string.Empty;
        private Output _output;

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
                _output = new Output(codeTextBox.Text);
                outputTextBox.Text = _output.FormattedValue;
            }
            else
            {
                _output = new Output(_fileText);
                outputTextBox.Text = _output.FormattedValue;
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
