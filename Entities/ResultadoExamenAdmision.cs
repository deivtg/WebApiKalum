using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class ResultadoExamenAdmision
    {
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "El campo numero de expediente debe ser de 12 caracteres")]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Anio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public int Nota { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}