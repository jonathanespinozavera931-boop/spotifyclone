using SpotifyApp;

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

// Rutas de la API
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

// ESTA LÍNEA ES CLAVE: Si alguien entra a una ruta que no es API, le muestra el index.html
app.MapFallbackToFile("index.html");

// Ruta para obtener el catálogo de canciones de prueba
app.MapGet("/api/canciones", () => {
    var catalogo = new[] {
        new { Id = 1, Titulo = "Gavilán o Paloma", Artista = "José José", Portada = "https://picsum.photos/id/10/300/300", UrlAudio = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3" },
        new { Id = 2, Titulo = "Sunflower", Artista = "Post Malone, Swae Lee", Portada = "https://picsum.photos/id/20/300/300", UrlAudio = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3" },
        new { Id = 3, Titulo = "As It Was", Artista = "Harry Styles", Portada = "https://picsum.photos/id/30/300/300", UrlAudio = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-3.mp3" },
        new { Id = 4, Titulo = "Blinding Lights", Artista = "The Weeknd", Portada = "https://picsum.photos/id/40/300/300", UrlAudio = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-4.mp3" },
        new { Id = 5, Titulo = "Tus Me Gusta", Artista = "Tus favoritos", Portada = "https://picsum.photos/id/50/300/300", UrlAudio = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-5.mp3" }
    };
    return Results.Ok(catalogo);
});

app.Run(); 