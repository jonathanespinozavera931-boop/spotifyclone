using SpotifyApp;
using System.Net.Http.Json;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

// Estructuras (igual que antes)
app.MapGet("/api/arreglo", () => Results.Ok(new { Mensaje = "Arreglo ok" }));
app.MapGet("/api/lista-simple", () => Results.Ok(new { Mensaje = "Lista ok" }));
app.MapGet("/api/bst", () => Results.Ok(new { Mensaje = "BST ok" }));
app.MapGet("/api/grafo", () => Results.Ok(new { Mensaje = "Grafo ok" }));
app.MapGet("/api/bd", () => Results.Ok(new { Mensaje = "BD ok" }));

async Task<object> BuscarEnITunes(string titulo, string artista, int id, string genero, bool esNuevo = false) {
    using var httpClient = new HttpClient();
    try {
        var query = Uri.EscapeDataString($"{titulo} {artista}");
        var response = await httpClient.GetFromJsonAsync<JsonElement>($"https://itunes.apple.com/search?term={query}&media=music&limit=1");
        
        string previewUrl = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"; 
        string artworkUrl = $"https://picsum.photos/id/{id + 10}/300/300";

        if (response.GetProperty("resultCount").GetInt32() > 0) {
            var result = response.GetProperty("results")[0];
            if(result.TryGetProperty("previewUrl", out var pUrl)) previewUrl = pUrl.GetString() ?? previewUrl;
            if(result.TryGetProperty("artworkUrl100", out var aUrl)) artworkUrl = aUrl.GetString()?.Replace("100x100bb", "300x300bb") ?? artworkUrl;
        }

        return new { id = id, titulo = titulo, artista = artista, portada = artworkUrl, urlAudio = previewUrl, genero = genero, esNuevo = esNuevo };
    } catch {
        return new { id = id, titulo = titulo, artista = artista, portada = $"https://picsum.photos/id/{id}/300/300", urlAudio = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3", genero = genero, esNuevo = esNuevo };
    }
}

app.MapGet("/api/canciones", async () => {
    var listaBase = new[] {
        ("Shape of You", "Ed Sheeran", "pop"), ("Blinding Lights", "The Weeknd", "pop"), ("Levitating", "Dua Lipa", "pop"),
        ("Bad Guy", "Billie Eilish", "pop"), ("As It Was", "Harry Styles", "pop"), ("Sunflower", "Post Malone", "pop"),
        ("Stay", "The Kid LAROI", "pop"), ("Believer", "Imagine Dragons", "rock"), ("Perfect", "Ed Sheeran", "pop"),
        ("Someone Like You", "Adele", "pop"), ("Uptown Funk", "Mark Ronson", "pop"), ("Havana", "Camila Cabello", "latino"),
        ("Despacito", "Luis Fonsi", "latino"), ("Tití Me Preguntó", "Bad Bunny", "latino"), ("Me Porto Bonito", "Bad Bunny", "latino"),
        ("Ojitos Lindos", "Bad Bunny", "latino"), ("La Canción", "J Balvin", "latino"), ("Tusa", "Karol G", "latino"),
        ("PROVENZA", "Karol G", "latino"), ("Todo De Ti", "Rauw Alejandro", "latino"), ("Pepas", "Farruko", "latino"),
        ("Yonaguni", "Bad Bunny", "latino"), ("Dákiti", "Bad Bunny", "latino"), ("Hawái", "Maluma", "latino"),
        ("Mónaco", "Bad Bunny", "latino"), ("Feliz Cumpleaños Ferxxo", "Feid", "latino"), ("Normal", "Feid", "latino"),
        ("Classy 101", "Feid", "latino"), ("Luna", "Feid", "latino"), ("Hey Mor", "Ozuna", "latino"),
        ("La Bebé", "Yng Lvcas", "latino"), ("Ella Baila Sola", "Eslabon Armado", "latino"), ("TQG", "Karol G", "latino"),
        ("Bohemian Rhapsody", "Queen", "rock"), ("Smells Like Teen Spirit", "Nirvana", "rock"), ("Hotel California", "Eagles", "rock"),
        ("Sweet Child O' Mine", "Guns N' Roses", "rock"), ("Imagine", "John Lennon", "rock"), ("Wonderwall", "Oasis", "indie"),
        ("Mr. Brightside", "The Killers", "indie"), ("Sweater Weather", "The Neighbourhood", "indie"), ("Dog Days Are Over", "Florence", "indie"),
        ("Take Me Out", "Franz Ferdinand", "indie"), ("Do I Wanna Know?", "Arctic Monkeys", "indie"), ("Clair de Lune", "Debussy", "clasica"),
        ("Nocturne Op.9 No.2", "Chopin", "clasica"), ("The Four Seasons", "Vivaldi", "clasica"), ("Moonlight Sonata", "Beethoven", "clasica"),
        ("Canon in D", "Pachelbel", "clasica"), ("Flowers", "Miley Cyrus", "pop")
    };

    var catalogo = new List<object>();
    var tareas = new List<Task>();
    using var semaforo = new SemaphoreSlim(10);

    for (int i = 0; i < listaBase.Length; i++) {
        int index = i;
        var (titulo, artista, genero) = listaBase[i];
        tareas.Add(Task.Run(async () => {
            await semaforo.WaitAsync();
            try {
                var obj = await BuscarEnITunes(titulo, artista, index, genero, index < 6);
                lock (catalogo) { catalogo.Add(obj); }
            } finally { semaforo.Release(); }
        }));
    }

    await Task.WhenAll(tareas);
    return Results.Ok(catalogo.OrderBy(c => ((dynamic)c).id).ToList());
});

// BUSCADOR: Acepta cualquier término de búsqueda
app.MapGet("/api/buscar", async (string q) => {
    if (string.IsNullOrWhiteSpace(q) || q.Length < 2) return Results.Ok(new List<object>());
    
    using var httpClient = new HttpClient();
    var query = Uri.EscapeDataString(q);
    var response = await httpClient.GetFromJsonAsync<JsonElement>($"https://itunes.apple.com/search?term={query}&media=music&limit=30");

    var resultados = new List<object>();
    int count = response.GetProperty("resultCount").GetInt32();
    var results = response.GetProperty("results");

    for (int i = 0; i < count; i++) {
        var item = results[i];
        string trackName = item.GetProperty("trackName").GetString() ?? "";
        string artistName = item.GetProperty("artistName").GetString() ?? "";
        string previewUrl = item.TryGetProperty("previewUrl", out var p) ? p.GetString() ?? "" : "";
        string artwork = item.TryGetProperty("artworkUrl100", out var a) ? a.GetString()?.Replace("100x100bb", "300x300bb") ?? "" : "";
        
        // Opcional: obtener el género real de iTunes, pero para búsquedas no importa
        string genero = "busqueda"; 

        if (!string.IsNullOrEmpty(previewUrl)) {
            resultados.Add(new {
                id = 1000 + i,
                titulo = trackName,
                artista = artistName,
                portada = artwork,
                urlAudio = previewUrl,
                genero = genero,
                esNuevo = false
            });
        }
    }
    return Results.Ok(resultados);
});

app.MapFallbackToFile("index.html");
app.Run();