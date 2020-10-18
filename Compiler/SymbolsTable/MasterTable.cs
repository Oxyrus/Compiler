using System;
using System.Collections.Generic;

namespace Compiler.SymbolsTable
{
    public class MasterTable
    {
        public static void Add(LexicalComponent component)
        {
            if (component != null)
            {
                component = ReservedKeywordsTable.CheckReservedKeyword(component);
                component = LiteralsTable.CheckLiteral(component);
                switch (component.ComponentType)
                {
                    case ComponentType.Symbol:
                        SymbolsTable.Add(component);
                        break;
                    case ComponentType.ReservedKeyword:
                        ReservedKeywordsTable.Add(component);
                        break;
                    case ComponentType.Dummy:
                        DummiesTable.Add(component);
                        break;
                    case ComponentType.Literal:
                        LiteralsTable.Add(component);
                        break;
                    default:
                        throw new Exception("Unsupported lexical component type");
                }
            }
        }

        public static List<LexicalComponent> ObtainComponent(ComponentType componentType)
        {
            return componentType switch
            {
                ComponentType.Symbol => SymbolsTable.ObtainAllSymbols(),
                ComponentType.ReservedKeyword => ReservedKeywordsTable.ObtainAllSymbols(),
                ComponentType.Dummy => DummiesTable.ObtainAllSymbols(),
                ComponentType.Literal => LiteralsTable.ObtainAllSymbols(),
                _ => throw new Exception("Unsupported lexical component type")
            };
        }

        public static void Clear()
        {
            SymbolsTable.Clear();
            ReservedKeywordsTable.Clear();
            DummiesTable.Clear();
            LiteralsTable.Clear();
        }
    }
}
