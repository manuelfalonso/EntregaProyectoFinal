using System.Data.SqlClient;

namespace CSharpAPICore.Repository
{
    public class General
    {
        public static string ConnectionToString()
        {
            SqlConnectionStringBuilder connectionBuilder = new SqlConnectionStringBuilder();
            // Se saca de Boton derecho properties de la conexion de la base de datos en 
            // SQL Server Management Studio
            connectionBuilder.DataSource = "DESKTOP-5L9GC2G\\SQLEXPRESS";
            connectionBuilder.InitialCatalog = "SistemaGestion";
            connectionBuilder.IntegratedSecurity = true;

            var cs = connectionBuilder.ConnectionString;
            return cs;
        }
    }
}
