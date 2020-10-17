using System.Collections.Generic;
using System.Linq;

namespace Compiler.TablaSimbolos
{
    public class TablaPalabrasReservadas
    {
        private static Dictionary<string, IEnumerable<ComponenteLexico>> _tablaPalabrasReservadas = new Dictionary<string, IEnumerable<ComponenteLexico>>();

        private static Dictionary<string, ComponenteLexico> _palabrasReservadasBase = new Dictionary<string, ComponenteLexico>();

        private static bool TablaInicializada = false;

        private static void Inicializar()
        {
            _palabrasReservadasBase.Add("AND", ComponenteLexico.CrearPalabraReservada(Categoria.And, "AND"));
            _palabrasReservadasBase.Add("OR", ComponenteLexico.CrearPalabraReservada(Categoria.Or, "OR"));
            _palabrasReservadasBase.Add("ORDER", ComponenteLexico.CrearPalabraReservada(Categoria.Order, "ORDER"));
            _palabrasReservadasBase.Add("BY", ComponenteLexico.CrearPalabraReservada(Categoria.By, "OR"));
            _palabrasReservadasBase.Add("ASC", ComponenteLexico.CrearPalabraReservada(Categoria.Asc, "ASC"));
            _palabrasReservadasBase.Add("DESC", ComponenteLexico.CrearPalabraReservada(Categoria.Desc, "DESC"));
            _palabrasReservadasBase.Add("DESC", ComponenteLexico.CrearPalabraReservada(Categoria.From, "FROM"));
            _palabrasReservadasBase.Add("WHERE", ComponenteLexico.CrearPalabraReservada(Categoria.Where, "WHERE"));
            _palabrasReservadasBase.Add("SELECT", ComponenteLexico.CrearPalabraReservada(Categoria.Select, "SELECT"));

            TablaInicializada = true;
        }

        public static ComponenteLexico ComprobarPalabraReservada(ComponenteLexico componenteLexico)
        {
            ComponenteLexico retorno = null;

            if (!TablaInicializada)
            {
                Inicializar();
            }

            if (_palabrasReservadasBase.ContainsKey(componenteLexico?.Lexema?.ToUpper()) && componenteLexico.Categoria == Categoria.Identificador)
            {
                retorno = ComponenteLexico.CrearPalabraReservada(
                    _palabrasReservadasBase[componenteLexico.Lexema.ToUpper()].Categoria,
                    componenteLexico.Lexema,
                    componenteLexico.NumeroLinea,
                    componenteLexico.PosicionInicial,
                    componenteLexico.PosicionFinal
                );
            }
            else
            {
                retorno = componenteLexico;
            }

            return retorno;
        }

        public static void Agregar(ComponenteLexico componente)
        {
            if (componente != null && componente.TipoComponente == TipoComponente.PalabraReservada)
            {
                ObtenerSimbolo(componente.Lexema).Add(componente);
            }
        }

        public static List<ComponenteLexico> ObtenerSimbolo(string lexema)
        {
            if (!_tablaPalabrasReservadas.ContainsKey(lexema))
            {
                _tablaPalabrasReservadas.Add(lexema, new List<ComponenteLexico>());
            }

            return _tablaPalabrasReservadas[lexema].ToList();
        }

        public static List<ComponenteLexico> ObtenerTodosSimbolos()
        {
            return _tablaPalabrasReservadas.Values.SelectMany(componente => componente).ToList();
        }

        public static void Limpiar()
        {
            _tablaPalabrasReservadas.Clear();
        }
    }
}
