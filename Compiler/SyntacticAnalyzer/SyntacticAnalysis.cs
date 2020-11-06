using Compiler.LexicalAnalyzer;
using Compiler.SymbolsTable;
using System.Windows.Forms;

namespace Compiler.SyntacticAnalyzer
{
    public class SyntacticAnalysis
    {
        private LexicalAnalysis _lexicalAnalysis = new LexicalAnalysis();
        private bool _debugEnabled = false;
        private LexicalComponent _lexicalComponent;
        private string _callStack = string.Empty;

        public void Analyze(bool debug = false)
        {
            _debugEnabled = debug;
            _callStack = string.Empty;
        }

        private void GetComponent()
        {
            _lexicalComponent = _lexicalAnalysis.BuildComponent();
        }

        public void DebugInput(string indentation, string rule)
        {
            _callStack += indentation + " entering rule " + rule + " with lexeme " + _lexicalComponent.Lexeme + " and category " + _lexicalComponent.Category+ "\n";
            PrintCallStack();
        }

        public void DebugOutput(string indentation, string rule)
        {
            _callStack += indentation + " leaving rule " + rule + "\n";
            PrintCallStack();
        }

        public void PrintCallStack()
        {
            if (_debugEnabled)
            {
                MessageBox.Show(_callStack);
            }
        }

        public void Expression(string indentation)
        {
            var nextLevelIndentation = indentation + "..";
            DebugInput(nextLevelIndentation, "<Expression>");

        }

        /*
        private void Digit(string indentation)
        {
            DebugInput(indentation, "<Digit>");
            var nextLevelIndentation = indentation + "..";
            Factor(nextLevelIndentation);
            TerminoPrima(nextLevelIndentation);
            DepurarSalida(indentation, "<Digit>");
        }

        private void Factor(string indentation)
        {

        }
        */
    }
}
