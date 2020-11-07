using Compiler.AnalizadorLexico;
using Compiler.ManejadorErrores;
using Compiler.TablaSimbolos;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Compiler.AnalizadorSintactico
{
    public class AnalisisSintactico
    {
        private AnalisisLexico AnalisisLexico = new AnalisisLexico();
        private bool DepuracionHabilitada;
        private ComponenteLexico _componenteLexico;
        private string pilaLlamados = string.Empty;
        private Stack<double> pila = new Stack<double>();

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
                if (pila.Count == 1)
                {
                    MessageBox.Show("El resultado de la operacion es " + pila.Pop());
                }
                else
                {
                    MessageBox.Show("El programa esta bien escrito pero faltaron números por evaluar " + pila.Pop());
                }
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

        private void DepurarOperacion(string indentacion, double derecho, double izquierdo, string operacion)
        {
            pilaLlamados += indentacion + " REALIZANDO OPERACION " + derecho.ToString() + operacion + derecho.ToString() + "\n";
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
                var derecha = pila.Pop();
                var izquierda = pila.Pop();

                DepurarOperacion(indentacionProximoNivel, derecha, izquierda, "+");

                pila.Push(izquierda + derecha);
            }
            else if (_componenteLexico.Categoria == Categoria.Resta)
            {
                PedirComponente();
                Expresion(indentacionProximoNivel);
                var derecha = pila.Pop();
                var izquierda = pila.Pop();

                DepurarOperacion(indentacionProximoNivel, derecha, izquierda, "-");

                pila.Push(izquierda - derecha);
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

        // <TerminoPrima> := * <Termino> {push{{pop-2}*{pop-1}} | / <Termino> {push{{pop-2}/{pop-1}} | ??
        private void TerminoPrima(string indentacion)
        {
            DepurarEntrada(indentacion, "<TerminoPrima>");
            var indentacionProximoNivel = indentacion + "..";
            if (_componenteLexico.Categoria == Categoria.Multiplicacion)
            {
                PedirComponente();
                Termino(indentacionProximoNivel);
                var derecho = pila.Pop();
                var izquierdo = pila.Pop();

                DepurarOperacion(indentacionProximoNivel, derecho, izquierdo, "*");

                pila.Push(izquierdo * derecho);
            }
            else if (_componenteLexico.Categoria == Categoria.Division)
            {
                PedirComponente();
                Termino(indentacionProximoNivel);
                var derecho = pila.Pop();
                var izquierdo = pila.Pop();
                if (derecho == 0)
                {
                    var error = Error.CrearErrorSemantico(
                        _componenteLexico.Lexema,
                        _componenteLexico.NumeroLinea,
                        _componenteLexico.PosicionInicial,
                        _componenteLexico.PosicionFinal,
                        "Division por cero",
                        "Lei \"" + _componenteLexico.Lexema + "\"",
                        "asegurese de que el denominador no sea cero");

                    GestorErrores.Reportar(error);

                    derecho = 1;
                }

                DepurarOperacion(indentacionProximoNivel, derecho, izquierdo, "/");

                pila.Push(izquierdo / derecho);
            }
            DepurarSalida(indentacion, "<TerminoPrima>");
        }

        private void Factor(string indentacion)
        {
            DepurarEntrada(indentacion, "<TerminoPrima>");
            var indentacionProximoNivel = indentacion + "..";
            if (_componenteLexico.Categoria == Categoria.NumeroEntero)
            {
                pila.Push(Convert.ToDouble(_componenteLexico.Lexema));
                PedirComponente();
            }
            else if (_componenteLexico.Categoria == Categoria.NumeroDecimal)
            {
                pila.Push(Convert.ToDouble(_componenteLexico.Lexema));
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
