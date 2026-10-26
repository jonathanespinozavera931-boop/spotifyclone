using System;

namespace SpotifyApp
{
    // El Nodo representa un eslabón de la cadena
    public class Nodo
    {
        public Cancion Dato { get; set; }
        public Nodo? Siguiente { get; set; } // El '?' quita la advertencia amarilla

        public Nodo(Cancion cancion)
        {
            Dato = cancion;
            Siguiente = null;
        }
    }

    // La Lista Simple
    public class ListaSimple
    {
        private Nodo? _cabeza; // El '?' quita la advertencia amarilla

        public ListaSimple()
        {
            _cabeza = null;
        }

        // Agregar al final de la lista
        public void Agregar(Cancion cancion)
        {
            Nodo nuevoNodo = new Nodo(cancion);
            
            if (_cabeza == null)
            {
                _cabeza = nuevoNodo; // Si la lista está vacía, este es el primero
            }
            else
            {
                // Si ya hay elementos, recorremos hasta el último
                Nodo actual = _cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
        }

        // Mostrar todas las canciones
        public void Mostrar()
        {
            if (_cabeza == null)
            {
                Console.WriteLine("La lista está vacía.");
                return;
            }

            Nodo? actual = _cabeza;
            int posicion = 1;
            while (actual != null)
            {
                Console.WriteLine($"{posicion}. {actual.Dato.ToString()}");
                actual = actual.Siguiente;
                posicion++;
            }
        }

        // REQUISITO DE RECURSIVIDAD (1 de 3+)
        // Contar cuántas canciones hay en la lista de forma recursiva
        public int ContarRecursivo()
        {
            return ContarDesdeNodo(_cabeza);
        }

        private int ContarDesdeNodo(Nodo? nodo) // El '?' aquí también
        {
            if (nodo == null) return 0; // Caso base
            return 1 + ContarDesdeNodo(nodo.Siguiente); // Llamada recursiva
        }
    }
}