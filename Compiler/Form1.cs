using Compiler.AnalizadorLexico;
using Compiler.AnalizadorSintactico;
using Compiler.ManejadorErrores;
using Compiler.TablaSimbolos;
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
        public static Output _output;

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
            Inicializar();

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
                Cache.Cache.Poblar(line);
            }

            var analisisSintactico = new AnalisisSintactico();
            // Lectura bandera de depuracion
            analisisSintactico.Analizar(true);

            /*

            var analizadorLexico = new AnalisisLexico();

            ComponenteLexico componenteLexico = null;

            do
            {
                componenteLexico = analizadorLexico.FormarComponente();
                MessageBox.Show(componenteLexico.ToString());
            } while (!componenteLexico.Categoria.Equals(Categoria.FinDeArchivo));
            ¨*/

            tablaSimbolos.DataSource = TablaSimbolos.TablaSimbolos.ObtenerTodosSimbolos();
            tablaDummies.DataSource = TablaDummies.ObtenerTodosSimbolos();
            tablaPalabrasReservadas.DataSource = TablaPalabrasReservadas.ObtenerTodosSimbolos();
            tablaLiterales.DataSource = TablaLiterales.ObtenerTodosSimbolos();
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

        private void Inicializar()
        {
            TablaMaestra.Limpiar();
            GestorErrores.Limpiar();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
