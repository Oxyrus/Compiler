using Compiler.AnalizadorLexico;
using Compiler.ManejadorErrores;
using Compiler.TablaSimbolos;
using System;
using System.Windows.Forms;

namespace Compiler.AnalizadorSintactico
{
    public class AnalisisSintactico
    {
        private AnalisisLexico AnalisisLexico = new AnalisisLexico();
        private bool DepuracionHabilitada;
        private ComponenteLexico _componenteLexico;
        private string pilaLlamados = string.Empty;

        public AnalisisSintactico()
        {
        }

        public void Analizar(bool depurar)
        {
            DepuracionHabilitada = depurar;
            pilaLlamados = string.Empty;
            PedirComponente();
            Expresion("..");

            if (GestorErrores.HayErrores())
            {
                MessageBox.Show("Hay errores de compilación, por favor verifique la consola de errores");
            }
            else if (_componenteLexico.Categoria == Categoria.FinDeArchivo)
            {
                MessageBox.Show("El programa compilo de forma satisfactoria");
            }
            else
            {
                MessageBox.Show("Aunque el programa compilo de manera satisfactoria, faltaron componentes por evaluar");
            }
        }

        private void PedirComponente()
        {
            _componenteLexico = AnalisisLexico.FormarComponente();
        }

        private void DepurarEntrada(string indentacion, string regla)
        {
            pilaLlamados += indentacion + " ENTRANDO A REGLA " + regla + " con lexema " + _componenteLexico.Lexema + " y categoria " + _componenteLexico.Categoria + "\n";
            ImprimirTraza();
        }

        private void DepurarSalida(string indentacion, string regla)
        {
            pilaLlamados += indentacion + " SALIENDO DE REGLA " + regla + "\n";
            ImprimirTraza();
        }

        private void ImprimirTraza()
        {
            if (DepuracionHabilitada)
            {
                MessageBox.Show(pilaLlamados);
            }
        }

        private void Expresion(string indentacion)
        {
            var indentacionProximoNivel = indentacion + "..";
            DepurarEntrada(indentacionProximoNivel, "<Expresion>");
            Termino(indentacionProximoNivel);
            ExpresionPrima(indentacionProximoNivel);
            DepurarSalida(indentacionProximoNivel, "<Expresion>");
        }

        private void ExpresionPrima(string indentacion)
        {
            DepurarEntrada(indentacion, "<ExpresionPrima>");
            var indentacionProximoNivel = indentacion + "..";
            if (_componenteLexico.Categoria == Categoria.Suma)
            {
                PedirComponente();
                Expresion(indentacionProximoNivel);
            }
            else if (_componenteLexico.Categoria == Categoria.Resta)
            {
                PedirComponente();
                Expresion(indentacionProximoNivel);
            }
            DepurarSalida(indentacion, "<ExpresionPrima>");
        }

        private void Termino(string indentacion)
        {
            DepurarEntrada(indentacion, "<Termino>");
            var indentacionProximoNivel = indentacion + "..";
            Factor(indentacionProximoNivel);
            TerminoPrima(indentacionProximoNivel);
            DepurarSalida(indentacion, "<Termino>");
        }

        private void TerminoPrima(string indentacion)
        {
            DepurarEntrada(indentacion, "<TerminoPrima>");
            var indentacionProximoNivel = indentacion + "..";
            if (_componenteLexico.Categoria == Categoria.Multiplicacion)
            {
                PedirComponente();
                Termino(indentacionProximoNivel);
            }
            else if (_componenteLexico.Categoria == Categoria.Division)
            {
                PedirComponente();
                Termino(indentacionProximoNivel);
            }
            DepurarSalida(indentacion, "<TerminoPrima>");
        }

        private void Factor(string indentacion)
        {
            DepurarEntrada(indentacion, "<TerminoPrima>");
            var indentacionProximoNivel = indentacion + "..";
            if (_componenteLexico.Categoria == Categoria.NumeroEntero)
            {
                PedirComponente();
            }
            else if (_componenteLexico.Categoria == Categoria.NumeroDecimal)
            {
                PedirComponente();
            }
            else if (_componenteLexico.Categoria == Categoria.ParentesisAbre)
            {
                PedirComponente();
                Expresion(indentacionProximoNivel);

                if (_componenteLexico.Categoria == Categoria.ParentesisCierra)
                {
                    PedirComponente();
                }
                else
                {
                    var error = Error.CrearErrorSintactico(
                        _componenteLexico.Lexema,
                        _componenteLexico.NumeroLinea,
                        _componenteLexico.PosicionInicial,
                        _componenteLexico.PosicionFinal,
                        "Esperaba parentésis que cierra",
                        "Leí " + _componenteLexico.Lexema,
                        "Asegúrese de cerrar el parentésis");

                    GestorErrores.Reportar(error);
                }
            }
            else
            {
                var error = Error.CrearErrorSintactico(
                    _componenteLexico.Lexema,
                    _componenteLexico.NumeroLinea,
                    _componenteLexico.PosicionInicial,
                    _componenteLexico.PosicionFinal,
                    "Componente no válido",
                    "Leí " + _componenteLexico.Lexema,
                    "Asegúrese que en esta posición se encuentra un número entero");

                GestorErrores.Reportar(error);

                throw new Exception("Se ha presentado un error de tipo sintáctico");
            }
            DepurarSalida(indentacion, "<Factor>");
        }
    }
}
