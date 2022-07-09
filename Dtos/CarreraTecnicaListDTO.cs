namespace WebApiKalum.Dtos
{
    public class CarreraTecnicaListDTO
    {
        public string CarreraId { get; set; }
        public string Nombre { get; set; }
        public List<AspiranteCreateDTO> Aspirantes { get; set; }
        public List<InscripcionesCreateDTO> Inscripciones { get; set; }
    }
}