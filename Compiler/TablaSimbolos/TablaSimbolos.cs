using System.Collections.Generic;
using System.Linq;

namespace Compiler.TablaSimbolos
{
    public class TablaSimbolos
    {
        private static Dictionary<string, IEnumerable<ComponenteLexico>> _tablaSimbolos = new Dictionary<string, IEnumerable<ComponenteLexico>>();

        public static void Agregar(ComponenteLexico componente)
        {
            if (componente != null && componente.TipoComponente == TipoComponente.Simbolo)
            {
                ObtenerSimbolo(componente.Lexema).Add(componente);
            }
        }

        public static List<ComponenteLexico> ObtenerSimbolo(string lexema)
        {
            if (!_tablaSimbolos.ContainsKey(lexema))
            {
                _tablaSimbolos.Add(lexema, new List<ComponenteLexico>());
            }

            return _tablaSimbolos[lexema].ToList();
        }

        public static List<ComponenteLexico> ObtenerTodosSimbolos()
        {
            return _tablaSimbolos.Values.SelectMany(componente => componente).ToList();
        }

        public static void Limpiar()
        {
            _tablaSimbolos.Clear();
        }
    }
}
