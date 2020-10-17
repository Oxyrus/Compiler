﻿using System.Collections.Generic;
using System.Linq;

namespace Compiler.TablaSimbolos
{
    public class TablaLiterales
    {
        private static Dictionary<string, List<ComponenteLexico>> _tablaSimbolos = new Dictionary<string, List<ComponenteLexico>>();

        public static void Agregar(ComponenteLexico componente)
        {

            if (componente != null && componente.TipoComponente == TipoComponente.Literal)
            {
                if (_tablaSimbolos.ContainsKey(componente.Lexema))
                {
                    _tablaSimbolos[componente.Lexema].Add(componente);
                }
                else
                {
                    _tablaSimbolos.Add(componente.Lexema, new List<ComponenteLexico> { componente });
                }
            }
        }

        public static ComponenteLexico ComprobarLiteral(ComponenteLexico componenteLexico)
        {
            ComponenteLexico retorno = null;

            if (componenteLexico != null && (componenteLexico.Categoria == Categoria.NumeroDecimal || componenteLexico.Categoria == Categoria.NumeroEntero))
            {
                retorno = ComponenteLexico.CrearLiteral(
                    componenteLexico.Categoria,
                    componenteLexico.Lexema,
                    componenteLexico.NumeroLinea,
                    componenteLexico.PosicionInicial,
                    componenteLexico.PosicionFinal
                );
            }
            else
            {
                retorno = componenteLexico;
            }

            return retorno;
        }

        public static List<ComponenteLexico> ObtenerSimbolo(string lexema)
        {
            if (!_tablaSimbolos.ContainsKey(lexema))
            {
                _tablaSimbolos.Add(lexema, new List<ComponenteLexico>());
            }

            return _tablaSimbolos[lexema].ToList();
        }

        public static List<ComponenteLexico> ObtenerTodosSimbolos()
        {
            return _tablaSimbolos.Values.SelectMany(componente => componente).ToList();
        }

        public static void Limpiar()
        {
            _tablaSimbolos.Clear();
        }
    }
}
