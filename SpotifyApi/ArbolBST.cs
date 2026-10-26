using System;

namespace SpotifyApp
{
    public class NodoBST
    {
        public Cancion Dato { get; set; }
        public NodoBST? Izquierda { get; set; }
        public NodoBST? Derecha { get; set; }

        public NodoBST(Cancion cancion)
        {
            Dato = cancion;
            Izquierda = null;
            Derecha = null;
        }
    }

    public class ArbolBST
    {
        public NodoBST? Raiz { get; private set; }

        public ArbolBST()
        {
            Raiz = null;
        }

        public void Insertar(Cancion cancion)
        {
            Raiz = InsertarRecursivo(Raiz, cancion);
        }

        // RECURSIVIDAD (3 de 3+): Inserción en BST
        private NodoBST InsertarRecursivo(NodoBST? actual, Cancion cancion)
        {
            if (actual == null)
            {
                return new NodoBST(cancion);
            }

            if (cancion.Id < actual.Dato.Id)
            {
                actual.Izquierda = InsertarRecursivo(actual.Izquierda, cancion);
            }
            else if (cancion.Id > actual.Dato.Id)
            {
                actual.Derecha = InsertarRecursivo(actual.Derecha, cancion);
            }
            // Si el ID es igual, no lo insertamos (asumimos IDs únicos)

            return actual;
        }

        public Cancion? Buscar(int id)
        {
            return BuscarRecursivo(Raiz, id);
        }

        // RECURSIVIDAD (4 de 3+): Búsqueda en BST
        private Cancion? BuscarRecursivo(NodoBST? actual, int id)
        {
            if (actual == null) return null; // No encontrado

            if (id == actual.Dato.Id) return actual.Dato; // Encontrado

            if (id < actual.Dato.Id)
                return BuscarRecursivo(actual.Izquierda, id);
            else
                return BuscarRecursivo(actual.Derecha, id);
        }

        // Recorrido InOrden para mostrar ordenado
        public void MostrarInOrden()
        {
            MostrarInOrdenRecursivo(Raiz);
        }

        private void MostrarInOrdenRecursivo(NodoBST? actual)
        {
            if (actual != null)
            {
                MostrarInOrdenRecursivo(actual.Izquierda);
                Console.WriteLine($"- ID: {actual.Dato.Id} | {actual.Dato.Titulo}");
                MostrarInOrdenRecursivo(actual.Derecha);
            }
        }
    }
}