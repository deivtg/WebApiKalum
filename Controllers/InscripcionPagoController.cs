using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/InscripcionesPagos")]
    public class InscripcionPagoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        private readonly IMapper Mapper;
        public InscripcionPagoController(KalumDbContext dbContext, ILogger<CarreraTecnicaController> logger, IMapper mapper)
        {
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionPagoListDTO>>> Get()
        {
            List<InscripcionPago> inscripcionesPago = null;
            Logger.LogDebug("Iniciando proceso de consulta de pagos de inscripcion en la base de datos");
            inscripcionesPago = await DbContext.InscripcionPago.ToListAsync();

            if(inscripcionesPago == null || inscripcionesPago.Count == 0){
                Logger.LogWarning("No existen pagos para inscripcion");
                return new NoContentResult();
            }
            
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            List<InscripcionPagoListDTO> lstInscripcionesPago = Mapper.Map<List<InscripcionPagoListDTO>>(inscripcionesPago);
            return Ok(lstInscripcionesPago);
        }

        [HttpGet("{noExpediente}", Name = "GetInscripcionPago")]
        public async Task<ActionResult<InscripcionPagoListDTO>> GetInscripcionPago(string noExpediente){
            Logger.LogDebug($"Iniciando el proceso de busqueda con el numero de expediente {noExpediente}");

            var inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.NoExpediente == noExpediente);

            if (inscripcionPago == null){
                Logger.LogWarning($"No existe el pago de inscripcion con el numero de expediente {noExpediente}");
                return new NoContentResult();
            }

            Logger.LogInformation($"Finalizando el proceso de busqueda de forma exitosa");
            InscripcionPagoListDTO pago = Mapper.Map<InscripcionPagoListDTO>(inscripcionPago);
            return Ok(pago);
        }

        [HttpGet("BoletaPago/{boletaPago}", Name = "GetBoletaPago")]
        public async Task<ActionResult<InscripcionPagoListDTO>> GetBoletaPago(string boletaPago){
            Logger.LogDebug($"Iniciando el proceso de busqueda con el numero de boleta de pago {boletaPago}");

            var inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.BoletaPago == boletaPago);

            if (inscripcionPago == null){
                Logger.LogWarning($"No existe el pago de inscripcion con el numero de expediente {boletaPago}");
                return new NoContentResult();
            }

            Logger.LogInformation($"Finalizando el proceso de busqueda de forma exitosa");
            InscripcionPagoListDTO pago = Mapper.Map<InscripcionPagoListDTO>(inscripcionPago);
            return Ok(pago);
        }

        [HttpPost]
        public async Task<ActionResult<InscripcionPago>> Post([FromBody] InscripcionPago value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una pago de inscripcion nuevo");

            await DbContext.InscripcionPago.AddAsync(value);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Finalizando el proceso de agregar un pago de inscripcion");
            return new CreatedAtRouteResult("GetInscripcionPago", new {noExpediente = value.NoExpediente}, value);
        }

        [HttpDelete("{noExpediente}")]
        public async Task<ActionResult<InscripcionPago>> Delete(string noExpediente)
        {
            Logger.LogDebug($"Iniciando el proceso de eliminacion del pago de inscripcion con el numero de expediente {noExpediente}");
            InscripcionPago inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.NoExpediente == noExpediente);

            if(inscripcionPago == null){
                Logger.LogWarning($"No se encontro ningun pago de inscripcion con el numero de expediente {noExpediente}");
                return NotFound();
            }else{
                DbContext.InscripcionPago.Remove(inscripcionPago);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamenta el pago de inscripcion con el numero de expediente {noExpediente}");
                return inscripcionPago;
            }
        }

        [HttpPut("{noExpediente}/{boletaPago}")]
        public async Task<ActionResult> Put(string noExpediente, string boletaPago, [FromBody] InscripcionPago value)
        {
            Logger.LogDebug($"Iniciando proceso de actualizacion del pago de inscripcion con el numero de expediente {noExpediente}");
            InscripcionPago inscripcionPago = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.NoExpediente == noExpediente && ip.BoletaPago == boletaPago);

            if(inscripcionPago == null){
                Logger.LogWarning($"No existe el pago de inscripcion con el numero de expediente {noExpediente}");
                return BadRequest();
            }

            inscripcionPago.FechaPago = value.FechaPago;
            inscripcionPago.Monto = value.Monto;

            DbContext.Entry(inscripcionPago).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
    }
}