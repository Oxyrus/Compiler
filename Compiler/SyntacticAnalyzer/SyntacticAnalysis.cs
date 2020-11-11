using Compiler.ErrorHandler;
using Compiler.LexicalAnalyzer;
using Compiler.SymbolsTable;
using System;
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
        //<QUERY> := <SELECTOR><COMPARADOR><ORDENACION>

        public void Query(string identation)
        {
            var identatioNextLevel = identation + "..";
            Selector(identatioNextLevel);
            Comparator(identatioNextLevel);
            ordination(identatioNextLevel);
        }
        private void Selector(string identation)
        {
            var identatioNextLevel = identation + "..";
            if (_lexicalComponent.Category == Category.Select)
            {
                GetComponent();
                Fields(identatioNextLevel);
                if (_lexicalComponent.Category == Category.From)
                {
                    GetComponent();
                    Table(identatioNextLevel);
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

        }

        private void Fields(string identation)
        {
            var identatioNextLevel = identation + "..";
            if (_lexicalComponent.Category == Category.Field)
            {
                GetComponent();
                if(_lexicalComponent.Category == Category.Separator)
                {
                    GetComponent();
                    Fields(identatioNextLevel);
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

        }

        private void Table(string identation)
        {
            var identatioNextLevel = identation + "..";
            if (_lexicalComponent.Category == Category.Table)
            {
                GetComponent();
                if (_lexicalComponent.Category == Category.Separator)
                {
                    GetComponent();
                    Table(identatioNextLevel);
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

        }

        private void Comparator(string identation)
        {
            var identatioNextLevel = identation + "..";
       
            if (_lexicalComponent.Category == Category.Where)
            {
                GetComponent();
                Conditions(identatioNextLevel);

            }
        }

        private void Conditions(string identation)
        {
            var identatioNextLevel = identation + "..";
            Operating(identatioNextLevel);
            Operator(identatioNextLevel);
            Operating(identatioNextLevel);
            Validator(identatioNextLevel);
        }
        private void Operating(string identation)
        {
            var identatioNextLevel = identation + "..";
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
                GetComponent();

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
        }
        private void Operator(string identation)
        {
            var identatioNextLevel = identation + "..";
            if (_lexicalComponent.Category == Category.GreaterThan)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.LessThan)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.EqualTo)
            {
                GetComponent();

            }
            else if (_lexicalComponent.Category == Category.GreaterThanOrEqualTo)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.LessThanOrEqualTo )
            {
                GetComponent();

            }
            else if (_lexicalComponent.Category == Category.DifferentThan)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.DifferentThan )
            {
                GetComponent();

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
        }
        private void Validator(string identation)
        {
            var identatioNextLevel = identation + "..";
            if (_lexicalComponent.Category == Category.And || _lexicalComponent.Category == Category.Or)
            {
                Conector(identatioNextLevel);
                Conditions(identatioNextLevel);
            }
        }

        private void Conector(string identation)
        {
            var identatioNextLevel = identation + "..";
            if (_lexicalComponent.Category == Category.And)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.Or)
            {
                GetComponent();
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

        }

        private void ordination(string identation)
        {
            var identatioNextLevel = identation + "..";

            if (_lexicalComponent.Category == Category.Order_by)
            {
                GetComponent();
                Criteria(identatioNextLevel);

            }

        }

        private void Criteria(string identation)
        {
            var identatioNextLevel = identation + "..";
            if (_lexicalComponent.Category == Category.Field)
            {
                Fields(identatioNextLevel);
                Criterion(identatioNextLevel);

            }else if(_lexicalComponent.Category == Category.Integer)
            {
                Indices(identatioNextLevel);
                Criterion(identatioNextLevel);
            }else
            {
                 var error = Error.CreateSemanticError(
               _lexicalComponent.Lexeme,
               _lexicalComponent.LineNumber,
               _lexicalComponent.InitialPosition,
               _lexicalComponent.FinalPosition,
               "I read " + _lexicalComponent.Lexeme, "Lexical error ", "Correct");
                ErrorHandler.ErrorHandler.Report(error);
            }
        }
        private void Indices(string identation)
        {
            var identatioNextLevel = identation + "..";
            if (_lexicalComponent.Category == Category.Integer)
            {
                GetComponent();
                if (_lexicalComponent.Category == Category.Separator)
                {
                    GetComponent();
                    Indices(identatioNextLevel);
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

        }

        private void Criterion(string identation)
        {
            var identatioNextLevel = identation + "..";
            if (_lexicalComponent.Category == Category.Asc)
            {
                GetComponent();
            }
            else if (_lexicalComponent.Category == Category.Desc)
            {
                GetComponent();
            }
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
