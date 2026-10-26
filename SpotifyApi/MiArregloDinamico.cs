using System;

namespace SpotifyApp
{
    public class MiArregloDinamico<T>
    {
        private T[] _elementos;
        private int _contador;

        public MiArregloDinamico(int capacidadInicial = 5)
        {
            _elementos = new T[capacidadInicial];
            _contador = 0;
        }

        public void Agregar(T elemento)
        {
            if (_contador == _elementos.Length)
            {
                Redimensionar();
            }
            _elementos[_contador] = elemento;
            _contador++;
        }

        private void Redimensionar()
        {
            T[] nuevoArreglo = new T[_elementos.Length * 2];
            for (int i = 0; i < _elementos.Length; i++)
            {
                nuevoArreglo[i] = _elementos[i];
            }
            _elementos = nuevoArreglo;
            Console.WriteLine($"[Sistema] Arreglo redimensionado a {_elementos.Length} posiciones.");
        }

        public T Obtener(int indice)
        {
            if (indice < 0 || indice >= _contador)
                throw new IndexOutOfRangeException("Índice fuera de rango.");
            
            return _elementos[indice];
        }

        public int Longitud => _contador;
    }
}