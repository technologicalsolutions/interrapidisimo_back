using System.ComponentModel.DataAnnotations;

namespace Inter.DAL.Dto.User
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El campo Nombres es obligatorio.")]
        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo Clave es obligatorio.")]
        public string Clave { get; set; }

        [Required(ErrorMessage = "La confirmación de clave es obligatoria.")]
        [Compare("Clave", ErrorMessage = "La confirmación de clave no coincide con la clave.")]
        public string Confirmacion { get; set; }

        public string Telefono { get; set; }
    }
}
