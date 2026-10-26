using System;

namespace SpotifyApp
{
    public class NodoAVL
    {
        public Cancion Dato { get; set; }
        public NodoAVL? Izquierda { get; set; }
        public NodoAVL? Derecha { get; set; }
        public int Altura { get; set; }

        public NodoAVL(Cancion cancion)
        {
            Dato = cancion;
            Izquierda = null;
            Derecha = null;
            Altura = 1; // Un nodo nuevo siempre empieza en altura 1
        }
    }

    public class ArbolAVL
    {
        public NodoAVL? Raiz { get; private set; }

        public ArbolAVL()
        {
            Raiz = null;
        }

        private int ObtenerAltura(NodoAVL? nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        private int ObtenerBalance(NodoAVL? nodo)
        {
            return nodo == null ? 0 : ObtenerAltura(nodo.Izquierda) - ObtenerAltura(nodo.Derecha);
        }

        // Rotación a la derecha
        private NodoAVL RotarDerecha(NodoAVL y)
        {
            NodoAVL x = y.Izquierda!;
            NodoAVL? T2 = x.Derecha;

            // Rotación
            x.Derecha = y;
            y.Izquierda = T2;

            // Actualizar alturas
            y.Altura = Math.Max(ObtenerAltura(y.Izquierda), ObtenerAltura(y.Derecha)) + 1;
            x.Altura = Math.Max(ObtenerAltura(x.Izquierda), ObtenerAltura(x.Derecha)) + 1;

            return x;
        }

        // Rotación a la izquierda
        private NodoAVL RotarIzquierda(NodoAVL x)
        {
            NodoAVL y = x.Derecha!;
            NodoAVL? T2 = y.Izquierda;

            // Rotación
            y.Izquierda = x;
            x.Derecha = T2;

            // Actualizar alturas
            x.Altura = Math.Max(ObtenerAltura(x.Izquierda), ObtenerAltura(x.Derecha)) + 1;
            y.Altura = Math.Max(ObtenerAltura(y.Izquierda), ObtenerAltura(y.Derecha)) + 1;

            return y;
        }

        public void Insertar(Cancion cancion)
        {
            Raiz = InsertarRecursivo(Raiz, cancion);
        }

        private NodoAVL InsertarRecursivo(NodoAVL? nodo, Cancion cancion)
        {
            // 1. Inserción normal de BST
            if (nodo == null) return new NodoAVL(cancion);

            if (cancion.Id < nodo.Dato.Id)
                nodo.Izquierda = InsertarRecursivo(nodo.Izquierda, cancion);
            else if (cancion.Id > nodo.Dato.Id)
                nodo.Derecha = InsertarRecursivo(nodo.Derecha, cancion);
            else
                return nodo; // No se permiten IDs duplicados

            // 2. Actualizar la altura del nodo actual
            nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierda), ObtenerAltura(nodo.Derecha));

            // 3. Obtener el factor de balanceo
            int balance = ObtenerBalance(nodo);

            // 4. Si se desbalancea, hacer las rotaciones
            // Caso Izquierda-Izquierda
            if (balance > 1 && cancion.Id < nodo.Izquierda!.Dato.Id)
                return RotarDerecha(nodo);

            // Caso Derecha-Derecha
            if (balance < -1 && cancion.Id > nodo.Derecha!.Dato.Id)
                return RotarIzquierda(nodo);

            // Caso Izquierda-Derecha
            if (balance > 1 && cancion.Id > nodo.Izquierda!.Dato.Id)
            {
                nodo.Izquierda = RotarIzquierda(nodo.Izquierda);
                return RotarDerecha(nodo);
            }

            // Caso Derecha-Izquierda
            if (balance < -1 && cancion.Id < nodo.Derecha!.Dato.Id)
            {
                nodo.Derecha = RotarDerecha(nodo.Derecha);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        public void MostrarInOrden()
        {
            MostrarInOrdenRecursivo(Raiz);
        }

        private void MostrarInOrdenRecursivo(NodoAVL? actual)
        {
            if (actual != null)
            {
                MostrarInOrdenRecursivo(actual.Izquierda);
                Console.WriteLine($"- ID: {actual.Dato.Id} | {actual.Dato.Titulo} (Altura: {actual.Altura})");
                MostrarInOrdenRecursivo(actual.Derecha);
            }
        }
    }
}
