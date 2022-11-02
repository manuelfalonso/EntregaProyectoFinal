using CSharpAPICore.Models;
using CSharpAPICore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CSharpAPICore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoVendidoController : ControllerBase
    {
        // El constructor y ILogger se copio de la clase ejemplo
        private readonly ILogger<ProductoVendidoController> _logger;

        public ProductoVendidoController(ILogger<ProductoVendidoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<ProductoVendido> Obtener(int idUsuario)
        {
            return ADO_ProductoVendido.TraerProductosVendidos(idUsuario);
        }

        [HttpDelete]
        public void Eliminar([FromBody] int id)
        {
            ADO_ProductoVendido.EliminarProductoVendido(id);
        }

        [HttpPut]
        public void Modificar([FromBody] ProductoVendido productoVendido)
        {
            ADO_ProductoVendido.ModificarProductoVendido(productoVendido);
        }

        [HttpPost]
        public void Agregar([FromBody] ProductoVendido productoVendido)
        {
            ADO_ProductoVendido.AgregarProductoVendido(productoVendido);
        }
    }
}
