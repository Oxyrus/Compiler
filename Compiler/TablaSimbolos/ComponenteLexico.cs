using System.Text;

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

        public static ComponenteLexico CrearDummy(Categoria categoria, string lexema, int numeroLinea, int posicionInicial, int posicionFinal)
        {
            return new ComponenteLexico(categoria, lexema, numeroLinea, posicionInicial, posicionFinal, TipoComponente.Dummy);
        }

        public static ComponenteLexico CrearPalabraReservada(Categoria categoria, string lexema)
        {
            return new ComponenteLexico(categoria, lexema, -1, -1, -1, TipoComponente.PalabraReservada);
        }

        public static ComponenteLexico CrearPalabraReservada(Categoria categoria, string lexema, int numeroLinea, int posicionInicial, int posicionFinal)
        {
            return new ComponenteLexico(categoria, lexema, numeroLinea, posicionInicial, posicionFinal, TipoComponente.PalabraReservada);
        }

        public static ComponenteLexico CrearLiteral(Categoria categoria, string lexema, int numeroLinea, int posicionInicial, int posicionFinal)
        {
            return new ComponenteLexico(categoria, lexema, numeroLinea, posicionInicial, posicionFinal, TipoComponente.Literal);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Tipo componente: ").Append(TipoComponente.ToString()).Append("\n");
            sb.Append("Categoria: ").Append(Categoria.ToString()).Append("\n");
            sb.Append("Lexema: ").Append(Lexema).Append("\n");
            sb.Append("Número línea: ").Append(NumeroLinea.ToString()).Append("\n");
            sb.Append("Posición inicial: ").Append(PosicionInicial.ToString()).Append("\n");
            sb.Append("Posición final: ").Append(PosicionFinal.ToString()).Append("\n");

            return sb.ToString();
        }
    }
}
