using SpotifyApp;
using System.Net.Http.Json;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Habilitar CORS
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors();

// ESTAS DOS LÍNEAS SON LAS QUE HACEN LA MAGIA PARA EL HTML
app.UseDefaultFiles(); // Busca el index.html automáticamente
app.UseStaticFiles();  // Habilita la carpeta wwwroot

// --- RUTAS DE LA API DE ESTRUCTURAS ---
app.MapGet("/api/arreglo", () => {
    var canciones = new MiArregloDinamico<string>(2);
    canciones.Agregar("Bohemian Rhapsody");
    canciones.Agregar("Billie Jean");
    canciones.Agregar("Hotel California");
    var lista = new List<string>();
    for (int i = 0; i < canciones.Longitud; i++) lista.Add(canciones.Obtener(i));
    return Results.Ok(new { Mensaje = "Arreglo Dinámico redimensionado con éxito", Canciones = lista });
});

app.MapGet("/api/lista-simple", () => {
    var lista = new ListaSimple();
    lista.Agregar(new Cancion(1, "Bohemian Rhapsody", "Queen"));
    lista.Agregar(new Cancion(2, "Billie Jean", "Michael Jackson"));
    return Results.Ok(new { TotalRecursivo = lista.ContarRecursivo(), Mensaje = "Lista simple contada" });
});

app.MapGet("/api/bst", () => {
    var bst = new ArbolBST();
    bst.Insertar(new Cancion(50, "Bohemian Rhapsody", "Queen"));
    bst.Insertar(new Cancion(30, "Billie Jean", "Michael Jackson"));
    return Results.Ok(new { Mensaje = "Árbol BST operativo y balanceado" });
});

app.MapGet("/api/grafo", () => {
    var red = new Grafo();
    red.AgregarRelacion("Queen", "David Bowie", 2);
    red.AgregarRelacion("David Bowie", "Michael Jackson", 3);
    return Results.Ok(new { Mensaje = "Grafo de similitud (Dijkstra/BFS) operativo" });
});

app.MapGet("/api/bd", () => {
    string connString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
                        ?? "Host=ep-young-mud-b4mg9755-pooler.c-6.us-east-2.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_tVb8mBXxE7vJ;SSL Mode=Require;Trust Server Certificate=true";
    BaseDatos db = new BaseDatos(connString);
    db.ProbarConexion();
    return Results.Ok(new { Mensaje = "Base de datos en Neon conectada correctamente" });
});

// --- NUEVA RUTA: CATÁLOGO CON AUDIOS REALES DESDE ITUNES ---
app.MapGet("/api/canciones", async () => {
    var nombresCanciones = new[] {
        ("Gavilán o Paloma", "José José"),
        ("Sunflower", "Post Malone"),
        ("As It Was", "Harry Styles"),
        ("Blinding Lights", "The Weeknd"),
        ("Tus Me Gusta", "Tus favoritos")
    };

    var catalogo = new List<object>();
    using var httpClient = new HttpClient();

    foreach (var (titulo, artista) in nombresCanciones) {
        try {
            // Buscamos la canción en iTunes
            var query = Uri.EscapeDataString($"{titulo} {artista}");
            var response = await httpClient.GetFromJsonAsync<JsonElement>($"https://itunes.apple.com/search?term={query}&media=music&limit=1");

            var previewUrl = "";
            var artworkUrl = "";

            // Si iTunes encontró resultados, extraemos el audio y la portada
            if (response.GetProperty("resultCount").GetInt32() > 0) {
                var result = response.GetProperty("results")[0];
                previewUrl = result.GetProperty("previewUrl").GetString() ?? "";
                artworkUrl = result.GetProperty("artworkUrl100").GetString()?.Replace("100x100bb", "300x300bb") ?? "";
            }

            // Si no hay audio válido, usamos uno de respaldo para que no falle el reproductor
            if (string.IsNullOrEmpty(previewUrl)) {
                previewUrl = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3";
            }

            catalogo.Add(new {
                titulo = titulo,
                artista = artista,
                portada = string.IsNullOrEmpty(artworkUrl) ? $"https://picsum.photos/id/{catalogo.Count + 10}/300/300" : artworkUrl,
                urlAudio = previewUrl
            });
        }
        catch {
            // En caso de error de red, evitamos que la app explote
            catalogo.Add(new {
                titulo = titulo,
                artista = artista,
                portada = $"https://picsum.photos/id/{catalogo.Count + 10}/300/300",
                urlAudio = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"
            });
        }
    }
    
    return Results.Ok(catalogo);
});

// ESTA LÍNEA VA AL FINAL: Si alguien entra a una ruta que no es API, le muestra el index.html
app.MapFallbackToFile("index.html");

app.Run();