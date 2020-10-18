using Compiler.Cache;
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

        public LexicalAnalysis()
        {
            LoadNewLine();
        }

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

        #region Checks

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

        private void Concatenate() => _lexeme += _currentCharacter;

        #endregion

        public LexicalComponent BuildComponent()
        {
            // var component = new LexicalComponent();
            var lexeme = "";
            var currentState = 0;
            var continueAnalysis = true;

            while (continueAnalysis)
            {
                if (currentState == 0)
                {
                    ReadNextCharacter();
                    IgnoreWhitespaces();
                }
            }

            // return component;
            return null;
        }

        private void ResetPointer() => _pointer = 0;

        private void MovePointerForward() => _pointer += 1;

        private void MovePointerBackwards() => _pointer -= 1;
    }
}
