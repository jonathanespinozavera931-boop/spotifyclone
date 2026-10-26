namespace SpotifyApp
{
    public class Cancion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Artista { get; set; }

        public Cancion(int id, string titulo, string artista)
        {
            Id = id;
            Titulo = titulo;
            Artista = artista;
        }

        public override string ToString()
        {
            return $"{Id} - {Titulo} (por {Artista})";
        }
    }
}