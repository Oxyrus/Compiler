namespace Compiler.Cache
{
    public class Linea
    {
        public int Numero { get; set; }
        public string Contenido { get; set; }

        private Linea(int numero, string contenido)
        {
            Numero = numero;
            Contenido = contenido ?? string.Empty;
        }

        public static Linea Crear(int numero, string contenido)
        {
            return new Linea(numero, contenido);
        }
    }
}
