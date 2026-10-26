using System;

namespace SpotifyApp
{
    public class ColaCircular
    {
        private Cancion?[] _arreglo;
        private int _frente;
        private int _fin;
        private int _capacidad;
        private int _cantidad;

        public ColaCircular(int capacidad)
        {
            _capacidad = capacidad;
            _arreglo = new Cancion?[capacidad];
            _frente = 0;
            _fin = -1;
            _cantidad = 0;
        }

        public bool EstaLlena() => _cantidad == _capacidad;
        public bool EstaVacia() => _cantidad == 0;

        public void Encolar(Cancion cancion)
        {
            if (EstaLlena())
            {
                Console.WriteLine("[Error] La cola circular está llena.");
                return;
            }

            // Magia de la cola circular: el operador módulo (%) hace que vuelva a 0 al llegar al final
            _fin = (_fin + 1) % _capacidad;
            _arreglo[_fin] = cancion;
            _cantidad++;
            Console.WriteLine($"[Encolada] {cancion.Titulo}");
        }

        public Cancion? Desencolar()
        {
            if (EstaVacia())
            {
                Console.WriteLine("La cola está vacía.");
                return null;
            }

            Cancion cancion = _arreglo[_frente]!;
            _arreglo[_frente] = null; // Liberamos el espacio
            _frente = (_frente + 1) % _capacidad; // Avanza de forma circular
            _cantidad--;
            
            return cancion;
        }

        public void Mostrar()
        {
            if (EstaVacia())
            {
                Console.WriteLine("(Cola circular vacía)");
                return;
            }

            Console.WriteLine("Estado actual de la cola:");
            int i = _frente;
            for (int cont = 0; cont < _cantidad; cont++)
            {
                Console.WriteLine($"- {_arreglo[i]!.Titulo}");
                i = (i + 1) % _capacidad;
            }
        }
    }
}