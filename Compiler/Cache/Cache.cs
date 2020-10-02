using System.Collections.Generic;
using System.Linq;

namespace Compiler.Cache
{
    public class Cache
    {
        public static List<Linea> Lineas { get; } = new List<Linea>();

        public static void Limpiar()
        {
            Lineas.Clear();
        }

        public static void Poblar(List<Linea> lineas)
        {
            Limpiar();

            if (lineas != null && lineas.Any())
            {
                Lineas.AddRange(lineas);
            }
        }

        public static void Poblar(string contenido)
        {
            if (!string.IsNullOrEmpty(contenido))
            {
                var numeroLinea = Lineas.Count + 1;

                Lineas.Add(Linea.Crear(numeroLinea, contenido));
            }
        }

        public static Linea ObtenerLinea(uint numeroLinea)
        {
            Linea lineaRetorno;

            if (ExisteLinea(numeroLinea))
            {
                lineaRetorno = Lineas[(int)numeroLinea - 1];
            }
            else
            {
                lineaRetorno = Linea.Crear(Lineas.Count + 1, "@EOF@");
            }

            return lineaRetorno;
        }

        public static bool ExisteLinea(uint numeroLinea)
        {
            return numeroLinea <= Lineas.Count;
        }
    }
}
