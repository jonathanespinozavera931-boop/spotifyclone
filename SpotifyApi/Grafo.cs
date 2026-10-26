using System;
using System.Collections.Generic;

namespace SpotifyApp
{
    // Representa una conexión a otro artista y su "peso" (qué tan similares son)
    public class Arista
    {
        public string Destino { get; set; }
        public int Peso { get; set; }

        public Arista(string destino, int peso)
        {
            Destino = destino;
            Peso = peso;
        }
    }

    public class Grafo
    {
        // Lista de adyacencia: Artista -> Lista de sus conexiones
        private Dictionary<string, List<Arista>> _adyacencia;

        public Grafo()
        {
            _adyacencia = new Dictionary<string, List<Arista>>();
        }

        public void AgregarArtista(string artista)
        {
            if (!_adyacencia.ContainsKey(artista))
                _adyacencia[artista] = new List<Arista>();
        }

        public void AgregarRelacion(string origen, string destino, int peso)
        {
            AgregarArtista(origen);
            AgregarArtista(destino);
            
            // Como es no dirigido, agregamos en ambos sentidos
            _adyacencia[origen].Add(new Arista(destino, peso));
            _adyacencia[destino].Add(new Arista(origen, peso));
        }

        // --- BFS (Búsqueda en Anchura) ---
        // Encuentra los artistas más cercanos (recomendaciones directas)
        public void BFS(string inicio)
        {
            if (!_adyacencia.ContainsKey(inicio)) return;

            HashSet<string> visitados = new HashSet<string>();
            Queue<string> cola = new Queue<string>();

            visitados.Add(inicio);
            cola.Enqueue(inicio);

            Console.WriteLine($"Recomendaciones BFS (Cercanas) partiendo de {inicio}:");
            while (cola.Count > 0)
            {
                string actual = cola.Dequeue();
                Console.WriteLine($"- {actual}");

                foreach (var arista in _adyacencia[actual])
                {
                    if (!visitados.Contains(arista.Destino))
                    {
                        visitados.Add(arista.Destino);
                        cola.Enqueue(arista.Destino);
                    }
                }
            }
        }

        // --- DFS (Búsqueda en Profundidad) RECURSIVA ---
        // Explora un camino hasta el fondo antes de retroceder
        public void DFS(string inicio)
        {
            if (!_adyacencia.ContainsKey(inicio)) return;
            
            HashSet<string> visitados = new HashSet<string>();
            Console.WriteLine($"Exploración DFS (Profunda) partiendo de {inicio}:");
            DFSRecursivo(inicio, visitados);
        }

        private void DFSRecursivo(string actual, HashSet<string> visitados)
        {
            visitados.Add(actual);
            Console.WriteLine($"- {actual}");

            foreach (var arista in _adyacencia[actual])
            {
                if (!visitados.Contains(arista.Destino))
                {
                    DFSRecursivo(arista.Destino, visitados);
                }
            }
        }

        // --- DIJKSTRA ---
        // Encuentra el camino más corto (menor peso) del artista "origen" a todos los demás
        public void Dijkstra(string origen)
        {
            if (!_adyacencia.ContainsKey(origen)) return;

            var distancias = new Dictionary<string, int>();
            var anteriores = new Dictionary<string, string?>();
            var noVisitados = new List<string>();

            // Inicializar distancias en infinito
            foreach (var artista in _adyacencia.Keys)
            {
                distancias[artista] = int.MaxValue;
                anteriores[artista] = null;
                noVisitados.Add(artista);
            }
            distancias[origen] = 0;

            while (noVisitados.Count > 0)
            {
                // Buscar el nodo no visitado con la distancia más pequeña
                string actual = noVisitados[0];
                foreach (var nodo in noVisitados)
                {
                    if (distancias[nodo] < distancias[actual])
                        actual = nodo;
                }

                if (distancias[actual] == int.MaxValue) break; // Los demás están inalcanzables

                noVisitados.Remove(actual);

                foreach (var arista in _adyacencia[actual])
                {
                    int nuevaDistancia = distancias[actual] + arista.Peso;
                    if (nuevaDistancia < distancias[arista.Destino])
                    {
                        distancias[arista.Destino] = nuevaDistancia;
                        anteriores[arista.Destino] = actual;
                    }
                }
            }

            Console.WriteLine($"\nDistancias más cortas (Similitud) desde {origen}:");
            foreach (var artista in distancias)
            {
                string dist = artista.Value == int.MaxValue ? "Inalcanzable" : artista.Value.ToString();
                Console.WriteLine($"- A {artista.Key}: Peso total {dist}");
            }
        }
    }
}