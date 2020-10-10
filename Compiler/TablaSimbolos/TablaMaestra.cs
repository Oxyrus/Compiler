using System.Collections.Generic;

namespace Compiler.TablaSimbolos
{
    public class TablaMaestra
    {
        public static void Agregar(ComponenteLexico componente)
        {
            if (componente != null)
            {
                switch (componente.TipoComponente)
                {
                    case TipoComponente.Simbolo:
                        TablaSimbolos.Agregar(componente);
                        break;
                    case TipoComponente.PalabraReservada:
                        // Sincronizar con la tabla de palabras reservadas
                        break;
                    case TipoComponente.Dummy:
                        TablaDummies.Agregar(componente);
                        break;
                    default:
                        TablaSimbolos.Agregar(componente);
                        break;
                }
            }
        }

        public static List<ComponenteLexico> ObtenerComponente(TipoComponente tipoComponente)
        {
            return tipoComponente switch
            {
                TipoComponente.Simbolo => TablaSimbolos.ObtenerTodosSimbolos(),
                TipoComponente.PalabraReservada => TablaSimbolos.ObtenerTodosSimbolos(),
                TipoComponente.Dummy => TablaDummies.ObtenerTodosSimbolos(),
                _ => TablaSimbolos.ObtenerTodosSimbolos(),
            };
        }

        public static void Limpiar()
        {
            TablaDummies.Limpiar();
            TablaSimbolos.Limpiar();
        }
    }
}
