using System;
using Npgsql;

namespace SpotifyApp
{
    public class BaseDatos
    {
        private string _connectionString;

        public BaseDatos(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Probar que la conexión funciona
        public void ProbarConexion()
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    Console.WriteLine("✅ ¡Conexión exitosa a la base de datos de Neon!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error de conexión: {ex.Message}");
            }
        }

        // Insertar una canción desde C# a la nube
        public void GuardarCancion(Cancion cancion)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO canciones (id, titulo, artista) VALUES (@id, @titulo, @artista)";
                    
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", cancion.Id);
                        cmd.Parameters.AddWithValue("titulo", cancion.Titulo);
                        cmd.Parameters.AddWithValue("artista", cancion.Artista);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine($"🎵 Canción '{cancion.Titulo}' guardada en la nube.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al guardar: {ex.Message}");
            }
        }

        // Leer las canciones de la nube
        public void MostrarCanciones()
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT id, titulo, artista FROM canciones ORDER BY id";
                    
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("\n--- Canciones en la Base de Datos ---");
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string titulo = reader.GetString(1);
                            string artista = reader.GetString(2);
                            Console.WriteLine($"[{id}] {titulo} - {artista}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al leer: {ex.Message}");
            }
        }
    }
}