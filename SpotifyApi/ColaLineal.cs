using System;

namespace SpotifyApp
{
    public class NodoCola
    {
        public Cancion Dato { get; set; }
        public NodoCola? Siguiente { get; set; }

        public NodoCola(Cancion cancion)
        {
            Dato = cancion;
            Siguiente = null;
        }
    }

    public class ColaLineal
    {
        private NodoCola? _frente; // El primero en la fila
        private NodoCola? _fin;    // El último en la fila

        public ColaLineal()
        {
            _frente = null;
            _fin = null;
        }

        // Encolar (Poner al final de la fila)
        public void Encolar(Cancion cancion)
        {
            NodoCola nuevoNodo = new NodoCola(cancion);
            if (_fin == null)
            {
                _frente = nuevoNodo;
                _fin = nuevoNodo;
            }
            else
            {
                _fin.Siguiente = nuevoNodo;
                _fin = nuevoNodo;
            }
            Console.WriteLine($"[Encolada] {cancion.Titulo}");
        }

        // Desencolar (Sacar al primero de la fila para reproducirlo)
        public Cancion? Desencolar()
        {
            if (_frente == null)
            {
                Console.WriteLine("La cola está vacía.");
                return null;
            }

            Cancion cancionReproducir = _frente.Dato;
            _frente = _frente.Siguiente;

            if (_frente == null)
                _fin = null; // Si se vació la cola

            return cancionReproducir;
        }

        public void Mostrar()
        {
            if (_frente == null)
            {
                Console.WriteLine("(Cola vacía)");
                return;
            }
            NodoCola? actual = _frente;
            while (actual != null)
            {
                Console.WriteLine($"- {actual.Dato.Titulo} (por {actual.Dato.Artista})");
                actual = actual.Siguiente;
            }
        }
    }
}