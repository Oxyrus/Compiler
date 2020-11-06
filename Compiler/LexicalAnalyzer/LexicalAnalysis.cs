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
                _currentCharacter = _currentLine.Content.Substring(_pointer - 1, 1);
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

        private bool CurrentCharacterIsLetter() => char.IsLetter(_currentCharacter, 0);

        private bool CurrentCharacterIsDigit() => char.IsDigit(_currentCharacter, 0);

        private bool CurrentCharacterIsLetterOrDigit() => char.IsLetterOrDigit(_currentCharacter, 0);

        private bool CurrentCharacterIsGreaterThan() => ">" == _currentCharacter;

        private bool CurrentCharacterIsLessThan() => "<" == _currentCharacter;

        private bool CurrentCharacterIsEqual() => "=" == _currentCharacter;

        private bool CurrentCharacterIsExclamationMark() => "!" == _currentCharacter;

        private bool CurrentCharacterIsDot() => "." == _currentCharacter;

        private bool CurrentCharacterIsEndOfLine() => "@FL@" == _currentCharacter;

        private bool CurrentCharacterIsEndOfFile() => "@EOF@" == _currentCharacter;

        private bool CurrentCharacterIsUnderscore() => "_" == _currentCharacter;

        private bool CurrentCharacterIsDollarSign() => "$" == _currentCharacter;

        private bool CurrentCharacterIsSinglequote() => "'" == _currentCharacter;

        private void Concatenate() => _lexeme += _currentCharacter;

        public LexicalComponent BuildComponent()
        {
            LexicalComponent component = null;
            _lexeme = "";
            var currentState = 0;
            var continueAnalysis = true;

            // Hay que validar si en la linea todavia se puede leer más
            if (_currentLine is null || _currentCharacter == "@FL@")
                LoadNewLine();

            while (continueAnalysis)
            {
                #region State 0
                if (currentState == 0)
                {
                    ReadNextCharacter();
                    IgnoreWhitespaces();

                    if (CurrentCharacterIsLetter() || CurrentCharacterIsUnderscore() || CurrentCharacterIsDollarSign())
                    {
                        currentState = 1;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsDigit())
                    {
                        currentState = 3;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsSinglequote())
                    {
                        currentState = 9;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsGreaterThan())
                    {
                        currentState = 12;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsLessThan())
                    {
                        currentState = 15;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsExclamationMark())
                    {
                        currentState = 19;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsEqual())
                    {
                        currentState = 22;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsEndOfLine())
                    {
                        currentState = 23;
                    }
                    else if (CurrentCharacterIsEndOfFile())
                    {
                        currentState = 24;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 25;

                        var error = Error.CreateLexicalError(
                            _lexeme,
                            _currentLineNumber,
                            _pointer - _lexeme.Length,
                            _pointer - 1,
                            "Invalid symbol",
                            _currentCharacter + " was read" + "\"",
                            "Make sure the data is valid");

                        ErrorHandler.ErrorHandler.Report(error);

                        throw new InvalidOperationException("There was a lexical error, check the console for more info.");
                    }
                }
                #endregion

                #region State 1
                else if (currentState == 1)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsDigit() || CurrentCharacterIsLetter() || CurrentCharacterIsUnderscore() || CurrentCharacterIsDollarSign())
                    {
                        currentState = 1;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 2;
                    }
                }
                #endregion

                #region State 3
                else if (currentState == 3)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsDigit())
                    {
                        currentState = 3;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsDot())
                    {
                        currentState = 4;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 7;
                    }
                }
                #endregion

                #region State 9
                else if (currentState == 9)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsSinglequote())
                    {
                        currentState = 10;
                    }
                    else
                    {
                        currentState = 9;
                        Concatenate();
                    }
                }
                #endregion

                #region State 12
                else if (currentState == 12)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsEqual())
                    {
                        currentState = 13;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 14;
                    }
                }
                #endregion

                #region State 15
                else if (currentState == 15)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsGreaterThan())
                    {
                        currentState = 16;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsEqual())
                    {
                        currentState = 17;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 18;
                    }
                }
                #endregion

                #region State 19
                else if (currentState == 19)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsEqual())
                    {
                        currentState = 20;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 21;

                        var error = Error.CreateLexicalError(
                            _lexeme,
                            _currentLineNumber,
                            _pointer - _lexeme.Length,
                            _pointer - 1,
                            "Invalid symbol",
                            _currentCharacter + " was read" + "\"",
                            "Invalid different than");

                        ErrorHandler.ErrorHandler.Report(error);

                        throw new InvalidOperationException("There was a lexical error, check the console for more info.");
                    }
                }
                #endregion

                #region State 4
                else if (currentState == 4)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsDigit())
                    {
                        currentState = 5;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 8;

                        var error = Error.CreateLexicalError(
                            _lexeme,
                            _currentLineNumber,
                            _pointer - _lexeme.Length,
                            _pointer - 1,
                            "Invalid decimal",
                            _currentCharacter + " was read" + "\"",
                            "Make sure the decimal number is valid");

                        ErrorHandler.ErrorHandler.Report(error);

                        throw new InvalidOperationException("There was a lexical error, check the console for more info.");
                    }
                }
                #endregion

                #region State 5
                else if (currentState == 5)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsDigit())
                    {
                        currentState = 5;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 6;
                    }
                }
                #endregion

                #region Identifier
                else if (currentState == 2)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.Identifier, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region Decimal
                else if (currentState == 6)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.Decimal, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region Integer
                else if (currentState == 6)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.Integer, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region Literal
                else if (currentState == 11)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateLiteral(Category.Literal, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region Greater than
                else if (currentState == 14)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.GreaterThan, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region Greater than or equal
                else if (currentState == 13)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.GreaterThanOrEqualTo, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region Less than
                else if (currentState == 18)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.LessThan, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region Less than or equal
                else if (currentState == 17)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.LessThanOrEqualTo, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region Different than
                else if (currentState == 16 || currentState == 20)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.DifferentThan, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region Equal to
                else if (currentState == 22)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.EqualTo, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region End of file
                else if (currentState == 24)
                {
                    ReadNextCharacter();
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.EndOfFile, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region End of line
                else if (currentState == 23)
                {
                    currentState = 0;
                }
                #endregion
            }

            return component;
        }

        private void ResetPointer() => _pointer = 1;

        private void MovePointerForward() => _pointer += 1;

        private void MovePointerBackward() => _pointer -= 1;
    }
}
