using Compiler.Cache;
using Compiler.TablaSimbolos;

namespace Compiler.AnalizadorLexico
{
    public class AnalisisLexico
    {
        private uint _numeroLineaActual;
        private int _puntero;
        private string _caracterActual;
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

        public ComponenteLexico FormatComponente()
        {
            var componente = new ComponenteLexico();
            var lexema = "";
            var estadoActual = 0;
            var continuarAnalisis = true;

            while (continuarAnalisis)
            {
                if (estadoActual == 0)
                {
                    LeerSiguienteCaracter();
                    DevorarEspacios();
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
