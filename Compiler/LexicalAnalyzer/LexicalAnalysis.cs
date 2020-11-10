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

        public void LoadNewLine()
        {
            _currentLineNumber += 1;
            _currentLine = Cache.Cache.GetLine(_currentLineNumber);

            _currentCharacter = string.Empty;

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
                if (currentState == 0)
                {
                    if (!CurrentCharacterIsEndOfLine())
                    {
                        ReadNextCharacter();
                        IgnoreWhitespaces();
                    }

                    if (CurrentCharacterIsDigit())
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
                    else if (_currentCharacter.ToLower() == "t")
                    {
                        currentState = 26;
                        Concatenate();
                    }
                    else if (_currentCharacter.ToLower() == "c")
                    {
                        currentState = 33;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsLetter() && _currentCharacter != "t" && _currentCharacter != "c")
                    {
                        currentState = 1;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsComma())
                    {
                        currentState = 40;
                        Concatenate();
                    }
                    else if (CurrentCharacterIsEndOfLine())
                    {
                        currentState = 23;
                    }
                    else if (CurrentCharacterIsEndOfFile())
                    {
                        currentState = 24;
                    }
                    else
                    {
                        currentState = 25;
                    }
                }
                else if (currentState == 1)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsLetter())
                    {
                        currentState = 1;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 2;
                    }
                }
                else if (currentState == 2)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.Identifier);
                    continueAnalysis = false;
                }
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
                    }
                }
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
                else if (currentState == 6)
                {
                    component = GenerateComponent(Category.Decimal);
                    continueAnalysis = false;
                }
                else if (currentState == 7)
                {
                    component = GenerateComponent(Category.Integer);
                    continueAnalysis = false;
                }
                else if (currentState == 8)
                {
                    throw new Exception("Número decimal invalido");
                }
                else if (currentState == 9)
                {
                    ReadNextCharacter();

                    if (!CurrentCharacterIsSinglequote())
                    {
                        currentState = 9;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 11;
                        Concatenate();
                    }
                }
                else if (currentState == 11)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.Literal);
                    continueAnalysis = false;
                }
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
                else if (currentState == 13)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.GreaterThanOrEqualTo);
                    continueAnalysis = false;
                }
                else if (currentState == 14)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.GreaterThan);
                    continueAnalysis = false;
                }
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
                else if (currentState == 16)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.DifferentThan);
                    continueAnalysis = false;
                }
                else if (currentState == 17)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.LessThanOrEqualTo);
                    continueAnalysis = false;
                }
                else if (currentState == 18)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.LessThan);
                    continueAnalysis = false;
                }
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
                    }
                }
                else if (currentState == 20)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.DifferentThan);
                    continueAnalysis = false;
                }
                else if (currentState == 21)
                {
                    throw new Exception("!= invalido");
                }
                else if (currentState == 22)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.EqualTo);
                    continueAnalysis = false;
                }
                else if (currentState == 23)
                {
                    LoadNewLine();
                    currentState = 0;
                }
                else if (currentState == 24)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.EndOfFile);
                    continueAnalysis = false;
                }
                else if (currentState == 25)
                {
                    throw new Exception() ;
                }
                else if (currentState == 26)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "a")
                    {
                        currentState = 27;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 32;
                    }
                }
                else if (currentState == 27)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "b")
                    {
                        currentState = 28;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 32;
                    }
                }
                else if (currentState == 28)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsUnderscore())
                    {
                        currentState = 29;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 32;
                    }
                }
                else if (currentState == 29)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsLetterOrDigit() || CurrentCharacterIsUnderscore())
                    {
                        currentState = 30;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 32;
                    }
                }
                else if (currentState == 30)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsLetterOrDigit() || CurrentCharacterIsUnderscore())
                    {
                        currentState = 30;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 31;
                    }
                }
                else if (currentState == 31)
                {
                    component = GenerateComponent(Category.Table);
                    continueAnalysis = false;
                }
                else if (currentState == 32)
                {
                    throw new Exception("TABLA NO VALIDA");
                }
                else if (currentState == 33)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "a")
                    {
                        currentState = 34;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 39;
                    }
                }
                else if (currentState == 34)
                {
                    ReadNextCharacter();

                    if (_currentCharacter.ToLower() == "m")
                    {
                        currentState = 35;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 39;
                    }
                }
                else if (currentState == 35)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsUnderscore())
                    {
                        currentState = 36;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 39;
                    }
                }
                else if (currentState == 36)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsLetterOrDigit() || CurrentCharacterIsUnderscore())
                    {
                        currentState = 37;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 39;
                    }
                }
                else if (currentState == 37)
                {
                    ReadNextCharacter();

                    if (CurrentCharacterIsLetterOrDigit() || CurrentCharacterIsUnderscore())
                    {
                        currentState = 37;
                        Concatenate();
                    }
                    else
                    {
                        currentState = 38;
                    }
                }
                else if (currentState == 38)
                {
                    component = GenerateComponent(Category.Field);
                    continueAnalysis = false;
                }
                else if (currentState == 39)
                {
                    throw new Exception("Campo no valido");
                }
                else if (currentState == 40)
                {
                    component = GenerateComponentWithoutMovingPointer(Category.Separator);
                    continueAnalysis = false;
                }
                else
                {
                    throw new Exception("Símbolo no válido para el compilador");
                }
            }

            return component;
        }

        private LexicalComponent GenerateComponent(Category category)
        {
            MovePointerBackward();
            var component = LexicalComponent.CreateSymbol(category, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
            MasterTable.Add(component);
            return component;
        }

        private LexicalComponent GenerateComponentWithoutMovingPointer(Category category)
        {
            var component = LexicalComponent.CreateSymbol(category, _lexeme, _currentLineNumber, _pointer - _lexeme.Length, _pointer - 1);
            MasterTable.Add(component);
            return component;
        }

        private void ResetPointer() => _pointer = 1;

        private void MovePointerForward() => _pointer += 1;

        private void MovePointerBackward() => _pointer -= 1;
    }
}
