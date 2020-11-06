using System.Collections.Generic;
using System.Linq;

namespace Compiler.SymbolsTable
{
    public sealed class LiteralsTable
    {
        private static Dictionary<string, List<LexicalComponent>> _symbolsTable = new Dictionary<string, List<LexicalComponent>>();

        public static void Add(LexicalComponent component)
        {
            if (component != null && component.ComponentType == ComponentType.Literal)
            {
                if (_symbolsTable.ContainsKey(component.Lexeme))
                {
                    _symbolsTable[component.Lexeme].Add(component);
                }
                else
                {
                    _symbolsTable.Add(component.Lexeme, new List<LexicalComponent> { component });
                }
            }
        }

        public static LexicalComponent CheckLiteral(LexicalComponent component)
        {
            if (component != null && (component.Category == Category.Decimal || component.Category == Category.Integer))
            {
                return LexicalComponent.CreateLiteral(
                    component.Category,
                    component.Lexeme,
                    component.LineNumber,
                    component.InitialPosition,
                    component.FinalPosition);
            }
            return component;
        }

        public static List<LexicalComponent> ObtainSymbol(string lexeme)
        {
            if (!_symbolsTable.ContainsKey(lexeme))
            {
                _symbolsTable.Add(lexeme, new List<LexicalComponent>());
            }
            return _symbolsTable[lexeme].ToList();
        }

        public static List<LexicalComponent> ObtainAllSymbols() => _symbolsTable.Values.SelectMany(component => component).ToList();

        public static void Clear() => _symbolsTable.Clear();
    }
}
