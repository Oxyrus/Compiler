using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Compiler.SymbolsTable
{
    public sealed class ReservedKeywordsTable
    {
        private static Dictionary<string, List<LexicalComponent>> _reservedKeywords = new Dictionary<string, List<LexicalComponent>>();

        private static Dictionary<string, LexicalComponent> _baseReservedKeywords = new Dictionary<string, LexicalComponent>();

        private static bool _tableInitialized = false;

        private static void Initialize()
        {
            _baseReservedKeywords.Add("AND", LexicalComponent.CreateReservedKeyword(Category.And, "AND"));
            _baseReservedKeywords.Add("OR", LexicalComponent.CreateReservedKeyword(Category.Or, "OR"));
            _baseReservedKeywords.Add("ORDER", LexicalComponent.CreateReservedKeyword(Category.Order, "ORDER"));
            _baseReservedKeywords.Add("BY", LexicalComponent.CreateReservedKeyword(Category.By, "BY"));
            _baseReservedKeywords.Add("ASC", LexicalComponent.CreateReservedKeyword(Category.Asc, "ASC"));
            _baseReservedKeywords.Add("DESC", LexicalComponent.CreateReservedKeyword(Category.Desc, "DESC"));
            _baseReservedKeywords.Add("FROM", LexicalComponent.CreateReservedKeyword(Category.From, "FROM"));
            _baseReservedKeywords.Add("WHERE", LexicalComponent.CreateReservedKeyword(Category.Where, "WHERE"));
            _baseReservedKeywords.Add("SELECT", LexicalComponent.CreateReservedKeyword(Category.Select, "SELECT"));

            _tableInitialized = true;
        }

        public static LexicalComponent CheckReservedKeyword(LexicalComponent component)
        {
            if (!_tableInitialized)
            {
                Initialize();
            }

            if (_baseReservedKeywords.ContainsKey(component?.Lexeme?.ToUpper()) && component.Category == Category.Identifier)
            {
                return LexicalComponent.CreateReservedKeyword(
                    _baseReservedKeywords[component.Lexeme.ToUpper()].Category,
                    component.Lexeme,
                    component.LineNumber,
                    component.InitialPosition,
                    component.FinalPosition);
            }

            return null;
        }

        public static void Add(LexicalComponent component)
        {
            if (component != null && component.ComponentType == ComponentType.ReservedKeyword)
            {
                if (_reservedKeywords.ContainsKey(component.Lexeme))
                {
                    _reservedKeywords[component.Lexeme].Add(component);
                }
                else
                {
                    _reservedKeywords.Add(component.Lexeme, new List<LexicalComponent> { component });
                }
            }
        }

        public static List<LexicalComponent> ObtainSymbol(string lexeme)
        {
            if (!_reservedKeywords.ContainsKey(lexeme))
            {
                _reservedKeywords.Add(lexeme, new List<LexicalComponent>());
            }
            return _reservedKeywords[lexeme].ToList();
        }

        public static List<LexicalComponent> ObtainAllSymbols() => _reservedKeywords.Values.SelectMany(component => component).ToList();

        public static void Clear() => _reservedKeywords.Clear();
    }
}
