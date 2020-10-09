﻿using Compiler.Cache;
using Compiler.TablaSimbolos;
using System;

namespace Compiler.AnalizadorLexico
{
    public class AnalisisLexico
    {
        private int _numeroLineaActual;
        private int _puntero;
        private string _caracterActual;
        private string _lexema = "";
        private Linea _lineaActual;

        private void CargarNuevaLinea()
        {
            _numeroLineaActual += 1;
            _lineaActual = Cache.Cache.ObtenerLinea(_numeroLineaActual);

            if (_lineaActual.Contenido == "@EOF@")
            {
                _numeroLineaActual = _lineaActual.Numero;
            }

            ResetearPuntero();
        }

        private void LeerSiguienteCaracter()
        {
            if (_lineaActual.Contenido == "@EOF@")
            {
                _caracterActual = _lineaActual.Contenido;
            }
            else if (_puntero > _lineaActual.Contenido.Length)
            {
                _caracterActual = "@FL@";
            }
            else
            {
                _caracterActual = _lineaActual.Contenido.Substring(_puntero, 1);
                AvanzarPuntero();
            }
        }

        private void DevorarEspacios()
        {
            while (_caracterActual == " ")
            {
                LeerSiguienteCaracter();
            }
        }

        private bool CaracterActualEsLetra() => Char.IsLetter(_caracterActual, 0);

        private bool CaracterActualEsDigito() => Char.IsDigit(_caracterActual, 0);

        private bool CaracterActualEsLetraODigito() => Char.IsLetterOrDigit(_caracterActual, 0);

        private bool CaracterActualEsSignoPesos() => "$".Equals(_caracterActual);

        private bool CaracterActualEsGuionBajo() => "_".Equals(_caracterActual);

        private void Concatenar() => _lexema += _caracterActual;

        private bool CaracterActualEsSuma() => "+".Equals(_caracterActual);

        private bool CaracterActualEsResta() => "-".Equals(_caracterActual);

        private bool CaracterActualEsMultiplicacion() => "*".Equals(_caracterActual);

        private bool CaracterActualEsDivision() => "/".Equals(_caracterActual);

        private bool CaracterActualEsModulo() => "%".Equals(_caracterActual);

        private bool CaracterActualEsParentesisAbre() => "(".Equals(_caracterActual);

        private bool CaracterActualEsParentesisCierra() => ")".Equals(_caracterActual);

        private bool CaracterActualEsIgual() => "=".Equals(_caracterActual);

        private bool CaracterActualEsMenorQue() => "<".Equals(_caracterActual);

        private bool CaracterActualEsMayorQue() => ">".Equals(_caracterActual);

        private bool CaracterActualEsDosPuntos() => ":".Equals(_caracterActual);

        private bool CaracterActualEsSignoExclamacion() => "!".Equals(_caracterActual);

        private bool CaracterActualEsFinDeLinea() => "@FL@".Equals(_caracterActual);

        private bool CaracterActualEsFinDeArchivo() => "@EOF@".Equals(_caracterActual);

        private bool CaracterActualEsComa() => ",".Equals(_caracterActual);

