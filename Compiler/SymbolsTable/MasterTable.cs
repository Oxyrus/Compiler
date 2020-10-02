using System.Collections.Generic;

namespace Compiler.SymbolsTable
{
    public class MasterTable
    {
        public static void Add(LexicalComponent component)
        {
            if (component != null)
            {
                switch (component.ComponentType)
                {
                    case ComponentType.Symbol:
                        SymbolsTable.Add(component);
                        break;
                    case ComponentType.ReservedKeyword:
                        // Sync with the table of reserved keywords
                        break;
                    default:
                        SymbolsTable.Add(component);
                        break;
                }
            }
        }

        public static List<LexicalComponent> ObtainComponent(ComponentType componentType)
        {
            return componentType switch
            {
                ComponentType.Symbol => SymbolsTable.ObtainAllSymbols(),
                ComponentType.ReservedKeyword => SymbolsTable.ObtainAllSymbols(),
                _ => SymbolsTable.ObtainAllSymbols()
            };
        }

        public static void Clear() => SymbolsTable.Clear();
    }
}
