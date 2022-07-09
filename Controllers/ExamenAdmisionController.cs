using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ExamenesAdmision")]
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;
        public ExamenAdmisionController(KalumDbContext dbContext, ILogger<ExamenAdmisionController> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmision>>> Get()
        {
            List<ExamenAdmision> examenAdmision = null;

            Logger.LogDebug("Iniciando la consulta de los Examenes de Admision en la base de datos.");
            examenAdmision = await DbContext.ExamenAdmision.Include(e => e.Aspirantes).ToListAsync();

            if(examenAdmision == null || examenAdmision.Count == 0)
            {
                Logger.LogWarning("No existen examenes de admision.");
                return new NoContentResult();
            }

            Logger.LogInformation("Se ejecuto la peticion de forma exitosa.");
            return Ok(examenAdmision);
        }

        [HttpGet("{id}", Name = "GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmision>> GetExamenAdmision(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de busqueda para el examen de admision con id: {id}");

            var examenAdmision = await DbContext.ExamenAdmision.Include(e => e.Aspirantes).FirstOrDefaultAsync(e => e.ExamenId == id);

            if(examenAdmision == null)
            {
                Logger.LogWarning("No existe examen de admision");
                return new NoContentResult();
            }

            Logger.LogInformation("Finaliza la busqueda de forma exitosa.");
            return Ok(examenAdmision);
        }

        [HttpPost]
        public async Task<ActionResult<ExamenAdmision>> Post([FromBody] ExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un examen de admision nuevo");
            value.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Finalizando el proceso de agregar un examen de admision");
            return new CreatedAtRouteResult("GetExamenAdmision", new {id = value.ExamenId}, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ExamenAdmision>> Delete(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de eliminacion del examen de admision con id {id}");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == id);

            if(examenAdmision == null){
                Logger.LogWarning($"No se encontro ningun examen de admision con el id {id}");
                return NotFound();
            }else{
                DbContext.ExamenAdmision.Remove(examenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamenta el examen de admision con el id {id}");
                return examenAdmision;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando proceso de actualizacion del examen de admision con el id {id}");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == id);

            if(examenAdmision == null){
                Logger.LogWarning($"No existe el examen de admision con el id {id}");
                return BadRequest();
            }

            examenAdmision.FechaExamen = value.FechaExamen;
            DbContext.Entry(examenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
    }
}