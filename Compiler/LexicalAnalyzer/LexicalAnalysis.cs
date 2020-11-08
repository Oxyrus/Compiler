using Compiler.Cache;
using Compiler.ErrorHandler;
using Compiler.SymbolsTable;

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

        private bool CurrentCharacterIsComma() => "," == _currentCharacter;

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

            while (continueAnalysis)
            {
                #region State 0
                if (currentState == 0)
                {
                    ReadNextCharacter();
                    IgnoreWhitespaces();

                    if (CurrentCharacterIsDigit())
                    {
                        currentState = 57;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsEqual())
                    {
                        currentState = 22;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsGreaterThan())
                    {
                        currentState = 39;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsLessThan())
                    {
                        currentState = 44;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsExclamationMark())
                    {
                        currentState = 48;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "s")
                    {
                        currentState = 1;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "c")
                    {
                        currentState = 7;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "f")
                    {
                        currentState = 12;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "t")
                    {
                        currentState = 16;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "w")
                    {
                        currentState = 24;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsSinglequote())
                    {
                        currentState = 30;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "a")
                    {
                        currentState = 33;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "d")
                    {
                        currentState = 40;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "o")
                    {
                        currentState = 45;
                        Concatenate();
                    }
                    // REVISAR EL MANEJO DE LOS PUNTOS
                    /*
                    else if (CurrentCharacterIsDot())
                    {
                        currentState = 84;
                        Concatenate();
                    }
                    */
                    else if (CurrentCharacterIsComma())
                    {
                        currentState = 84;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsEndOfLine())
                    {
                        currentState = 38;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsEndOfFile())
                    {
                        currentState = 32;
                    }
                    else
                    {
                        currentState = 71;
                    }
                }
                #endregion

                #region State 1
                else if (currentState == 1)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "e")
                    {
                        currentState = 2;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 72;
                    }
                }
                #endregion

                #region State 2
                else if (currentState == 2)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "l")
                    {
                        currentState = 3;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 72;
                    }
                }
                #endregion

                #region State 3
                else if (currentState == 3)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "e")
                    {
                        currentState = 4;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 72;
                    }
                }
                #endregion

                #region State 4
                else if (currentState == 4)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "c")
                    {
                        currentState = 5;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 72;
                    }
                }
                #endregion

                #region State 5
                if (currentState == 5)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "t")
                    {
                        currentState = 6;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 72;
                    }
                }
                #endregion

                #region State 6 - Return SELECT
                else if (currentState == 6)
                {
                    component = LexicalComponent.CreateSymbol(Category.Select, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 7
                else if (currentState == 7)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "a")
                    {
                        currentState = 8;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 81;
                    }
                }
                #endregion

                #region State 8
                else if (currentState == 8)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "m")
                    {
                        currentState = 9;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 81;
                    }
                }
                #endregion

                #region State 9
                else if (currentState == 9)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsUnderscore())
                    {
                        currentState = 10;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 81;
                    }
                }
                #endregion

                #region State 10
                else if (currentState == 10)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsLetterOrDigit() || CurrentCharacterIsUnderscore())
                    {
                        currentState = 10;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 11;
                    }
                }
                #endregion

                #region State 11 - Return Field
                else if (currentState == 11)
                {
                    MovePointerBackward();

                    component = LexicalComponent.CreateSymbol(Category.Field, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 12
                else if (currentState == 12)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "r")
                    {
                        currentState = 13;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 73;
                    }
                }
                #endregion

                #region State 13
                else if (currentState == 13)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "o")
                    {
                        currentState = 14;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 73;
                    }
                }
                #endregion

                #region State 14
                else if (currentState == 14)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "m")
                    {
                        currentState = 15;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 73;
                    }
                }
                #endregion

                #region State 15 - Return From
                else if (currentState == 15)
                {
                    component = LexicalComponent.CreateSymbol(Category.From, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 16
                else if (currentState == 16)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "a")
                    {
                        currentState = 17;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 80;
                    }
                }
                #endregion

                #region State 17
                else if (currentState == 17)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "b")
                    {
                        currentState = 18;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 80;
                    }
                }
                #endregion

                #region State 18
                else if (currentState == 18)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsUnderscore())
                    {
                        currentState = 19;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 80;
                    }
                }
                #endregion

                #region State 19
                else if (currentState == 19)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsLetterOrDigit() || CurrentCharacterIsUnderscore())
                    {
                        currentState = 19;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 20;
                    }
                }
                #endregion

                #region State 20 - Return Table
                else if (currentState == 20)
                {
                    MovePointerBackward();
                    component = LexicalComponent.CreateSymbol(Category.Table, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 22 - Return Equal to
                else if (currentState == 22)
                {
                    component = LexicalComponent.CreateSymbol(Category.EqualTo, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 24
                else if (currentState == 24)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "h")
                    {
                        currentState = 25;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 74;
                    }
                }
                #endregion

                #region State 25
                else if (currentState == 25)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "e")
                    {
                        currentState = 26;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 74;
                    }
                }
                #endregion

                #region State 26
                else if (currentState == 26)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "r")
                    {
                        currentState = 27;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 74;
                    }
                }
                #endregion

                #region State 27
                else if (currentState == 27)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "e")
                    {
                        currentState = 28;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 74;
                    }
                }
                #endregion

                #region State 28 - Return Where
                else if (currentState == 28)
                {
                    component = LexicalComponent.CreateSymbol(Category.Where, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 30
                else if (currentState == 30)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsSinglequote())
                    {
                        currentState = 31;
                        Concatenate();
                    }
                    else if (!CurrentCharacterIsEndOfFile() || !CurrentCharacterIsEndOfLine())
                    {
                        currentState = 30;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsEndOfLine())
                    {
                        currentState = 69;
                    }
                    else
                    {
                        currentState = 70;
                    }
                }
                #endregion

                #region State 31 - Return Literal
                else if (currentState == 31)
                {
                    component = LexicalComponent.CreateSymbol(Category.Literal, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 32 - Return End of File
                else if (currentState == 32)
                {
                    component = LexicalComponent.CreateSymbol(Category.EndOfFile, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 33
                else if (currentState == 33)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "n")
                    {
                        currentState = 34;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "s")
                    {
                        currentState = 35;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 75;
                    }
                }
                #endregion

                #region State 34
                else if (currentState == 34)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "d")
                    {
                        currentState = 37;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 75;
                    }
                }
                #endregion

                #region State 35
                else if (currentState == 35)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "c")
                    {
                        currentState = 36;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 76;
                    }
                }
                #endregion

                #region State 36 - Return ASC
                else if (currentState == 36)
                {
                    component = LexicalComponent.CreateSymbol(Category.Asc, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 37 - Return AND
                else if (currentState == 37)
                {
                    component = LexicalComponent.CreateSymbol(Category.And, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 38
                else if (currentState == 38)
                {
                    LoadNewLine();
                    currentState = 0;
                }
                #endregion

                #region State 39
                else if (currentState == 39)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsEqual())
                    {
                        currentState = 63;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 64;
                    }
                }
                #endregion

                #region State 40
                else if (currentState == 40)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "e")
                    {
                        currentState = 41;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 77;
                    }
                }
                #endregion

                #region State 41
                else if (currentState == 41)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "s")
                    {
                        currentState = 42;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 77;
                    }
                }
                #endregion

                #region State 42
                else if (currentState == 42)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "c")
                    {
                        currentState = 43;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 77;
                    }
                }
                #endregion

                #region State 43 - Return DESC
                else if (currentState == 43)
                {
                    component = LexicalComponent.CreateSymbol(Category.Desc, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    MasterTable.Add(component);
                    continueAnalysis = false;
                }
                #endregion

                #region State 44
                else if (currentState == 44)
                {

                }
                #endregion

                #region State 83 - Return Decimal
                else if (currentState == 83)
                {
                    component = LexicalComponent.CreateDummy(Category.Decimal, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
                    var error = Error.CreateLexicalError(
                        _lexeme,
                        _currentLineNumber,
                        _pointer - _lexeme.Length,
                        _pointer - 1,
                        "Invalid decimal", "Invalid decimal number", "Use a dot");

                    MasterTable.Add(component);
                    ErrorHandler.ErrorHandler.Report(error);
                    continueAnalysis = false;
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
