using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace eaSI3Web.Controllers
{
    public class BDPrueba
    {
        public SqlConnection conexion;
        private string semana;
        private string fechaActual;
        private string usuario;
        private int horas;
        private int error;

        public BDPrueba(string usuario, string semana, string fechaActual, int horas, int error) {

            this.usuario = usuario;
            this.semana = semana;
            this.fechaActual = fechaActual;
            this.horas = horas;
            this.error = error;

            conexion = new SqlConnection();
        }
        public void Conexion()
        {

            conexion.ConnectionString = "Password=sancho;User ID=sa;Data Source=.;Initial Catalog=easi3;Connection Timeout=2147483647;Pooling=False;Connection Lifetime=1";
            conexion.Open();

            SqlTransaction tr = conexion.BeginTransaction(IsolationLevel.Serializable);

            SqlCommand cmd = new SqlCommand("INSERT INTO DatosEasi3(IdUsuario,Semana,FechaActual,Horas,Error) VALUES(@id, @sem, @f, @h, @e) ", conexion, tr);
            cmd.Parameters.Add("@i", SqlDbType.VarChar).Value = usuario;
            cmd.Parameters.Add("@sem", SqlDbType.VarChar).Value = semana;
            cmd.Parameters.Add("@f",SqlDbType.VarChar).Value = fechaActual;
            cmd.Parameters.Add("@h",SqlDbType.Int).Value = horas;
            cmd.Parameters.Add("@e", SqlDbType.Int).Value = error;
            try
            {
                //Ejecuto
                cmd.ExecuteNonQuery();
                tr.Commit();

            }
            catch (Exception ex)
            {
                tr.Rollback();
              
            }
            finally
            {
                conexion.Close();
            }
            
        }

    }
}
