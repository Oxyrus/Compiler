using System;

namespace Compiler.SymbolsTable
{
    public class LexicalComponent
    {
        public LexicalComponent(
            Category category,
            string lexeme,
            int lineNumber,
            int initialPosition,
            int finalPosition,
            ComponentType componentType)
        {
            Category = category;
            Lexeme = lexeme ?? throw new ArgumentNullException(nameof(lexeme));
            LineNumber = lineNumber;
            InitialPosition = initialPosition;
            FinalPosition = finalPosition;
            ComponentType = componentType;
        }

        public Category Category { get; set; }

        public string Lexeme { get; set; }

        public int LineNumber { get; set; }

        public int InitialPosition { get; set; }

        public int FinalPosition { get; set; }

        public ComponentType ComponentType { get; }

        public static LexicalComponent CreateSymbol(Category category, string lexeme, int lineNumber, int initialPosition, int finalPosition)
        {
            return new LexicalComponent(category, lexeme, lineNumber, initialPosition, finalPosition, ComponentType.Symbol);
        }
    }
}
