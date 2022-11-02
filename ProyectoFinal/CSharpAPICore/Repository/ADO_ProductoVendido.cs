using CSharpAPICore.Models;
using System.Data.SqlClient;

namespace CSharpAPICore.Repository
{
    public class ADO_ProductoVendido
    {
        /// <summary>
        /// Traer Todos los productos vendidos de un Usuario, cuya información está en 
        /// su producto (Utilizar dentro de esta función el "Traer Productos"
        /// anteriormente hecho para saber que productosVendidos ir a buscar).
        /// </summary>
        public static List<ProductoVendido> TraerProductosVendidos(int idUsuario)
        {
            // Traigo listado completo de productos de un usuario
            List<Producto> productos = ADO_Producto.TraerProductos(idUsuario);

            // Si el producto existe en la tabla producto vendido lo guardo en la lista
            List<ProductoVendido> productosVendidos = new();

            foreach (Producto producto in productos)
            {
                using (SqlConnection connection = new(General.ConnectionToString()))
                {
                    connection.Open();

                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * " +
                        "FROM ProductoVendido " +
                        "WHERE IdProducto = @idProducto";

                    // Crear parametro
                    var param =
                        new SqlParameter("idProducto", System.Data.SqlDbType.BigInt);
                    param.Value = producto.Id;

                    // Agregar el parametro
                    cmd.Parameters.Add(param);

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var productoVendido = new ProductoVendido();

                        productoVendido.Id = Convert.ToInt32(reader.GetValue(0));
                        productoVendido.Stock = Convert.ToInt32(reader.GetValue(1));
                        productoVendido.IdProducto = Convert.ToInt32(reader.GetValue(2));
                        productoVendido.IdVenta = Convert.ToInt32(reader.GetValue(3));

                        productosVendidos.Add(productoVendido);
                    }

                    reader.Close();
                    connection.Close();
                }
            }

            return productosVendidos;
        }

        public static List<ProductoVendido> TraerProductosVendidosByIdVenta(int idVenta)
        {
            List<ProductoVendido> productosVendidos = new();

            using (SqlConnection connection = new(General.ConnectionToString()))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * " +
                    "FROM ProductoVendido " +
                    "WHERE IdVenta = @idVenta";

                // Crear parametro
                var param =
                    new SqlParameter("idVenta", System.Data.SqlDbType.BigInt);
                param.Value = idVenta;

                // Agregar el parametro
                cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var productoVendido = new ProductoVendido();

                    productoVendido.Id = Convert.ToInt32(reader.GetValue(0));
                    productoVendido.Stock = Convert.ToInt32(reader.GetValue(1));
                    productoVendido.IdProducto = Convert.ToInt32(reader.GetValue(2));
                    productoVendido.IdVenta = Convert.ToInt32(reader.GetValue(3));

                    productosVendidos.Add(productoVendido);
                }

                reader.Close();
                connection.Close();
            }

            return productosVendidos;
        }

        internal static ProductoVendido TraerProductoVendido(int id)
        {
            ProductoVendido productoVendido = new();

            using (SqlConnection connection = new(General.ConnectionToString()))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * " +
                    "FROM ProductoVendido " +
                    "WHERE Id = @id";

                // Crear parametro
                var param =
                    new SqlParameter("id", System.Data.SqlDbType.BigInt);
                param.Value = id;

                // Agregar el parametro
                cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    productoVendido.Id = Convert.ToInt32(reader.GetValue(0));
                    productoVendido.Stock = Convert.ToInt32(reader.GetValue(1));
                    productoVendido.IdProducto = Convert.ToInt32(reader.GetValue(2));
                    productoVendido.IdVenta = Convert.ToInt32(reader.GetValue(3));
                }

                reader.Close();
                connection.Close();
            }

            return productoVendido;
        }

        internal static void EliminarProductoVendidoByIdProducto(int idProducto)
        {
            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM ProductoVendido WHERE IdProducto = @idProducto";

                // Crear parametro
                var param = new SqlParameter();
                param.ParameterName = "idProducto";
                param.Value = idProducto;
                param.SqlDbType = System.Data.SqlDbType.BigInt;

                // Agregar parametro
                cmd.Parameters.Add(param);
                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static void EliminarProductoVendido(int id)
        {
            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM ProductoVendido WHERE Id = @id";

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

        internal static void AgregarProductoVendido(ProductoVendido productoVendido)
        {
            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO ProductoVendido (Stock, IdProducto, IdVenta) " +
                    "VALUES (@stock, @idProducto, @idVenta)";

                // Crear parametros
                var paramStock =
                    new SqlParameter("stock", System.Data.SqlDbType.Int)
                    { Value = productoVendido.Stock };
                var paramIdProducto =
                    new SqlParameter("idProducto", System.Data.SqlDbType.BigInt)
                    { Value = productoVendido.IdProducto };
                var paramIdVenta =
                    new SqlParameter("idVenta", System.Data.SqlDbType.BigInt)
                    { Value = productoVendido.IdVenta };

                // Agregar parametro
                cmd.Parameters.Add(paramStock);
                cmd.Parameters.Add(paramIdProducto);
                cmd.Parameters.Add(paramIdVenta);

                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static void ModificarProductoVendido(ProductoVendido productoVendido)
        {
            // Buscar producto para ver si existe
            var productoVendidoBuscado = TraerProductoVendido(productoVendido.Id);

            if (productoVendidoBuscado != null && productoVendidoBuscado.Id == 0)
                return;

            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =
                    "UPDATE ProductoVendido SET Stock = @stock, IdProducto = @idProducto, IdVenta = @idVenta " +
                    "WHERE Id = @id";

                // Crear parametros
                var paramId = new SqlParameter("id", System.Data.SqlDbType.BigInt);
                paramId.Value = productoVendido.Id;

                var paramStock = new SqlParameter("stock", System.Data.SqlDbType.Int);
                paramStock.Value = productoVendido.Stock;

                var paramIdProducto = new SqlParameter("idProducto", System.Data.SqlDbType.BigInt);
                paramIdProducto.Value = productoVendido.IdProducto;

                var paramIdVenta = new SqlParameter("idVenta", System.Data.SqlDbType.BigInt);
                paramIdVenta.Value = productoVendido.IdVenta;

                // Agregar parametro
                cmd.Parameters.Add(paramId);
                cmd.Parameters.Add(paramStock);
                cmd.Parameters.Add(paramIdProducto);
                cmd.Parameters.Add(paramIdVenta);

                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
