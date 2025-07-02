using Npgsql;
using System;
using System.Configuration;

namespace GyG.Datos
{
    public class Conexion
    {
        private static readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=;Database=gyg";

        public static NpgsqlConnection ObtenerConexion()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar con la base de datos: " + ex.Message);
            }
        }

        public static void CerrarConexion(NpgsqlConnection conn)
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }
}