using CSharpAPICore.Models;
using CSharpAPICore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CSharpAPICore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        // El constructor y ILogger se copio de la clase ejemplo
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ILogger<ProductoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Producto> Obtener(int idUsuario)
        {
            return ADO_Producto.TraerProductos(idUsuario);
        }

        [HttpDelete]
        public void Eliminar([FromBody] int id)
        {
            ADO_Producto.EliminarProducto(id);
        }

        [HttpPut]
        public void Modificar([FromBody] Producto producto)
        {
            ADO_Producto.ModificarProducto(producto);
        }

        [HttpPost]
        public void Agregar([FromBody] Producto producto)
        {
            ADO_Producto.AgregarProducto(producto);
        }
    }
}
