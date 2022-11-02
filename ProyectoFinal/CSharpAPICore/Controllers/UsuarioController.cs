using CSharpAPICore.Models;
using CSharpAPICore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CSharpAPICore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        // El constructor y ILogger se copio de la clase ejemplo
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetUsuarios")]
        public IEnumerable<Usuario> Obtener()
        {
            return ADO_Usuario.DevolverUsuarios();
        }

        [HttpGet("GetUsuarioById")]
        public Usuario Obtener(int id)
        {
            return ADO_Usuario.DevolverUsuario(id);
        }

        [HttpGet("GetUsuarioByNombreUsuario")]
        public Usuario Obtener(string nombreUsuario)
        {
            return ADO_Usuario.TraerUsuario(nombreUsuario);
        }

        // http://localhost:5000/api/Usuario/InicioSesion?nombreUsuario=malonso&contraseña=coderhouse123
        [HttpGet("InicioSesion")]
        public Usuario Obtener(string nombreUsuario, string contraseña)
        {
            return ADO_Usuario.InicioDeSesion(nombreUsuario, contraseña);
        }

        [HttpDelete]
        public void Eliminar([FromBody] int id)
        {
            ADO_Usuario.EliminarUsuario(id);
        }

        [HttpPut]
        public void Modificar([FromBody] Usuario usuario)
        {
            ADO_Usuario.ModificarUsuario(usuario);
        }

        [HttpPost]
        public void Agregar([FromBody] Usuario usuario)
        {
            ADO_Usuario.AgregarUsuario(usuario);
        }
    }
}