using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppController : ControllerBase
    {
        // El constructor y ILogger se copio de la clase ejemplo
        private readonly ILogger<AppController> _logger;

        public AppController(ILogger<AppController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Se debe enviar un JSON al front que contenga únicamente un string con el Nombre de la App a elección.
        /// </summary>
        [HttpGet("TraerNombre")]
        public string TraerNombre()
        {
            return "Coderhouse - Sistema Gestion API 2022";
        }
    }
}
