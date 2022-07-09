using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class InscripcionPago
    {
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string BoletaPago { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "El campo numero de expediente debe ser de 12 caracteres")]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Anio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public DateTime FechaPago { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public decimal Monto { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}