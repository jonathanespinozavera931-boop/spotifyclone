using System;

namespace SpotifyApp
{
    public class NodoDoble
    {
        public Cancion Dato { get; set; }
        public NodoDoble? Siguiente { get; set; }
        public NodoDoble? Anterior { get; set; }

        public NodoDoble(Cancion cancion)
        {
            Dato = cancion;
            Siguiente = null;
            Anterior = null;
        }
    }

    public class ListaDoble
    {
        private NodoDoble? _cabeza;
        private NodoDoble? _cola;

        public ListaDoble()
        {
            _cabeza = null;
            _cola = null;
        }

        // Agregar al final
        public void Agregar(Cancion cancion)
        {
            NodoDoble nuevoNodo = new NodoDoble(cancion);
            if (_cabeza == null)
            {
                _cabeza = nuevoNodo;
                _cola = nuevoNodo;
            }
            else
            {
                nuevoNodo.Anterior = _cola;
                _cola!.Siguiente = nuevoNodo; // El '!' silencia advertencia porque sabemos que _cola no es nulo aquí
                _cola = nuevoNodo;
            }
        }

        public void MostrarAdelante()
        {
            NodoDoble? actual = _cabeza;
            while (actual != null)
            {
                Console.WriteLine($"-> {actual.Dato.ToString()}");
                actual = actual.Siguiente;
            }
        }

        public void MostrarAtras()
        {
            NodoDoble? actual = _cola;
            while (actual != null)
            {
                Console.WriteLine($"<- {actual.Dato.ToString()}");
                actual = actual.Anterior;
            }
        }

        // REQUISITO DE RECURSIVIDAD (2 de 3+)
        // Buscar una canción por título usando recursividad (hacia adelante)
        public bool BuscarRecursivo(string titulo)
        {
            return BuscarDesdeNodo(_cabeza, titulo);
        }

        private bool BuscarDesdeNodo(NodoDoble? nodo, string titulo)
        {
            if (nodo == null) return false; // Caso base: llegamos al final
            if (nodo.Dato.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase)) return true; // Lo encontramos
            return BuscarDesdeNodo(nodo.Siguiente, titulo); // Llamada recursiva
        }
    }
}