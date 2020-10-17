using System.Collections.Generic;

namespace Compiler.TablaSimbolos
{
    public class TablaMaestra
    {
        public static void Agregar(ComponenteLexico componente)
        {
            if (componente != null)
            {
                componente = TablaPalabrasReservadas.ComprobarPalabraReservada(componente);
                componente = TablaLiterales.ComprobarLiteral(componente);
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
                    case TipoComponente.Literal:
                        TablaLiterales.Agregar(componente);
                        break;
                    default:
                        throw new System.Exception("Tipo de componente léxico no soportado");
                }
            }
        }

        public static List<ComponenteLexico> ObtenerComponente(TipoComponente tipoComponente)
        {
            return tipoComponente switch
            {
                TipoComponente.Simbolo => TablaSimbolos.ObtenerTodosSimbolos(),
                TipoComponente.PalabraReservada => TablaPalabrasReservadas.ObtenerTodosSimbolos(),
                TipoComponente.Dummy => TablaDummies.ObtenerTodosSimbolos(),
                TipoComponente.Literal => TablaLiterales.ObtenerTodosSimbolos(),
                _ => throw new System.Exception("Tipo de componente léxico no soportado"),
            };
        }

        public static void Limpiar()
        {
            TablaDummies.Limpiar();
            TablaSimbolos.Limpiar();
            TablaPalabrasReservadas.Limpiar();
            TablaLiterales.Limpiar();
        }
    }
}
