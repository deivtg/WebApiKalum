using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Cargo
    {
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string CargoId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public string Prefijo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public decimal Monto { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public bool GeneraMora { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public int PorcentajeMora { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido}")]
        public virtual List<CuentaPorCobrar> CuentasPorCobrar { get; set; }
    }
}