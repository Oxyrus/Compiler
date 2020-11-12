using System;
using Compiler.ErrorHandler;
using Compiler.LexicalAnalyzer;
using Compiler.SymbolsTable;
using System.Windows.Forms;

namespace Compiler.SyntacticAnalyzer
{
    public class SyntacticAnalysis
    {
        private readonly LexicalAnalysis _lexicalAnalysis = new LexicalAnalysis();
        private bool _debugEnabled;
        private LexicalComponent _lexicalComponent;
        private string _callStack = string.Empty;

        public void Analyze(bool debug = false)
        {
            _debugEnabled = debug;
            _callStack = string.Empty;

            _lexicalAnalysis.LoadNewLine();
            GetComponent();

            Query("..");

            if (ErrorHandler.ErrorHandler.HasErrors())
            {
                MessageBox.Show("Hay errores de compilación");
            }
            else if (_lexicalComponent.Category == Category.EndOfFile)
            {
                MessageBox.Show("El programa compilo de forma satisfactoria");
            }
            else
            {
                MessageBox.Show("Aunque el programa compilo de manera satisfactoria, faltaron componentes por evaluar");
            }
        }

        private void GetComponent()
        {
            try
            {
                _lexicalComponent = _lexicalAnalysis.BuildComponent();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        private void DebugInput(string indentation, string rule)
        {
            _callStack += indentation + " entrando a la regla " + rule + " con lexema " + _lexicalComponent.Lexeme + " y categoria " + _lexicalComponent.Category + "\n";
            PrintCallStack();
        }

        private void DebugOutput(string indentation, string rule)
        {
            _callStack += indentation + " dejando regla " + rule + "\n";
            PrintCallStack();
        }

        private void PrintCallStack()
        {
            if (_debugEnabled)
            {
                MessageBox.Show(_callStack);
            }
        }

        //<QUERY> := <SELECTOR><COMPARADOR><ORDENACION>
        private void Query(string indentation)
        {
            var indentationNextLevel = indentation + "..";
            DebugInput(indentationNextLevel, "<Query>");
            Selector(indentationNextLevel);
            Comparator(indentationNextLevel);
            Ordination(indentationNextLevel);
            DebugOutput(indentationNextLevel, "<Query>");
        }

        private void Selector(string indentation)
        {
            var indentationNextLevel = indentation + "..";
            DebugInput(indentationNextLevel, "<Selector>");
            if (_lexicalComponent.Category == Category.Select)
            {
                GetComponent();
                Fields(indentationNextLevel);
                if (_lexicalComponent.Category == Category.From)
                {
                    From(indentationNextLevel);
                }
                else
                {
                    var error = Error.CreateSemanticError(
                       _lexicalComponent.Lexeme,
                       _lexicalComponent.LineNumber,
                       _lexicalComponent.InitialPosition,
                       _lexicalComponent.FinalPosition,
                       "I read " + _lexicalComponent.Lexeme, "From not found", "Correct");
                    ErrorHandler.ErrorHandler.Report(error);
                }
            }
            else
            {
                var error = Error.CreateSemanticError(
                   _lexicalComponent.Lexeme,
                   _lexicalComponent.LineNumber,
                   _lexicalComponent.InitialPosition,
                   _lexicalComponent.FinalPosition,
                   "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                ErrorHandler.ErrorHandler.Report(error);
            }
            DebugOutput(indentationNextLevel, "<Selector>");
        }

        private void From(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<From>");
            GetComponent();
            Table(nextLevel);
            DebugOutput(nextLevel, "<From>");
        }

        private void Fields(string indentation)
        {
            var indentationNextLevel = indentation + "..";
            DebugInput(indentationNextLevel, "<Fields>");
            if (_lexicalComponent.Category == Category.Field)
            {
                GetComponent();
                if (_lexicalComponent.Category == Category.Separator)
                {
                    GetComponent();
                    Fields(indentationNextLevel);
                }
            }
            else
            {
                var error = Error.CreateSemanticError(
               _lexicalComponent.Lexeme,
               _lexicalComponent.LineNumber,
               _lexicalComponent.InitialPosition,
               _lexicalComponent.FinalPosition,
               "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                ErrorHandler.ErrorHandler.Report(error);
            }
            DebugOutput(indentationNextLevel, "<Fields>");
        }

        private void Table(string indentation)
        {
            var indentationNextLevel = indentation + "..";
            DebugInput(indentationNextLevel, "<Table>");
            if (_lexicalComponent.Category == Category.Table)
            {
                GetComponent();
                if (_lexicalComponent.Category == Category.Separator)
                {
                    GetComponent();
                    Table(indentationNextLevel);
                }
            }
            else
            {
                var error = Error.CreateSemanticError(
               _lexicalComponent.Lexeme,
               _lexicalComponent.LineNumber,
               _lexicalComponent.InitialPosition,
               _lexicalComponent.FinalPosition,
               "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                ErrorHandler.ErrorHandler.Report(error);
            }
            DebugOutput(indentationNextLevel, "<Table>");
        }

        private void Comparator(string indentation)
        {
            var indentationNextLevel = indentation + "..";
            if (_lexicalComponent.Category == Category.Where)
            {
                DebugInput(indentationNextLevel, "<Comparator>");
                GetComponent();
                Conditions(indentationNextLevel);
                DebugOutput(indentationNextLevel, "<Comparator>");
            }
        }

        private void Conditions(string indentation)
        {
            var indentationNextLevel = indentation + "..";
            DebugInput(indentationNextLevel, "<Conditions>");
            Operating(indentationNextLevel);
            Operator(indentationNextLevel);
            Operating(indentationNextLevel);
            Validator(indentationNextLevel);
            DebugOutput(indentationNextLevel, "<Conditions>");
        }

        private void Operating(string indentation)
        {
            var indentationNextLevel = indentation + "..";
            DebugInput(indentationNextLevel, "<Operating>");
            if (_lexicalComponent.Category == Category.Field)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.Literal)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.Integer || _lexicalComponent.Category == Category.Decimal)
            {
                DebugInput(indentationNextLevel, "<Number>");
                GetComponent();
                DebugOutput(indentationNextLevel, "<Number>");
            }
            else
            {
                var error = Error.CreateSemanticError(
                  _lexicalComponent.Lexeme,
                  _lexicalComponent.LineNumber,
                  _lexicalComponent.InitialPosition,
                  _lexicalComponent.FinalPosition,
                  "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                ErrorHandler.ErrorHandler.Report(error);
            }
            DebugOutput(indentationNextLevel, "<Operating>");
        }

        private void Operator(string indentation)
        {
            var indentationNextLevel = indentation + "..";
            DebugInput(indentationNextLevel, "<Operator>");
            switch (_lexicalComponent.Category)
            {
                case Category.GreaterThan:
                case Category.LessThan:
                case Category.EqualTo:
                case Category.GreaterThanOrEqualTo:
                case Category.LessThanOrEqualTo:
                // Por aqui elimine un different than duplicado xd
                case Category.DifferentThan:
                    GetComponent();
                    break;
                default:
                {
                    var error = Error.CreateSemanticError(
                        _lexicalComponent.Lexeme,
                        _lexicalComponent.LineNumber,
                        _lexicalComponent.InitialPosition,
                        _lexicalComponent.FinalPosition,
                        "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                    ErrorHandler.ErrorHandler.Report(error);
                    break;
                }
            }
            DebugOutput(indentationNextLevel, "<Operator>");
        }

        private void Validator(string indentation)
        {
            var nextLevel = indentation + "..";
            if (_lexicalComponent.Category == Category.And || _lexicalComponent.Category == Category.Or)
            {
                DebugInput(nextLevel, "<Validator>");
                Connector(nextLevel);
                Conditions(nextLevel);
                DebugOutput(nextLevel, "<Validator>");
            }
        }

        private void Connector(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<Connector>");
            switch (_lexicalComponent.Category)
            {
                case Category.And:
                case Category.Or:
                    GetComponent();
                    break;
                default:
                {
                    var error = Error.CreateSemanticError(
                        _lexicalComponent.Lexeme,
                        _lexicalComponent.LineNumber,
                        _lexicalComponent.InitialPosition,
                        _lexicalComponent.FinalPosition,
                        "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                    ErrorHandler.ErrorHandler.Report(error);
                    break;
                }
            }
            DebugOutput(nextLevel, "<Connector>");
        }

        private void Ordination(string indentation)
        {
            var nextLevel = indentation + "..";
            if (_lexicalComponent.Category == Category.Order)
            {
                DebugInput(nextLevel, "<Ordination>");
                GetComponent();
                if (_lexicalComponent.Category == Category.By)
                {
                    GetComponent();
                    Criteria(nextLevel);
                }
                else
                {
                    var error = Error.CreateSemanticError(
                     _lexicalComponent.Lexeme,
                     _lexicalComponent.LineNumber,
                     _lexicalComponent.InitialPosition,
                     _lexicalComponent.FinalPosition,
                     "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                    ErrorHandler.ErrorHandler.Report(error);
                }
                DebugOutput(nextLevel, "<Ordination>");
            }
        }

        private void Criteria(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<Criteria>");
            if (_lexicalComponent.Category == Category.Field)
            {
                Fields(nextLevel);
                Criterion(nextLevel);
            }
            else if (_lexicalComponent.Category == Category.Integer)
            {
                Indices(nextLevel);
                Criterion(nextLevel);
            }
            else
            {
                var error = Error.CreateSemanticError(
                  _lexicalComponent.Lexeme,
                  _lexicalComponent.LineNumber,
                  _lexicalComponent.InitialPosition,
                  _lexicalComponent.FinalPosition,
                  "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                ErrorHandler.ErrorHandler.Report(error);
            }
            DebugOutput(nextLevel, "<Criteria>");
        }

        private void Indices(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<Indices>");
            if (_lexicalComponent.Category == Category.Integer)
            {
                GetComponent();
                if (_lexicalComponent.Category == Category.Separator)
                {
                    GetComponent();
                    Indices(nextLevel);
                }
            }
            else
            {
                var error = Error.CreateSemanticError(
                   _lexicalComponent.Lexeme,
                   _lexicalComponent.LineNumber,
                   _lexicalComponent.InitialPosition,
                   _lexicalComponent.FinalPosition,
                   "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                ErrorHandler.ErrorHandler.Report(error);
            }
            DebugOutput(nextLevel, "<Indices>");
        }

        private void Criterion(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<Criterion>");
            if (_lexicalComponent.Category == Category.Asc)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.Desc)
            {
                GetComponent();
            }
            DebugOutput(nextLevel, "<Criterion>");
        }
    }
}
