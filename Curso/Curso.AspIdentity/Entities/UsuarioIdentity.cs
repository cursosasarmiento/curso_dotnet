using Microsoft.AspNetCore.Identity;

namespace Curso.AspIdentity.Entities
{
    public class UsuarioIdentity : IdentityUser
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int? Edad { get; set; }
    }
}
