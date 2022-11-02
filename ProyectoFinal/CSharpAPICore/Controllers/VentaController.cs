using CSharpAPICore.Models;
using CSharpAPICore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CSharpAPICore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        // El constructor y ILogger se copio de la clase ejemplo
        private readonly ILogger<VentaController> _logger;

        public VentaController(ILogger<VentaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Venta> Obtener(int idUsuario)
        {
            return ADO_Venta.TraerVentas(idUsuario);
        }

        [HttpPost("PostVentaByProductos")]
        public void Agregar([FromBody] List<Producto> productos, int idUsuario)
        {
            ADO_Venta.AgregarVenta(productos, idUsuario);
        }

        [HttpPost("PostVenta")]
        public void Agregar([FromBody] Venta venta)
        {
            ADO_Venta.AgregarVenta(venta);
        }

        [HttpDelete]
        public void Eliminar([FromBody] Venta venta)
        {
            ADO_Venta.EliminarVenta(venta);
        }

        [HttpPut]
        public void Modificar([FromBody] Venta venta)
        {
            ADO_Venta.ModificarVenta(venta);
        }
    }
}
