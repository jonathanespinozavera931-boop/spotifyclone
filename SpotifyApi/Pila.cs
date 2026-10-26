    using System;

namespace SpotifyApp
{
    public class NodoPila
    {
        public Cancion Dato { get; set; }
        public NodoPila? Siguiente { get; set; }

        public NodoPila(Cancion cancion)
        {
            Dato = cancion;
            Siguiente = null;
        }
    }

    public class Pila
    {
        private NodoPila? _tope;

        public Pila()
        {
            _tope = null;
        }

        // Push (Apilar)
        public void Apilar(Cancion cancion)
        {
            NodoPila nuevoNodo = new NodoPila(cancion);
            nuevoNodo.Siguiente = _tope;
            _tope = nuevoNodo;
            Console.WriteLine($"[Historial] Se guardó: {cancion.Titulo}");
        }

        // Pop (Desapilar)
        public Cancion? Desapilar()
        {
            if (_tope == null)
            {
                Console.WriteLine("[Historial] Vacío, no hay a dónde volver.");
                return null;
            }

            Cancion cancionEliminada = _tope.Dato;
            _tope = _tope.Siguiente;
            return cancionEliminada;
        }

        public void MostrarHistorial()
        {
            if (_tope == null)
            {
                Console.WriteLine("El historial está vacío.");
                return;
            }

            NodoPila? actual = _tope;
            int pos = 1;
            Console.WriteLine("--- HISTORIAL (Más reciente arriba) ---");
            while (actual != null)
            {
                Console.WriteLine($"{pos}. {actual.Dato.Titulo}");
                actual = actual.Siguiente;
                pos++;
            }
        }
    }
}