using System.Collections.Generic;
using System.Linq;

namespace Compiler.SymbolsTable
{
    public sealed class LiteralsTable
    {
        private static readonly Dictionary<string, List<LexicalComponent>> SymbolsTable = new Dictionary<string, List<LexicalComponent>>();

        public static void Add(LexicalComponent component)
        {
            if (component == null || component.ComponentType != ComponentType.Literal) return;
            if (SymbolsTable.ContainsKey(component.Lexeme))
            {
                SymbolsTable[component.Lexeme].Add(component);
            }
            else
            {
                SymbolsTable.Add(component.Lexeme, new List<LexicalComponent> { component });
            }
        }

        public static LexicalComponent CheckLiteral(LexicalComponent component)
        {
            if (component != null && component.Category == Category.Literal)
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

        public static List<LexicalComponent> ObtainAllSymbols() => SymbolsTable.Values.SelectMany(component => component).ToList();

        public static void Clear() => SymbolsTable.Clear();
    }
}
