namespace Compiler.Cache
{
    public class Linea
    {
        public uint Numero { get; set; }
        public string Contenido { get; set; }

        private Linea(uint numero, string contenido)
        {
            Numero = numero;
            Contenido = contenido ?? string.Empty;
        }

        public static Linea Crear(uint numero, string contenido)
        {
            return new Linea(numero, contenido);
        }
    }
}
