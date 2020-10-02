using System.Collections.Generic;
using System.Linq;

namespace Compiler.SymbolsTable
{
    public class SymbolsTable
    {
        private static Dictionary<string, IEnumerable<LexicalComponent>> _symbolsTable = new Dictionary<string, IEnumerable<LexicalComponent>>();

        public static void Add(LexicalComponent component)
        {
            if (component != null && component.ComponentType == ComponentType.Symbol)
            {
                ObtainSymbol(component.Lexeme).Add(component);
            }
        }

        public static List<LexicalComponent> ObtainSymbol(string lexeme)
        {
            if (_symbolsTable.ContainsKey(lexeme))
            {
                _symbolsTable.Add(lexeme, new List<LexicalComponent>());
            }
            return _symbolsTable[lexeme].ToList();
        }

        public static List<LexicalComponent> ObtainAllSymbols() => _symbolsTable.Values.SelectMany(component => component).ToList();

        public static void Clear() => _symbolsTable.Clear();
    }
}
