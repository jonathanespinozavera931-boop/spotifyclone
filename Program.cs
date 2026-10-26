using System;

namespace SpotifyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool salir = false;
            
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("     🎵 SPOTIFY CLONE - MENÚ 🎵         ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Arreglo Dinámico");
                Console.WriteLine("2. Lista Simple");
                Console.WriteLine("3. Lista Doble");
                Console.WriteLine("4. Pila (Historial)");
                Console.WriteLine("5. Cola Lineal");
                Console.WriteLine("6. Cola Circular");
                Console.WriteLine("7. Árbol BST (Catálogo)");
                Console.WriteLine("8. Árbol AVL (Ranking)");
                Console.WriteLine("9. Grafo (Recomendaciones y Dijkstra)");
                Console.WriteLine("10. Probar Base de Datos (Neon)");
                Console.WriteLine("0. Salir");
                Console.WriteLine("========================================");
                Console.Write("Elige una opción: ");

                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1": ProbarArregloDinamico(); break;
                    case "2": ProbarListaSimple(); break;
                    case "3": ProbarListaDoble(); break;
                    case "4": ProbarPila(); break;
                    case "5": ProbarColaLineal(); break;
                    case "6": ProbarColaCircular(); break;
                    case "7": ProbarBST(); break;
                    case "8": ProbarAVL(); break;
                    case "9": ProbarGrafo(); break;
                    case "10": ProbarBaseDatos(); break;
                    case "0": salir = true; break;
                    default:
                        Console.WriteLine("Opción no válida...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ProbarArregloDinamico()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Arreglo Dinámico ---");
            MiArregloDinamico<string> canciones = new MiArregloDinamico<string>(2);
            canciones.Agregar("Bohemian Rhapsody");
            canciones.Agregar("Billie Jean");
            canciones.Agregar("Hotel California"); 
            
            for (int i = 0; i < canciones.Longitud; i++)
                Console.WriteLine($"- {canciones.Obtener(i)}");

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }

        static void ProbarListaSimple()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Lista Simple Enlazada ---");
            ListaSimple miPlaylist = new ListaSimple();
            miPlaylist.Agregar(new Cancion(1, "Bohemian Rhapsody", "Queen"));
            miPlaylist.Agregar(new Cancion(2, "Billie Jean", "Michael Jackson"));
            miPlaylist.Agregar(new Cancion(3, "Hotel California", "Eagles"));

            miPlaylist.Mostrar();
            int total = miPlaylist.ContarRecursivo();
            Console.WriteLine($"\n[Recursividad] Total de canciones: {total}");

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }

        static void ProbarListaDoble()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Lista Doble ---");
            ListaDoble playlist = new ListaDoble();
            playlist.Agregar(new Cancion(1, "Shape of You", "Ed Sheeran"));
            playlist.Agregar(new Cancion(2, "Blinding Lights", "The Weeknd"));
            playlist.Agregar(new Cancion(3, "Levitating", "Dua Lipa"));

            Console.WriteLine("Recorrido hacia ADELANTE:");
            playlist.MostrarAdelante();

            Console.WriteLine("\nRecorrido hacia ATRÁS:");
            playlist.MostrarAtras();

            Console.Write("\n[Recursividad] Buscar canción 'Blinding': ");
            bool encontrada = playlist.BuscarRecursivo("Blinding");
            Console.WriteLine(encontrada ? "¡Encontrada!" : "No existe.");

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }

        static void ProbarPila()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Pila (Historial de Reproducción) ---");
            Pila historial = new Pila();

            historial.Apilar(new Cancion(1, "Song A", "Artist 1"));
            historial.Apilar(new Cancion(2, "Song B", "Artist 2"));
            historial.Apilar(new Cancion(3, "Song C", "Artist 3"));

            historial.MostrarHistorial();

            Console.WriteLine("\nPresionando 'Atrás' (Desapilando)...");
            Cancion? cancionAnterior = historial.Desapilar();
            if (cancionAnterior != null)
            {
                Console.WriteLine($"Volviendo a reproducir: {cancionAnterior.Titulo}");
            }

            Console.WriteLine("\nHistorial actualizado:");
            historial.MostrarHistorial();

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }

        static void ProbarColaLineal()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Cola Lineal (Reproducción automática) ---");
            ColaLineal cola = new ColaLineal();

            cola.Encolar(new Cancion(1, "Canción 1", "Artista A"));
            cola.Encolar(new Cancion(2, "Canción 2", "Artista B"));
            cola.Encolar(new Cancion(3, "Canción 3", "Artista C"));

            Console.WriteLine("\nCanciones en espera:");
            cola.Mostrar();

            Console.WriteLine("\nReproduciendo (Desencolando)...");
            Cancion? reproducir = cola.Desencolar();
            Console.WriteLine($"Sonando ahora: {reproducir?.Titulo}");

            Console.WriteLine("\nCanciones en espera después de reproducir una:");
            cola.Mostrar();

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }

        static void ProbarColaCircular()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Cola Circular (Modo Repetir) ---");
            ColaCircular cola = new ColaCircular(3);

            cola.Encolar(new Cancion(1, "Hit 1", "Artista X"));
            cola.Encolar(new Cancion(2, "Hit 2", "Artista Y"));
            cola.Encolar(new Cancion(3, "Hit 3", "Artista Z"));
            
            cola.Encolar(new Cancion(4, "Hit 4", "Artista W")); 

            Console.WriteLine("\nCola llena. Reproduciendo una...");
            cola.Desencolar();

            Console.WriteLine("\nComo se liberó un espacio, agregamos la Canción 4:");
            cola.Encolar(new Cancion(4, "Hit 4", "Artista W"));

            Console.WriteLine("\nEstado actual (fíjate cómo dio la vuelta):");
            cola.Mostrar();

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }

        static void ProbarBST()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Árbol BST (Catálogo) ---");
            ArbolBST catalogo = new ArbolBST();

            catalogo.Insertar(new Cancion(50, "Bohemian Rhapsody", "Queen"));
            catalogo.Insertar(new Cancion(30, "Billie Jean", "Michael Jackson"));
            catalogo.Insertar(new Cancion(70, "Hotel California", "Eagles"));
            catalogo.Insertar(new Cancion(20, "Imagine", "John Lennon"));
            catalogo.Insertar(new Cancion(40, "Smells Like Teen Spirit", "Nirvana"));

            Console.WriteLine("Catálogo ordenado por ID (Recorrido InOrden):");
            catalogo.MostrarInOrden();

            Console.WriteLine("\nBuscando canción con ID 40 de forma recursiva...");
            Cancion? encontrada = catalogo.Buscar(40);
            if (encontrada != null)
                Console.WriteLine($"¡Encontrada! -> {encontrada.Titulo} por {encontrada.Artista}");
            else
                Console.WriteLine("No se encontró la canción.");

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }

        static void ProbarAVL()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Árbol AVL (Auto-balanceado) ---");
            ArbolAVL ranking = new ArbolAVL();

            ranking.Insertar(new Cancion(10, "Canción A", "Artista 1"));
            ranking.Insertar(new Cancion(20, "Canción B", "Artista 2"));
            ranking.Insertar(new Cancion(30, "Canción C", "Artista 3"));
            ranking.Insertar(new Cancion(40, "Canción D", "Artista 4"));
            ranking.Insertar(new Cancion(50, "Canción E", "Artista 5"));

            Console.WriteLine("Ranking en orden (observa las alturas balanceadas):");
            ranking.MostrarInOrden();

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }
                static void ProbarGrafo()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Grafo (Red de Artistas) ---");
            Grafo red = new Grafo();

            // Creamos un mapa de similitudes (Peso bajo = Muy similares)
            red.AgregarRelacion("Queen", "David Bowie", 2);
            red.AgregarRelacion("Queen", "The Beatles", 4);
            red.AgregarRelacion("David Bowie", "Michael Jackson", 3);
            red.AgregarRelacion("Michael Jackson", "The Weeknd", 1);
            red.AgregarRelacion("The Beatles", "The Weeknd", 5);
            red.AgregarRelacion("The Weeknd", "Dua Lipa", 2);

            // 1. BFS
            red.BFS("Queen");
            Console.WriteLine();

            // 2. DFS
            red.DFS("Queen");
            Console.WriteLine();

            // 3. Dijkstra
            red.Dijkstra("Queen");

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }
                        static void ProbarBaseDatos()
        {
            Console.Clear();
            Console.WriteLine("--- Prueba de Base de Datos en Neon ---");
            
            // Cadena convertida al formato que entiende C# (.NET)
            string connString = "Host=ep-young-mud-b4mg9755-pooler.c-6.us-east-2.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_tVb8mBXxE7vJ;SSL Mode=Require;Trust Server Certificate=true"; 

            BaseDatos db = new BaseDatos(connString);
            
            // 1. Probar conexión
            db.ProbarConexion();

            // 2. Guardar una canción de prueba
            Cancion nueva = new Cancion(99, "Mi Canción en la Nube", "Artista Cloud");
            db.GuardarCancion(nueva);

            // 3. Leer todas las canciones guardadas
            db.MostrarCanciones();

            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }
    }
}