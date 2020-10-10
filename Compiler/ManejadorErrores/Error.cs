namespace Compiler.ManejadorErrores
{
    public class Error
    {
        private Error(string lexema, int numeroLinea, int posicionInicial, int posicionFinal, string falla, string causa, string solucion, TipoError tipo)
        {
            Lexema = lexema ?? "";
            NumeroLinea = numeroLinea;
            PosicionInicial = posicionInicial;
            PosicionFinal = posicionFinal;
            Falla = falla;
            Causa = causa;
            Solucion = solucion;
            Tipo = tipo;
        }

        public static Error CrearErrorLexico(
            string lexema,
            int numeroLinea,
            int posicionInicial,
            int posicionFinal,
            string falla,
            string causa,
            string solucion)
        {
            return new Error(lexema, numeroLinea, posicionInicial, posicionFinal, falla, causa, solucion, TipoError.Lexico);
        }

        public static Error CrearErrorSemantico(
            string lexema,
            int numeroLinea,
            int posicionInicial,
            int posicionFinal,
            string falla,
            string causa,
            string solucion)
        {
            return new Error(lexema, numeroLinea, posicionInicial, posicionFinal, falla, causa, solucion, TipoError.Semantico);
        }

        public static Error CrearErrorSintactico(
            string lexema,
            int numeroLinea,
            int posicionInicial,
            int posicionFinal,
            string falla,
            string causa,
            string solucion)
        {
            return new Error(lexema, numeroLinea, posicionInicial, posicionFinal, falla, causa, solucion, TipoError.Sintactico);
        }

        public string Lexema { get; private set; }
        public int NumeroLinea { get; private set; }
        public int PosicionInicial { get; private set; }
        public int PosicionFinal { get; private set; }
        public string Falla { get; private set; }
        public string Causa { get; private set; }
        public string Solucion { get; private set; }
        public TipoError Tipo { get; private set; }
    }
}
