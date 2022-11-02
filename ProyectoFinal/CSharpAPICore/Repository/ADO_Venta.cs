using CSharpAPICore.Models;
using System.Data.SqlClient;

namespace CSharpAPICore.Repository
{
    public class ADO_Venta
    {
        /// <summary>
        /// Recibe como parámetro un IdUsuario, debe traer todas las ventas de la
        /// base asignados al usuario particular.
        /// </summary>
        public static List<Venta> TraerVentas(int idUsuario)
        {
            List<Venta> ventas = new();

            using (SqlConnection connection = new(General.ConnectionToString()))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * " +
                    "FROM Venta " +
                    "WHERE IdUsuario = @idUsuario";

                // Crear parametro
                var param =
                    new SqlParameter("idUsuario", System.Data.SqlDbType.BigInt);
                param.Value = idUsuario;

                // Agregar el parametro
                cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var venta = new Venta();

                    venta.Id = Convert.ToInt32(reader.GetValue(0));
                    venta.Comentarios = reader.GetValue(1).ToString();
                    venta.IdUsuario = Convert.ToInt32(reader.GetValue(2));

                    ventas.Add(venta);
                }

                reader.Close();
                connection.Close();
            }

            return ventas;
        }

        public static Venta TraerVenta(int id)
        {
            Venta venta = new();

            using (SqlConnection connection = new(General.ConnectionToString()))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * " +
                    "FROM Venta " +
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
                    venta.Id = Convert.ToInt32(reader.GetValue(0));
                    venta.Comentarios = reader.GetValue(1).ToString();
                    venta.IdUsuario = Convert.ToInt32(reader.GetValue(2));
                }

                reader.Close();
                connection.Close();
            }

            return venta;
        }

        /// <summary>
        /// Recibe una lista de productos y el número de IdUsuario de quien la 
        /// efectuó, primero cargar una nueva Venta en la base de datos, luego 
        /// debe cargar los productos recibidos en la base de ProductosVendidos 
        /// uno por uno por un lado, y descontar el stock en la base de 
        /// productos por el otro.
        /// </summary>
        public static void AgregarVenta(List<Producto> productos, int idUsuario)
        {
            // Cargar Venta
            Venta venta = new Venta();
            venta.IdUsuario = idUsuario;
            venta.Comentarios = String.Empty;

            var idVenta = AgregarVenta(venta);

            // Cargar ProductosVendidos
            List<ProductoVendido> productoVendidos = new List<ProductoVendido>();

            foreach (Producto producto in productos)
            {
                var nuevoProductoVendido = new ProductoVendido()
                {
                    IdProducto = producto.Id,
                    IdVenta = (int)idVenta,
                    Stock = producto.Stock,
                };

                productoVendidos.Add(nuevoProductoVendido);
            }

            foreach (ProductoVendido productoVendido in productoVendidos)
            {
                ADO_ProductoVendido.AgregarProductoVendido(productoVendido);
            }

            // Modificar Productos restando stock
            foreach (Producto producto in productos)
            {
                ADO_Producto.ModificarProductoStock(producto.Id, producto.Stock);
            }
        }

        public static long AgregarVenta(Venta venta)
        {
            long nuevoId = 0;

            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =
                    "INSERT INTO Venta (Comentarios, IdUsuario) " +
                    "VALUES (@comentarios, @idUsuario); SELECT @@Identity";

                var paramComentarios = 
                    new SqlParameter("comentarios", System.Data.SqlDbType.VarChar) 
                    { Value = venta.Comentarios };

                var paramIdUsuario = 
                    new SqlParameter("idUsuario", System.Data.SqlDbType.BigInt) 
                    { Value = venta.IdUsuario};

                // Agregar parametro
                cmd.Parameters.Add(paramComentarios);
                cmd.Parameters.Add(paramIdUsuario);

                // Ejecutar comando con ExecuteScalar, devuelve el primer dato
                // devuelto, en este caso el Identity
                nuevoId = Convert.ToInt64(cmd.ExecuteScalar());

                connection.Close();
            }

            return nuevoId;
        }

        /// <summary>
        /// Recibe una venta con su número de Id, debe buscar en la base de 
        /// Productos Vendidos cuáles lo tienen eliminándolos, sumar el 
        /// stock a los productos incluidos, y eliminar la venta de la base 
        /// de datos.
        /// </summary>
        /// <param name="id"></param>
        public static void EliminarVenta(Venta venta)
        {
            // Buscar en la base de Productos Vendidos cuáles lo tienen
            var productosVendidos = ADO_ProductoVendido.TraerProductosVendidosByIdVenta(venta.Id);

            foreach (var productoVendido in productosVendidos)
            {
                // Eliminar Productos Vendidos
                ADO_ProductoVendido.EliminarProductoVendido(productoVendido.Id);
                // Sumar el stock a los productos incluidos
                ADO_Producto.ModificarProductoStock(productoVendido.IdProducto, -productoVendido.Stock);
            }

            // Eliminar la venta
            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Venta WHERE Id = @id";

                // Crear parametro
                var param = new SqlParameter();
                param.ParameterName = "id";
                param.Value = venta.Id;
                param.SqlDbType = System.Data.SqlDbType.BigInt;

                // Agregar parametro
                cmd.Parameters.Add(param);
                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void ModificarVenta(Venta venta)
        {
            // Buscar Venta para ver si existe
            var ventaBuscada = TraerVenta(venta.Id);

            if (ventaBuscada != null && ventaBuscada.Id == 0)
                return;

            using (SqlConnection connection = new SqlConnection(General.ConnectionToString()))
            {
                connection.Open();

                // Crear comando
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =
                    "UPDATE Venta SET Comentarios = @comentarios, IdUsuario = @idUsuario " +
                    "WHERE Id = @id";

                // Crear parametros
                var paramId = new SqlParameter("id", System.Data.SqlDbType.BigInt);
                paramId.Value = venta.Id;

                var paramComentarios = new SqlParameter("comentarios", System.Data.SqlDbType.VarChar);
                paramComentarios.Value = venta.Comentarios;

                var paramIdUsuario = new SqlParameter("idUsuario", System.Data.SqlDbType.BigInt);
                paramIdUsuario.Value = venta.IdUsuario;

                // Agregar parametro
                cmd.Parameters.Add(paramId);
                cmd.Parameters.Add(paramComentarios);
                cmd.Parameters.Add(paramIdUsuario);

                // Ejecutar
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
