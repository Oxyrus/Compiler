using Compiler.Cache;
using Compiler.ErrorHandler;
using Compiler.SymbolsTable;
using System;

namespace Compiler.LexicalAnalyzer
{
    public class LexicalAnalysis
    {
        private int _currentLineNumber;
        private int _pointer;
        private string _currentCharacter;
        private string _lexeme = "";
        private Line _currentLine;

        private void LoadNewLine()
        {
            _currentLineNumber += 1;
            _currentLine = Cache.Cache.GetLine(_currentLineNumber);

            if (_currentLine.Content == "@EOF@")
            {
                _currentLineNumber = _currentLine.Number;
            }

            ResetPointer();
        }

        private void ReadNextCharacter()
        {
            if (_currentLine.Content == "@EOF@")
            {
                _currentCharacter = _currentLine.Content;
            }
            else if (_pointer > _currentLine.Content.Length)
            {
                _currentCharacter = "@FL@";
            }
            else
            {
                _currentCharacter = _currentLine.Content.Substring(_pointer, 1);
                MovePointerForward();
            }
        }

        private void IgnoreWhitespaces()
        {
            while (_currentCharacter == " ")
            {
                ReadNextCharacter();
            }
        }

        private bool CaracterActualEsLetra() => Char.IsLetter(_currentCharacter, 0);

        private bool CaracterActualEsDigito() => Char.IsDigit(_currentCharacter, 0);

        private bool CaracterActualEsLetraODigito() => Char.IsLetterOrDigit(_currentCharacter, 0);

        private bool CaracterActualEsSignoPesos() => "$".Equals(_currentCharacter);

        private bool CaracterActualEsGuionBajo() => "_".Equals(_currentCharacter);

        private void Concatenar() => _lexeme += _currentCharacter;

        private bool CaracterActualEsSuma() => "+".Equals(_currentCharacter);

        private bool CaracterActualEsResta() => "-".Equals(_currentCharacter);

        private bool CaracterActualEsMultiplicacion() => "*".Equals(_currentCharacter);

        private bool CaracterActualEsDivision() => "/".Equals(_currentCharacter);

        private bool CaracterActualEsModulo() => "%".Equals(_currentCharacter);

        private bool CaracterActualEsParentesisAbre() => "(".Equals(_currentCharacter);

        private bool CaracterActualEsParentesisCierra() => ")".Equals(_currentCharacter);

        private bool CaracterActualEsIgual() => "=".Equals(_currentCharacter);

        private bool CaracterActualEsMenorQue() => "<".Equals(_currentCharacter);

        private bool CaracterActualEsMayorQue() => ">".Equals(_currentCharacter);

        private bool CaracterActualEsDosPuntos() => ":".Equals(_currentCharacter);

        private bool CaracterActualEsSignoExclamacion() => "!".Equals(_currentCharacter);

        private bool CaracterActualEsFinDeLinea() => "@FL@".Equals(_currentCharacter);

        private bool CaracterActualEsFinDeArchivo() => "@EOF@".Equals(_currentCharacter);

        private bool CaracterActualEsComa() => ",".Equals(_currentCharacter);

