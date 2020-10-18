using System;
using System.Text;

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

        public static LexicalComponent CreateDummy(Category category, string lexeme, int lineNumber, int initialPosition, int finalPosition)
        {
            return new LexicalComponent(category, lexeme, lineNumber, initialPosition, finalPosition, ComponentType.Dummy);
        }

        public static LexicalComponent CreateReservedKeyword(Category category, string lexeme)
        {
            return new LexicalComponent(category, lexeme, -1, -1, -1, ComponentType.ReservedKeyword);
        }

        public static LexicalComponent CreateReservedKeyword(Category category, string lexeme, int lineNumber, int initialPosition, int finalPosition)
        {
            return new LexicalComponent(category, lexeme, lineNumber, initialPosition, finalPosition, ComponentType.ReservedKeyword);
        }

        public static LexicalComponent CreateLiteral(Category category, string lexeme, int lineNumber, int initialPosition, int finalPosition)
        {
            return new LexicalComponent(category, lexeme, lineNumber, initialPosition, finalPosition, ComponentType.Literal);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Component type: ").Append(ComponentType.ToString()).Append("\n");
            sb.Append("Category: ").Append(Category.ToString()).Append("\n");
            sb.Append("Lexeme: ").Append(Lexeme.ToString()).Append("\n");
            sb.Append("Line number: ").Append(LineNumber.ToString()).Append("\n");
            sb.Append("Initial position: ").Append(InitialPosition.ToString()).Append("\n");
            sb.Append("Final position: ").Append(FinalPosition.ToString()).Append("\n");

            return sb.ToString();
        }
    }
}
