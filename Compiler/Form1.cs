using Compiler.LexicalAnalyzer;
using Compiler.SymbolsTable;
using Compiler.SyntacticAnalyzer;
using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;

namespace Compiler
{
    public partial class Compiler : Form
    {
        private readonly OpenFileDialog _openFileDialog;
        private string _fileText = string.Empty;
        private static Output _output;

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

        private void CompileButton_Click(object sender, EventArgs e)
        {
            MasterTable.Clear();
            Cache.Cache.Clear();
            ErrorHandler.ErrorHandler.Clear();

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

            foreach (var line in _output.Value)
            {
                Cache.Cache.Populate(line);
            }

            var syntacticAnalyzer = new SyntacticAnalysis();

            syntacticAnalyzer.Analyze();

            ConfigureSymbolsTable();
            ConfigureReservedWordsTable();
            ConfigureLiteralsTable();
            ConfigureErrorsTable();
        }

        private void ConfigureSymbolsTable()
        {
            symbolsTable.DataSource = SymbolsTable.SymbolsTable.ObtainAllSymbols();
            symbolsTable.Columns["LineNumber"].Visible = false;
            symbolsTable.Columns["InitialPosition"].Visible = false;
            symbolsTable.Columns["FinalPosition"].Visible = false;
        }

        private void ConfigureReservedWordsTable()
        {
            reservedKeywordsTable.DataSource = ReservedKeywordsTable.ObtainAllSymbols();
            reservedKeywordsTable.Columns["LineNumber"].Visible = false;
            reservedKeywordsTable.Columns["InitialPosition"].Visible = false;
            reservedKeywordsTable.Columns["FinalPosition"].Visible = false;
        }

        private void ConfigureLiteralsTable()
        {
            literalsTable.DataSource = LiteralsTable.ObtainAllSymbols();
            literalsTable.Columns["LineNumber"].Visible = false;
            literalsTable.Columns["InitialPosition"].Visible = false;
            literalsTable.Columns["FinalPosition"].Visible = false;
        }

        private void ConfigureErrorsTable()
        {
            errorsTable.DataSource = ErrorHandler.ErrorHandler.ObtainAllErrors().Take(1).ToList();
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
