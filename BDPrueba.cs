using System;
using MySql.Data.MySqlClient;

public class BDPrueba
{
	public BDPrueba()
	{

	}

    public static bool probarConexion()
    {
        
        
        MySqlConnection con;
        String servidor = "localhost";
        String puerto = "3306";
        String usuario = "usuario";
        String password = "clave";
        String database = "base_datos";

        //Cadena de conexion
        Sesion.cadenaConexion = String.Format("Password=sancho;User ID=sa;Data Source=laptop104;Initial Catalog=easi3;timeout=2147483647;Pooling=False;Connection Lifetime=1");

        con = new MySqlConnection(Sesion.cadenaConexion);
        con.Open();//se abre la conexion
        if (con.State == System.Data.ConnectionState.Open)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
