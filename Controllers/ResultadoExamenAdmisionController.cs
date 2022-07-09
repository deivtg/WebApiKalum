using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ResultadosExamenesAdmision")]
    public class ResultadoExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        private readonly IMapper Mapper;
        public ResultadoExamenAdmisionController(KalumDbContext dbContext, ILogger<CarreraTecnicaController> logger, IMapper mapper)
        {
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmision>>> Get()
        {
            List<ResultadoExamenAdmision> resultadosExmanenAdmision = null;
            Logger.LogDebug("Iniciando proceso de consulta de resultados examen de admision");
            resultadosExmanenAdmision = await DbContext.ResultadoExamenAdmisions.ToListAsync();

            if(resultadosExmanenAdmision == null || resultadosExmanenAdmision.Count == 0){
                Logger.LogWarning("No existen resultados de examen de admision");
                return new NoContentResult();
            }
            
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            
            return Ok(resultadosExmanenAdmision);
        }

        [HttpGet("{noExpediente}", Name = "GetResultadosExamenesAdmision")]
        public async Task<ActionResult<ResultadoExamenAdmision>> GetResultadosExamenesAdmision(string noExpediente){
            Logger.LogDebug($"Iniciando el proceso de busqueda con el numero de expediente {noExpediente}");

            var resultadoExamenAdmision = await DbContext.ResultadoExamenAdmisions.FirstOrDefaultAsync(ea => ea.NoExpediente == noExpediente);

            if (resultadoExamenAdmision == null){
                Logger.LogWarning($"No existe el examen de admision con el numero de expediente {noExpediente}");
                return new NoContentResult();
            }

            Logger.LogInformation($"Finalizando el proceso de busqueda de forma exitosa");
            
            return Ok(resultadoExamenAdmision);
        }

        [HttpPost]
        public async Task<ActionResult<ResultadoExamenAdmision>> Post([FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una resultado de examen de admision nuevo");

            await DbContext.ResultadoExamenAdmisions.AddAsync(value);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Finalizando el proceso de agregar un resultado de examen de admision");
            return new CreatedAtRouteResult("GetResultadosExamenesAdmision", new {noExpediente = value.NoExpediente}, value);
        }

        [HttpDelete("{noExpediente}")]
        public async Task<ActionResult<ResultadoExamenAdmision>> Delete(string noExpediente)
        {
            Logger.LogDebug($"Iniciando el proceso de eliminacion de resultado de examen de admision con el numero de expediente {noExpediente}");
            ResultadoExamenAdmision resultadoExamenAdmision = await DbContext.ResultadoExamenAdmisions.FirstOrDefaultAsync(re => re.NoExpediente == noExpediente);

            if(resultadoExamenAdmision == null){
                Logger.LogWarning($"No se encontro ningun resultado de examen de admision con el numero de expediente {noExpediente}");
                return NotFound();
            }else{
                DbContext.ResultadoExamenAdmisions.Remove(resultadoExamenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamenta el pago de inscripcion con el numero de expediente {noExpediente}");
                return resultadoExamenAdmision;
            }
        }

        [HttpPut("{noExpediente}")]
        public async Task<ActionResult> Put(string noExpediente, [FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando proceso de actualizacion del resultado de examen de admision con el numero de expediente {noExpediente}");
            ResultadoExamenAdmision resultadoExamenAdmision = await DbContext.ResultadoExamenAdmisions.FirstOrDefaultAsync(ip => ip.NoExpediente == noExpediente);

            if(resultadoExamenAdmision == null){
                Logger.LogWarning($"No existe el resultado de examen de admision con el numero de expediente {noExpediente}");
                return BadRequest();
            }

            resultadoExamenAdmision.Descripcion = value.Descripcion;
            resultadoExamenAdmision.Nota = value.Nota;

            DbContext.Entry(resultadoExamenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
    }
}