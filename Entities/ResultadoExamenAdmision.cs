namespace WebApiKalum.Entities
{
    public class ResultadoExamenAdmision
    {
        public string NoExpediente { get; set; }
        public string Anio { get; set; }
        public string Descipcion { get; set; }
        public int Nota { get; set; }
        public virtual Aspirante Aspirante { get; set; }
    }
}