using CSharpAPICore.Models;
using System.Data.SqlClient;

namespace CSharpAPICore.Repository
{
    public class ADO_Producto
    {
        /// <summary>
        /// Recibe un número de IdUsuario como parámetro, debe traer todos los productos 
        /// cargados en la base de este usuario en particular.
        /// </summary>
        public static List<Producto> TraerProductos(int idUsuario)
        {
            List<Producto> productos = new();

            using (SqlConnection connection = new(General.ConnectionToString()))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * " +
                    "FROM Producto" +
                    " WHERE IdUsuario = @idUsuario";

                // Crear parametro
                var param =
                    new SqlParameter("idUsuario", System.Data.SqlDbType.BigInt);
                param.Value = idUsuario;

                // Agregar el parametro
                cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var producto = new Producto();

                    producto.Id = Convert.ToInt32(reader.GetValue(0));
                    producto.Descripciones = reader.GetValue(1).ToString();
                    producto.Costo = Convert.ToDouble(reader.GetValue(2));
                    producto.PrecioVenta = Convert.ToDouble(reader.GetValue(3));
                    producto.Stock = Convert.ToInt32(reader.GetValue(4));
                    producto.IdUsuario = Convert.ToInt32(reader.GetValue(5));

                    productos.Add(producto);
                }

                reader.Close();
                connection.Close();
            }

            return productos;
        }

        internal static Producto DevolverProducto(int id)
        {
            var producto = new Producto();

            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Producto WHERE Id = @id";

                // Crear parametro
                var param = new SqlParameter();
                param.ParameterName = "id";
                param.Value = id;
                param.SqlDbType = System.Data.SqlDbType.BigInt;

                // Agregar parametro
                cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    producto.Id = Convert.ToInt32(reader.GetValue(0));
                    producto.Descripciones = reader.GetValue(1).ToString();
                    producto.Costo = Convert.ToDouble(reader.GetValue(2));
                    producto.PrecioVenta = Convert.ToDouble(reader.GetValue(3));
                    producto.Stock = Convert.ToInt32(reader.GetValue(4));
                    producto.IdUsuario = Convert.ToInt32(reader.GetValue(5));
                }

                reader.Close();

                connection.Close();
            }

            return producto;
        }

        /// <summary>
        /// Recibe una lista de tareas por JSON, número de Id 0, 
        /// Descripción , costo, precio venta y stock.
        /// </summary>
        /// <param name="producto"></param>
        public static void AgregarProducto(Producto producto)
        {
            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO Producto (Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                    "VALUES (@descripciones, @costo, @precioVenta, @stock, @idUsuario)";

                // Crear parametros
                var paramDescripciones = new SqlParameter("descripciones", System.Data.SqlDbType.VarChar);
                paramDescripciones.Value = producto.Descripciones;

                var paramCosto = new SqlParameter("costo", System.Data.SqlDbType.Money);
                paramCosto.Value = producto.Costo;

                var paramPrecioVenta = new SqlParameter("precioVenta", System.Data.SqlDbType.Money);
                paramPrecioVenta.Value = producto.PrecioVenta;

                var paramStock = new SqlParameter("stock", System.Data.SqlDbType.Int);
                paramStock.Value = producto.Stock;

                var paramIdUsuario = new SqlParameter("idUsuario", System.Data.SqlDbType.BigInt);
                paramIdUsuario.Value = producto.IdUsuario;

                // Agregar parametro
                cmd.Parameters.Add(paramDescripciones);
                cmd.Parameters.Add(paramCosto);
                cmd.Parameters.Add(paramPrecioVenta);
                cmd.Parameters.Add(paramStock);
                cmd.Parameters.Add(paramIdUsuario);

                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// Recibe un producto con su número de Id, 
        /// debe modificarlo con la nueva información.
        /// </summary>
        /// <param name="producto"></param>
        public static void ModificarProducto(Producto producto)
        {
            // Buscar producto para ver si existe
            var productoBuscado = DevolverProducto(producto.Id);

            if (productoBuscado != null && productoBuscado.Id == 0)
                return;

            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =
                    "UPDATE Producto SET Descripciones = @descripciones, Costo = @costo, " +
                    "PrecioVenta = @precioVenta, Stock = @stock, IdUsuario = @idUsuario " +
                    "WHERE Id = @id";

                // Crear parametros
                var paramId = new SqlParameter("id", System.Data.SqlDbType.BigInt);
                paramId.Value = producto.Id;

                var paramDescripciones = new SqlParameter("descripciones", System.Data.SqlDbType.VarChar);
                paramDescripciones.Value = producto.Descripciones;

                var paramCosto = new SqlParameter("costo", System.Data.SqlDbType.Money);
                paramCosto.Value = producto.Costo;

                var paramPrecioVenta = new SqlParameter("precioVenta", System.Data.SqlDbType.Money);
                paramPrecioVenta.Value = producto.PrecioVenta;

                var paramStock = new SqlParameter("stock", System.Data.SqlDbType.Int);
                paramStock.Value = producto.Stock;

                var paramIdUsuario = new SqlParameter("idUsuario", System.Data.SqlDbType.BigInt);
                paramIdUsuario.Value = producto.IdUsuario;

                // Agregar parametro
                cmd.Parameters.Add(paramId);
                cmd.Parameters.Add(paramDescripciones);
                cmd.Parameters.Add(paramCosto);
                cmd.Parameters.Add(paramPrecioVenta);
                cmd.Parameters.Add(paramStock);
                cmd.Parameters.Add(paramIdUsuario);

                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <param name="idProducto"></param>
        /// <param name="stock">Ingresar stock negativo para sumar stock</param>
        internal static void ModificarProductoStock(int idProducto, int stock)
        {
            // Buscar producto para ver si existe
            var productoBuscado = DevolverProducto(idProducto);

            if (productoBuscado != null && productoBuscado.Id == 0)
                return;

            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =
                    "UPDATE Producto SET Stock = @stock " +
                    "WHERE Id = @id";

                // Crear parametros
                var paramId = 
                    new SqlParameter("id", System.Data.SqlDbType.BigInt) 
                    { Value = idProducto };
                var paramStock = 
                    new SqlParameter("stock", System.Data.SqlDbType.Int) 
                    { Value = productoBuscado.Stock - stock };

                // Agregar parametro
                cmd.Parameters.Add(paramId);
                cmd.Parameters.Add(paramStock);

                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// Recibe un id de producto a eliminar y debe eliminarlo de la base de datos
        /// (eliminar antes sus productos vendidos también, sino no lo podrá hacer)
        /// </summary>
        /// <param name="id"></param>
        internal static void EliminarProducto(int id)
        {
            ADO_ProductoVendido.EliminarProductoVendidoByIdProducto(id);

            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Producto WHERE Id = @id";

                // Crear parametro
                var param = new SqlParameter();
                param.ParameterName = "id";
                param.Value = id;
                param.SqlDbType = System.Data.SqlDbType.BigInt;

                // Agregar parametro
                cmd.Parameters.Add(param);
                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
