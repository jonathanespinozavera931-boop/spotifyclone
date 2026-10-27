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

// Rutas de las estructuras (se mantienen igual)
app.MapGet("/api/arreglo", () => { /* ... tu código anterior ... */ return Results.Ok(new { Mensaje = "Arreglo ok" }); });
app.MapGet("/api/lista-simple", () => { /* ... tu código anterior ... */ return Results.Ok(new { Mensaje = "Lista ok" }); });
app.MapGet("/api/bst", () => { /* ... tu código anterior ... */ return Results.Ok(new { Mensaje = "BST ok" }); });
app.MapGet("/api/grafo", () => { /* ... tu código anterior ... */ return Results.Ok(new { Mensaje = "Grafo ok" }); });
app.MapGet("/api/bd", () => { /* ... tu código anterior ... */ return Results.Ok(new { Mensaje = "BD ok" }); });

// --- NUEVA RUTA: 50 CANCIONES POPULARES ---
app.MapGet("/api/canciones", async () => {
    var nombresCanciones = new[] {
        ("Shape of You", "Ed Sheeran"), ("Blinding Lights", "The Weeknd"), ("Levitating", "Dua Lipa"),
        ("Bad Guy", "Billie Eilish"), ("As It Was", "Harry Styles"), ("Sunflower", "Post Malone"),
        ("Stay", "The Kid LAROI"), ("Believer", "Imagine Dragons"), ("Perfect", "Ed Sheeran"),
        ("Someone Like You", "Adele"), ("Uptown Funk", "Mark Ronson"), ("Havana", "Camila Cabello"),
        ("Despacito", "Luis Fonsi"), ("Tití Me Preguntó", "Bad Bunny"), ("Me Porto Bonito", "Bad Bunny"),
        ("Ojitos Lindos", "Bad Bunny"), ("La Canción", "J Balvin"), ("Tusa", "Karol G"),
        ("PROVENZA", "Karol G"), ("Todo De Ti", "Rauw Alejandro"), ("Pepas", "Farruko"),
        ("Yonaguni", "Bad Bunny"), ("Dákiti", "Bad Bunny"), ("Hawái", "Maluma"),
        ("Mónaco", "Bad Bunny"), ("Feliz Cumpleaños Ferxxo", "Feid"), ("Normal", "Feid"),
        ("Classy 101", "Feid"), ("Luna", "Feid"), ("Hey Mor", "Ozuna"),
        ("La Bebé", "Yng Lvcas"), ("Ella Baila Sola", "Eslabon Armado"), ("TQG", "Karol G"),
        ("Shakira: Bzrp Music Sessions", "Bizarrap"), ("Flowers", "Miley Cyrus"), ("Anti-Hero", "Taylor Swift"),
        ("Cruel Summer", "Taylor Swift"), ("Karma", "Taylor Swift"), ("Vampire", "Olivia Rodrigo"),
        ("Paint The Town Red", "Doja Cat"), ("Snooze", "SZA"), ("Kill Bill", "SZA"),
        ("Fast Car", "Luke Combs"), ("Last Night", "Morgan Wallen"), ("Calm Down", "Rema"),
        ("Essence", "Wizkid"), ("Love Nwantiti", "CKay"), ("Unholy", "Sam Smith"),
        ("Cold Heart", "Elton John"), ("Where Are You Now", "Lost Frequencies")
    };

    var catalogo = new List<object>();
    using var httpClient = new HttpClient();
    var tareas = new List<Task>();

    for (int i = 0; i < nombresCanciones.Length; i++) {
        int index = i;
        var (titulo, artista) = nombresCanciones[i];

        tareas.Add(Task.Run(async () => {
            try {
                var query = Uri.EscapeDataString($"{titulo} {artista}");
                var response = await httpClient.GetFromJsonAsync<JsonElement>($"https://itunes.apple.com/search?term={query}&media=music&limit=1");
                
                string previewUrl = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"; // Respaldo
                string artworkUrl = $"https://picsum.photos/id/{index + 10}/300/300";

                if (response.GetProperty("resultCount").GetInt32() > 0) {
                    var result = response.GetProperty("results")[0];
                    if(result.TryGetProperty("previewUrl", out var pUrl)) previewUrl = pUrl.GetString() ?? previewUrl;
                    if(result.TryGetProperty("artworkUrl100", out var aUrl)) artworkUrl = aUrl.GetString()?.Replace("100x100bb", "300x300bb") ?? artworkUrl;
                }

                lock (catalogo) {
                    catalogo.Add(new {
                        id = index,
                        titulo = titulo,
                        artista = artista,
                        portada = artworkUrl,
                        urlAudio = previewUrl,
                        esNuevo = index < 6 // Los primeros 6 tendrán la etiqueta "Nuevo"
                    });
                }
            } catch { /* Ignorar errores individuales */ }
        }));
    }

    await Task.WhenAll(tareas);
    
    // Ordenamos por ID para que no se mezclen
    var catalogoOrdenado = catalogo.OrderBy(c => ((dynamic)c).id).ToList();
    return Results.Ok(catalogoOrdenado);
});

app.MapFallbackToFile("index.html");
app.Run();