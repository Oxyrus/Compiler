namespace Compiler.TablaSimbolos
{
    public class ComponenteLexico
    {
        public Categoria Categoria { get; private set; }
        public string Lexema { get; private set; }
        public int NumeroLinea { get; private set; }
        public int PosicionInicial { get; private set; }
        public int PosicionFinal { get; private set; }
        public TipoComponente TipoComponente { get; }

        public ComponenteLexico(Categoria categoria, string lexema, int numeroLinea, int posicionInicial, int posicionFinal, TipoComponente tipoComponente)
        {
            Categoria = categoria;
            Lexema = lexema;
            NumeroLinea = numeroLinea;
            PosicionInicial = posicionInicial;
            PosicionFinal = posicionFinal;
            TipoComponente = tipoComponente;
        }

        public static ComponenteLexico CrearSimbolo(Categoria categoria, string lexema, int numeroLinea, int posicionInicial, int posicionFinal)
        {
            return new ComponenteLexico(categoria, lexema, numeroLinea, posicionInicial, posicionFinal, TipoComponente.Simbolo);
        }
    }
}
