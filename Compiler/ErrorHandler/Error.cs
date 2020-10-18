namespace Compiler.ErrorHandler
{
    public sealed class Error
    {
        private Error(
            string lexeme,
            int lineNumber,
            int initialPosition,
            int finalPosition,
            string failure,
            string cause,
            string solution,
            ErrorType errorType)
        {
            Lexeme = lexeme;
            LineNumber = lineNumber;
            InitialPosition = initialPosition;
            FinalPosition = finalPosition;
            Failure = failure;
            Cause = cause;
            Solution = solution;
            ErrorType = errorType;
        }

        public static Error CreateLexicalError(
            string lexeme,
            int lineNumber,
            int initialPosition,
            int finalPosition,
            string failure,
            string cause,
            string solution)
        {
            return new Error(lexeme, lineNumber, initialPosition, finalPosition, failure, cause, solution, ErrorType.Lexical);
        }

        public static Error CreateSemanticError(
            string lexeme,
            int lineNumber,
            int initialPosition,
            int finalPosition,
            string failure,
            string cause,
            string solution)
        {
            return new Error(lexeme, lineNumber, initialPosition, finalPosition, failure, cause, solution, ErrorType.Semantic);
        }

        public static Error CreateSyntacticError(
            string lexeme,
            int lineNumber,
            int initialPosition,
            int finalPosition,
            string failure,
            string cause,
            string solution)
        {
            return new Error(lexeme, lineNumber, initialPosition, finalPosition, failure, cause, solution, ErrorType.Syntactic);
        }

        public string Lexeme { get; private set; }

        public int LineNumber { get; private set; }

        public int InitialPosition { get; private set; }

        public int FinalPosition { get; private set; }

        public string Failure { get; private set; }

        public string Cause { get; private set; }

        public string Solution { get; private set; }

        public ErrorType ErrorType { get; private set; }
    }
}
