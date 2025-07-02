using Npgsql;
using System;
using System.Windows.Forms;

namespace GyG.Datos
{
    public class LoginDAO
    {
        public static bool VerificarCredenciales(string usuario, string clave)
        {
            bool valido = false;

            try
            {
                using (var conn = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT COUNT(*) FROM owner WHERE username = @usuario AND password = @clave";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("usuario", usuario);
                        cmd.Parameters.AddWithValue("clave", clave);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        valido = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar login: " + ex.Message);
            }

            return valido;
        }
    }
}