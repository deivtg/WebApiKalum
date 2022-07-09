using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Alumno
    {
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Carne { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Telefono { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*@[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,5}", ErrorMessage = "El formato del correo no es valida")]
        public string Email { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<CuentaPorCobrar> CuentasPorCobrar { get; set; }
    }
}