namespace Curso.Domain.AspIdentity.DTOs.Requests
{
    public class UsuarioPatchRequestDto
    {
        public string? Username { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public int? Edad { get; set; }
    }
}
