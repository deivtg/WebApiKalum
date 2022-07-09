using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class ExamenAdmisionCreateDTO
    {
        [DataType(DataType.Date)]
        public DateTime FechaExamen { get; set; }
    }
}