        public ComponenteLexico FormarComponente()
        {
            ComponenteLexico componente = null;
            _lexema = "";
            var estadoActual = 0;
            var continuarAnalisis = true;

            while (continuarAnalisis)
            {
                if (estadoActual == 0)
                {
                    LeerSiguienteCaracter();
                    DevorarEspacios();

                    if (CaracterActualEsLetra() || CaracterActualEsGuionBajo() || CaracterActualEsSignoPesos())
                    {
                        estadoActual = 4;
                        Concatenar();
                    }
                    else if (CaracterActualEsDigito())
                    {
                        estadoActual = 1;
                        Concatenar();
                    }
                    else if (CaracterActualEsSuma())
                    {
                        estadoActual = 5;
                        Concatenar();
                    }
                    else if (CaracterActualEsResta())
                    {
                        estadoActual = 6;
                        Concatenar();
                    }
                    else if (CaracterActualEsMultiplicacion())
                    {
                        estadoActual = 7;
                        Concatenar();
                    }
                    else if (CaracterActualEsMultiplicacion())
                    {
                        estadoActual = 7;
                        Concatenar();
                    }
                    else if (CaracterActualEsDivision())
                    {
                        estadoActual = 8;
                        Concatenar();
                    }
                    else if (CaracterActualEsModulo())
                    {
                        estadoActual = 9;
                        Concatenar();
                    }
                    else if (CaracterActualEsParentesisAbre())
                    {
                        estadoActual = 10;
                        Concatenar();
                    }
                    else if (CaracterActualEsParentesisCierra())
                    {
                        estadoActual = 11;
                        Concatenar();
                    }
                    else if (CaracterActualEsIgual())
                    {
                        estadoActual = 19;
                        Concatenar();
                    }
                    else if (CaracterActualEsMenorQue())
                    {
                        estadoActual = 20;
                        Concatenar();
                    }
                    else if (CaracterActualEsMayorQue())
                    {
                        estadoActual = 21;
                        Concatenar();
                    }
                    else if (CaracterActualEsDosPuntos())
                    {
                        estadoActual = 22;
                        Concatenar();
                    }
                    else if (CaracterActualEsSignoExclamacion())
                    {
                        estadoActual = 30;
                        Concatenar();
                    }
                    else if (CaracterActualEsFinDeLinea())
                    {
                        estadoActual = 13;
                    }
                    else if (CaracterActualEsFinDeArchivo())
                    {
                        estadoActual = 12;
                        Concatenar();
                    }
                    else
                    {
                        estadoActual = 18;
                    }
                }
                else if (estadoActual == 4)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsLetraODigito() || CaracterActualEsSignoPesos() || CaracterActualEsGuionBajo())
                    {
                        estadoActual = 4;
                        Concatenar();
                    }
                    else
                    {
                        estadoActual = 16;
                    }
                }
                else if (estadoActual == 1)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsDigito())
                    {
                        estadoActual = 1;
                        Concatenar();
                    }
                    else if (CaracterActualEsComa())
                    {
                        estadoActual = 2;
                        Concatenar();
                    }
                    else
                    {
                        estadoActual = 14;
                    }
                }
                else if (estadoActual == 2)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsDigito())
                    {
                        estadoActual = 3;
                        Concatenar();
                    }
                    else
                    {
                        estadoActual = 17;
                    }
                }
                else if (estadoActual == 3)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsDigito())
                    {
                        estadoActual = 3;
                        Concatenar();
                    }
                    else
                    {
                        estadoActual = 15;
                    }
                }
                else if (estadoActual == 8)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsMultiplicacion())
                    {
                        estadoActual = 34;
                        Concatenar();
                    }
                    else if (CaracterActualEsDivision())
                    {
                        estadoActual = 36;
                        Concatenar();
                    }
                    else
                    {
                        estadoActual = 33;
                    }
                }
                else if (estadoActual == 34)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsMultiplicacion())
                    {
                        estadoActual = 35;
                        Concatenar();
                    }
                    else
                    {
                        estadoActual = 34;
                        Concatenar();
                    }
                }
                else if (estadoActual == 35)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsMultiplicacion()) {
                        estadoActual = 35;
                        Concatenar();
                    }
                    else if (CaracterActualEsDivision())
                    {
                        estadoActual = 0;
                        Concatenar();
                    }
                    else
                    {
                        estadoActual = 34;
                        Concatenar();
                    }
                }
                else if (estadoActual == 36)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsFinDeLinea())
                    {
                        estadoActual = 13;
                    }
                    else
                    {
                        estadoActual = 36;
                        Concatenar();
                    }
                }
                else if (estadoActual == 13)
                {
                    CargarNuevaLinea();

                    estadoActual = 0;
                }
                else if (estadoActual == 16)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.Identificador, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 14)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.NumeroEntero, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 14)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.NumeroDecimal, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 5)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.Suma, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 6)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.Resta, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 7)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.Multiplicacion, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 33)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.Division, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 9)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.Modulo, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 10)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.ParentesisAbre, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 11)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.ParentesisCierra, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 12)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.FinDeArchivo, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 19)
                {
                    DevolverPuntero();
                    componente = ComponenteLexico.CrearSimbolo(Categoria.IgualQue, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                    TablaMaestra.Agregar(componente);
                    continuarAnalisis = false;
                }
                else if (estadoActual == 20)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsMayorQue())
                    {
                        DevolverPuntero();
                        componente = ComponenteLexico.CrearSimbolo(Categoria.DiferenteQue, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                        TablaMaestra.Agregar(componente);
                        continuarAnalisis = false;
                    }
                    else if (CaracterActualEsIgual())
                    {
                        DevolverPuntero();
                        componente = ComponenteLexico.CrearSimbolo(Categoria.MenorOIgualQue, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                        TablaMaestra.Agregar(componente);
                        continuarAnalisis = false;
                    }
                    else
                    {
                        DevolverPuntero();
                        componente = ComponenteLexico.CrearSimbolo(Categoria.MenorQue, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                        TablaMaestra.Agregar(componente);
                        continuarAnalisis = false;
                    }
                }
                else if (estadoActual == 21)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsIgual())
                    {
                        DevolverPuntero();
                        componente = ComponenteLexico.CrearSimbolo(Categoria.MayorOIgualQue, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                        TablaMaestra.Agregar(componente);
                        continuarAnalisis = false;
                    }
                    else
                    {
                        DevolverPuntero();
                        componente = ComponenteLexico.CrearSimbolo(Categoria.MayorQue, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                        TablaMaestra.Agregar(componente);
                        continuarAnalisis = false;
                    }
                }
                else if (estadoActual == 22)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsIgual())
                    {
                        DevolverPuntero();
                        componente = ComponenteLexico.CrearSimbolo(Categoria.Asignacion, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                        TablaMaestra.Agregar(componente);
                        continuarAnalisis = false;
                    }
                }
                else if (estadoActual == 30)
                {
                    LeerSiguienteCaracter();

                    if (CaracterActualEsIgual())
                    {
                        DevolverPuntero();
                        componente = ComponenteLexico.CrearSimbolo(Categoria.DiferenteQue, _lexema, _numeroLineaActual, _puntero - _lexema.Length, _puntero - 1);
                        TablaMaestra.Agregar(componente);
                        continuarAnalisis = false;
                    }
                }
            }

            return componente;
        }

        private void ResetearPuntero()
        {
            _puntero = 1;
        }

        private void AvanzarPuntero()
        {
            _puntero += 1;
        }

        private void DevolverPuntero()
        {
            _puntero -= 1;
        }
    }
}
