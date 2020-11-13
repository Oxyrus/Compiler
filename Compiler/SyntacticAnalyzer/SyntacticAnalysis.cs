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

            Query();

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

        // <query>
        private void Query()
        {
            var indentation = "..";
            GetComponent();
            DebugInput(indentation, "<query>");
            Selector(indentation);
            Where(indentation);
            OrderBy(indentation);
            DebugOutput(indentation, "<query>");
        }

        // <selector>
        private void Selector(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<selector>");

            // Busca SELECT
            if (_lexicalComponent.Category != Category.Select)
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba SELECT",
                    "Utilice un SELECT para iniciar la construcción de la query"
                );

                ErrorHandler.ErrorHandler.Report(error);
                return;
            }

            // Busca CAMPOS
            GetComponent();
            Fields(nextLevel);

            // Busca FROM
            if (_lexicalComponent.Category != Category.From)
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba FROM",
                    "Utilice un SELECT para iniciar la construcción de la query"
                );

                ErrorHandler.ErrorHandler.Report(error);
                return;
            }

            // Busca TABLAS
            GetComponent();
            Tables(nextLevel);

            DebugOutput(nextLevel, "<selector>");
        }

        // <fields>
        private void Fields(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<fields>");

            if (_lexicalComponent.Category == Category.Field)
            {
                GetComponent();

                if (_lexicalComponent.Category == Category.Separator)
                {
                    GetComponent();
                    Fields(nextLevel);
                }
            }
            else
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba <campo>",
                    "Utilice un campo después del <select> al iniciar la construcción de la query"
                );

                ErrorHandler.ErrorHandler.Report(error);
                return;
            }

            DebugOutput(nextLevel, "<fields>");
        }

        // <tables>
        private void Tables(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<tables>");

            if (_lexicalComponent.Category == Category.Table)
            {
                GetComponent();

                if (_lexicalComponent.Category == Category.Separator)
                {
                    GetComponent();
                    Tables(nextLevel);
                }
            }
            else
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba TABLA",
                    "Utilice una TABLA después del FROM al iniciar la construcción de la query"
                );

                ErrorHandler.ErrorHandler.Report(error);
                return;
            }

            DebugOutput(nextLevel, "<tables>");
        }

        // <where>
        private void Where(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<where>");

            if (_lexicalComponent.Category == Category.Where)
            {
                Conditions(nextLevel);
            }

            DebugOutput(nextLevel, "<where>");
        }

        // <conditions>
        private void Conditions(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<conditions>");

            Operating(nextLevel);
            Operator(nextLevel);
            SecondOperating(nextLevel);

            DebugOutput(nextLevel, "<conditions>");
        }

        private void Operating(string indentation)
        {
            GetComponent();
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<operating>");

            // Si no es un operando
            if (_lexicalComponent.Category is not Category.Field
                && _lexicalComponent.Category is not Category.Literal
                && _lexicalComponent.Category is not Category.Integer
                && _lexicalComponent.Category is not Category.Decimal)
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba OPERANDO",
                    "Utilice un OPERANDO para las condiciones"
                );

                ErrorHandler.ErrorHandler.Report(error);
                DebugOutput(nextLevel, "<operating>");
                return;
            }
            DebugOutput(nextLevel, "<operating>");
        }

        private void SecondOperating(string indentation)
        {
            GetComponent();
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<operating>");

            // Si no es un operando
            if (_lexicalComponent.Category is not Category.Field
                && _lexicalComponent.Category is not Category.Literal
                && _lexicalComponent.Category is not Category.Integer
                && _lexicalComponent.Category is not Category.Decimal)
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba OPERANDO",
                    "Utilice un OPERANDO después del operador"
                );

                ErrorHandler.ErrorHandler.Report(error);
                DebugOutput(nextLevel, "<operating>");
                return;
            }

            Connector(nextLevel);

            DebugOutput(nextLevel, "<operating>");
        }

        // <operator>
        private void Operator(string indentation)
        {
            GetComponent();
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<operator>");

            if (_lexicalComponent.Category is not Category.DifferentThan
                && _lexicalComponent.Category is not Category.GreaterThan
                && _lexicalComponent.Category is not Category.LessThan
                && _lexicalComponent.Category is not Category.EqualTo
                && _lexicalComponent.Category is not Category.GreaterThanOrEqualTo
                && _lexicalComponent.Category is not Category.LessThanOrEqualTo)
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba OPERADOR",
                    "Utilice un OPERADOR después de OPERANDO"
                );

                ErrorHandler.ErrorHandler.Report(error);
                DebugOutput(nextLevel, "<operator>");
                return;
            }

            DebugOutput(nextLevel, "<operator>");
        }

        private void Connector(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<connector>");

            GetComponent();

            if (_lexicalComponent.Category == Category.And || _lexicalComponent.Category == Category.Or)
            {
                Conditions(nextLevel);
            }
            else if (_lexicalComponent.Category is Category.Field
                     || _lexicalComponent.Category is Category.Literal
                     || _lexicalComponent.Category is Category.Integer
                     || _lexicalComponent.Category is Category.Decimal)
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba CONECTOR u ORDER BY",
                    "Utilice un CONECTOR"
                );

                ErrorHandler.ErrorHandler.Report(error);
                DebugOutput(nextLevel, "<connector>");
                return;
            }

            DebugOutput(nextLevel, "<connector>");
        }

        private void OrderBy(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<connector>");

            if (_lexicalComponent.Category == Category.Order)
            {
                GetComponent();

                if (_lexicalComponent.Category == Category.By)
                {
                    GetComponent();

                    if (_lexicalComponent.Category == Category.Integer)
                    {
                        Index(nextLevel);
                    }
                    else if (_lexicalComponent.Category == Category.Field)
                    {
                        Fields(nextLevel);
                    }
                    else
                    {
                        var error = Error.CreateSyntacticError(
                            _lexicalComponent.Lexeme,
                            _lexicalComponent.LineNumber,
                            _lexicalComponent.InitialPosition,
                            _lexicalComponent.FinalPosition,
                            "Leí " + _lexicalComponent.Lexeme,
                            "Esperaba CAMPO O INDICE de ordenación",
                            "Utilice un CAMPO O INDICE"
                        );

                        ErrorHandler.ErrorHandler.Report(error);
                        DebugOutput(nextLevel, "<connector>");
                        return;
                    }

                    Criteria(nextLevel);
                }
            }
            else if (_lexicalComponent.Category == Category.EndOfFile)
            {
                // Todo Ok, no lea más
            }
            else
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba ORDER BY",
                    "Utilice un ORDER BY"
                );

                ErrorHandler.ErrorHandler.Report(error);
                DebugOutput(nextLevel, "<connector>");
                return;
            }

            DebugOutput(nextLevel, "<connector>");
        }

        private void Index(string indentation)
        {
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<index>");

            GetComponent();

            if (_lexicalComponent.Category == Category.Separator)
            {
                GetComponent();
                Index(nextLevel);
            }

            DebugOutput(nextLevel, "<index>");
        }

        private void Criteria(string indentation)
        {
            GetComponent();
            var nextLevel = indentation + "..";
            DebugInput(nextLevel, "<criteria>");

            if (_lexicalComponent.Category is Category.Asc
                || _lexicalComponent.Category is Category.Desc)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.EndOfFile)
            {
                // Todo OK
            }
            else
            {
                var error = Error.CreateSyntacticError(
                    _lexicalComponent.Lexeme,
                    _lexicalComponent.LineNumber,
                    _lexicalComponent.InitialPosition,
                    _lexicalComponent.FinalPosition,
                    "Leí " + _lexicalComponent.Lexeme,
                    "Esperaba CRITERIO o FIN DE ARCHIVO",
                    "Utilice un CRITERIO o FIN DE ARCHIVO"
                );

                ErrorHandler.ErrorHandler.Report(error);
                DebugOutput(nextLevel, "<criteria>");
                return;
            }

            DebugOutput(nextLevel, "<criteria>");
        }
    }
}
