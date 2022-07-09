using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornadas")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        public JornadaController(KalumDbContext dbContext, ILogger<JornadaController> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jornada>>> Get()
        {
            List<Jornada> jornadas = null;

            Logger.LogDebug("Iniciando la consulta de las Jornadas en la base de datos.");
            jornadas = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).ToListAsync();

            if(jornadas == null || jornadas.Count == 0)
            {
                Logger.LogWarning("No existen Jornadas");
                return new NoContentResult();
            }

            Logger.LogInformation("Se ejecuto la peticion de forma exitosa.");
            return Ok(jornadas);
        }

        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<Jornada>> GetJornada(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de busqueda para la jornada con id: {id}");

            var jornada = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).FirstOrDefaultAsync(j => j.JornadaId == id);

            if(jornada == null)
            {
                Logger.LogWarning($"No existe la jornada con id: {id}");
                return new NoContentResult();
            }

            Logger.LogInformation("Finaliza la busqueda de forma exitosa.");
            return Ok(jornada);
        }

        [HttpPost]
        public async Task<ActionResult<Jornada>> Post([FromBody] Jornada value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una jornada nueva");
            value.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(value);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Finalizando el proceso de agregar una jornada");
            return new CreatedAtRouteResult("GetJornada", new {id = value.JornadaId}, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de eliminacion de la jornada con id {id}");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);

            if(jornada == null){
                Logger.LogWarning($"No se encontro ninguna jornada con el id {id}");
                return NotFound();
            }else{
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamenta la jornada con el id {id}");
                return jornada;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Jornada value)
        {
            Logger.LogDebug($"Iniciando proceso de actualizacion de la jornada con el id {id}");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);

            if(jornada == null){
                Logger.LogWarning($"No existe la carrera tecnica con el id {id}");
                return BadRequest();
            }

            jornada.NombreCorto = value.NombreCorto;
            jornada.Descripcion = value.Descripcion;
            DbContext.Entry(jornada).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
    }
}