        public LexicalComponent BuildComponent()
        {
            LexicalComponent component = null;
            var lexeme = "";
            var currentState = 0;
            var continueAnalysis = true;

            while (continueAnalysis)
            {
             
                {
                    if (currentState == 0)
                    {
                        ReadNextCharacter();
                        IgnoreWhitespaces();

                        if (CaracterActualEsLetra() || CaracterActualEsGuionBajo() || CaracterActualEsSignoPesos())
                        {
                            currentState = 4;
                            Concatenar();
                        }
                        else if (CaracterActualEsDigito())
                        {
                            currentState = 1;
                            Concatenar();
                        }
                        else if (CaracterActualEsSuma())
                        {
                            currentState = 5;
                            Concatenar();
                        }
                        else if (CaracterActualEsResta())
                        {
                            currentState = 6;
                            Concatenar();
                        }
                        else if (CaracterActualEsMultiplicacion())
                        {
                            currentState = 7;
                            Concatenar();
                        }
                        else if (CaracterActualEsMultiplicacion())
                        {
                            currentState = 7;
                            Concatenar();
                        }
                        else if (CaracterActualEsDivision())
                        {
                            currentState = 8;
                            Concatenar();
                        }
                        else if (CaracterActualEsModulo())
                        {
                            currentState = 9;
                            Concatenar();
                        }
                        else if (CaracterActualEsParentesisAbre())
                        {
                            currentState = 10;
                            Concatenar();
                        }
                        else if (CaracterActualEsParentesisCierra())
                        {
                            currentState = 11;
                            Concatenar();
                        }
                        else if (CaracterActualEsIgual())
                        {
                            currentState = 19;
                            Concatenar();
                        }
                        else if (CaracterActualEsMenorQue())
                        {
                            currentState = 20;
                            Concatenar();
                        }
                        else if (CaracterActualEsMayorQue())
                        {
                            currentState = 21;
                            Concatenar();
                        }
                        else if (CaracterActualEsDosPuntos())
                        {
                            currentState = 22;
                            Concatenar();
                        }
                        else if (CaracterActualEsSignoExclamacion())
                        {
                            currentState = 30;
                            Concatenar();
                        }
                        else if (CaracterActualEsFinDeLinea())
                        {
                            currentState = 13;
                        }
                        else if (CaracterActualEsFinDeArchivo())
                        {
                            currentState = 12;
                            Concatenar();
                        }
                        else
                        {
                            currentState = 18;

                            var error = Error.CreateLexicalError(
                                _lexeme,
                                _currentLineNumber,
                                _pointer - _lexeme.Length,
                                _pointer - 1,
                                "Símbolo no válido",
                                "Leí \"" + _currentCharacter + "\"",
                                "Asegúrese que los símbolos ingresados son válidos");

                            ErrorHandler.ErrorHandler.Report(error);

                            throw new Exception("Se ha presentado un error léxico que tiene el proceso, por favor validar la consola de errores");
                        }
                    }
                    else if (currentState == 4)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsLetraODigito() || CaracterActualEsSignoPesos() || CaracterActualEsGuionBajo())
                        {
                            currentState = 4;
                            Concatenar();
                        }
                        else
                        {
                            currentState = 16;
                        }
                    }
                    else if (currentState == 1)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsDigito())
                        {
                            currentState = 1;
                            Concatenar();
                        }
                        else if (CaracterActualEsComa())
                        {
                            currentState = 2;
                            Concatenar();
                        }
                        else
                        {
                            currentState = 14;
                        }
                    }
                    else if (currentState == 2)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsDigito())
                        {
                            currentState = 3;
                            Concatenar();
                        }
                        else
                        {
                            currentState = 17;
                            var error = Error.CreateLexicalError(
                                _lexeme,
                                _currentLineNumber,
                                _pointer - _lexeme.Length,
                                _pointer - 1,
                                "Número decimal no válido",
                                "Después del separador decimal leí \"" + _currentCharacter + "\"",
                                "Asegúrese de que luego del separador decimal se encuentre un dígito del 0 al 9");

                            ErrorHandler.ErrorHandler.Report(error);

                            component = LexicalComponent.CreateDummy(Category.Decimal, _lexeme + "0", _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);

                            MasterTable.Add(component);

                            continueAnalysis = false;
                        }
                    }
                    else if (currentState == 3)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsDigito())
                        {
                            currentState = 3;
                            Concatenar();
                        }
                        else
                        {
                            currentState = 15;
                        }
                    }
                    else if (currentState == 8)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsMultiplicacion())
                        {
                            currentState = 34;
                            Concatenar();
                        }
                        else if (CaracterActualEsDivision())
                        {
                            currentState = 36;
                            Concatenar();
                        }
                        else
                        {
                            currentState = 33;
                        }
                    }
                    else if (currentState == 34)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsMultiplicacion())
                        {
                            currentState = 35;
                            Concatenar();
                        }
                        else
                        {
                            currentState = 34;
                            Concatenar();
                        }
                    }
                    else if (currentState == 35)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsMultiplicacion())
                        {
                            currentState = 35;
                            Concatenar();
                        }
                        else if (CaracterActualEsDivision())
                        {
                            currentState = 0;
                            Concatenar();
                        }
                        else
                        {
                            currentState = 34;
                            Concatenar();
                        }
                    }
                    else if (currentState == 36)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsFinDeLinea())
                        {
                            currentState = 13;
                        }
                        else
                        {
                            currentState = 36;
                            Concatenar();
                        }
                    }
                    else if (currentState == 13)
                    {

                        currentState = 0;
                    }
                    else if (currentState == 16)
                    {
                        ReadNextCharacter();
                        MovePointerBackward();
                        component = LexicalComponent.CreateSymbol(Category.Identifier, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                        MasterTable.Add(component);
                        continueAnalysis = false;
                    }
                    else if (currentState == 14)
                    {
                        ReadNextCharacter();
                        MovePointerBackward();
                        component = LexicalComponent.CreateSymbol(Category.Integer, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                        MasterTable.Add(component);
                        continueAnalysis = false;
                    }
                    else if (currentState == 15)
                    {
                        ReadNextCharacter();
                        MovePointerBackward();
                        component = LexicalComponent.CreateSymbol(Category.Decimal, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                        MasterTable.Add(component);
                        continueAnalysis = false;
                    }
                    else if (currentState == 12)
                    {
                        ReadNextCharacter();
                        MovePointerBackward();
                        component = LexicalComponent.CreateSymbol(Category.EndOfFile, _lexeme, _currentLineNumber, _pointer, _pointer);
                        MasterTable.Add(component);
                        continueAnalysis = false;
                    }
                    else if (currentState == 19)
                    {
                        ReadNextCharacter();
                        MovePointerBackward();
                        component = LexicalComponent.CreateSymbol(Category.EqualTo, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                        MasterTable.Add(component);
                        continueAnalysis = false;
                    }
                    else if (currentState == 20)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsMayorQue())
                        {
                            MovePointerBackward();
                            component = LexicalComponent.CreateSymbol(Category.DifferentThan, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                            MasterTable.Add(component);
                            continueAnalysis = false;
                        }
                        else if (CaracterActualEsIgual())
                        {
                            MovePointerBackward();
                            component = LexicalComponent.CreateSymbol(Category.LessThanOrEqualTo, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                            MasterTable.Add(component);
                            continueAnalysis = false;
                        }
                        else
                        {
                            MovePointerBackward();
                            component = LexicalComponent.CreateSymbol(Category.LessThan, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                            MasterTable.Add(component);
                            continueAnalysis = false;
                        }
                    }
                    else if (currentState == 21)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsIgual())
                        {
                            MovePointerBackward();
                            component = LexicalComponent.CreateSymbol(Category.GreaterThanOrEqualTo, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                            MasterTable.Add(component);
                            continueAnalysis = false;
                        }
                        else
                        {
                            MovePointerBackward();
                            component = LexicalComponent.CreateSymbol(Category.GreaterThan, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                            MasterTable.Add(component);
                            continueAnalysis = false;
                        }
                    }
                    else if (currentState == 30)
                    {
                        ReadNextCharacter();

                        if (CaracterActualEsIgual())
                        {
                            MovePointerBackward();
                            component = LexicalComponent.CreateSymbol(Category.DifferentThan, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                            MasterTable.Add(component);
                            continueAnalysis = false;
                        }
                        else
                        {
                            currentState = 32;
                            var error = Error.CreateLexicalError(
                                _lexeme,
                                _currentLineNumber,
                                _pointer - _lexeme.Length,
                                _pointer - 1,
                                "Asignación no valida",
                                "Después de símbolo de exlamación (!) leí \"" + _currentCharacter + "\"",
                                "Asegúrese de diferenciar utilizando !=");

                            component = LexicalComponent.CreateDummy(Category.DifferentThan, _lexeme + "=", _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);

                            MasterTable.Add(component);

                            continueAnalysis = false;

                            ErrorHandler.ErrorHandler.Report(error);
                        }
                    }
                }

            }

            // return component;
            return null;
        }

        private void ResetPointer() => _pointer = 0;

        private void MovePointerForward() => _pointer += 1;

        private void MovePointerBackward() => _pointer -= 1;
    }
}
