using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.ManejadorErrores
{
    public class GestorErrores
    {
        private static Dictionary<TipoError, List<Error>> _errores = new Dictionary<TipoError, List<Error>>();

        public static List<Error> ObtenerErrores(TipoError tipoError)
        {
            if (!_errores.ContainsKey(tipoError))
            {
                _errores.Add(tipoError, new List<Error>());
            }

            return _errores[tipoError];
        }

        public static void Reportar(Error error)
        {
            if (error != null)
            {
                ObtenerErrores(error.Tipo).Add(error);
            }
        }

        public static bool HayErrores(TipoError tipoError) => ObtenerErrores(tipoError).Count > 0;

        public static bool HayErrores()
        {
            return HayErrores(TipoError.Lexico) || HayErrores(TipoError.Semantico) || HayErrores(TipoError.Sintactico);
        }

        public static List<Error> ObtenerTodosErrores()
        {
            return _errores.Values.SelectMany(error => error).ToList();
        }

        public static void Limpiar()
        {
            _errores.Clear();
        }
    }
}
