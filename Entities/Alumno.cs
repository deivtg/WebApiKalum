using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Alumno
    {
        public string Carne { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*@[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,5}", ErrorMessage = "El formato del correo no es valida")]
        public string Email { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<CuentaPorCobrar> CuentasPorCobrar { get; set; }
    }
}