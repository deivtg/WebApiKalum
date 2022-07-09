using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumnos")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        public AlumnoController(KalumDbContext dbContext, ILogger<AlumnoController> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> Get()
        {
            List<Alumno> alumnos = null;

            Logger.LogDebug("Iniciando consulta de Alumnos en la base de datos.");
            alumnos = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentasPorCobrar).ToListAsync();

            if(alumnos == null || alumnos.Count == 0)
            {
                Logger.LogWarning("No existen Alumnos");
                return new NoContentResult();
            }

            Logger.LogInformation("La peticion se ejecuto de forma exitosa.");
            return Ok(alumnos);
        }

        [HttpGet("{carne}", Name = "GetAlumno")]
        public async Task<ActionResult<Alumno>> GetAlumno(string carne)
        {
            Logger.LogDebug($"Iniciando busqueda del Alumno con numero de expediente: {carne}");

            var alumno = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentasPorCobrar).FirstOrDefaultAsync(a => a.Carne == carne);

            if(alumno == null)
            {
                Logger.LogWarning($"No existe Alumno con el numero de carne: {carne}");
                return new NoContentResult();
            }

            Logger.LogInformation("Finaliza la busqueda de forma exitosa.");
            return Ok(alumno);
        }

        [HttpDelete("{carne}")]
        public async Task<ActionResult<Alumno>> Delete(string carne){
            Logger.LogDebug($"Iniciando el proceso de eliminacion del alumno con carne {carne}");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == carne);

            if(alumno == null){
                Logger.LogWarning($"No encontro ningun alumno con el carne {carne}");
                return NotFound();
            }else{
                DbContext.Alumno.Remove(alumno);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el alumno con el carne {carne}");
                return alumno;
            }
        }

        [HttpPut("{carne}")]
        public async Task<ActionResult> Put(string carne, [FromBody] Alumno value){
            Logger.LogDebug($"Iniciando el proceso de actualizacion del Alumno con carne {carne}");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == carne);

            if(alumno == null){
                Logger.LogWarning($"No existe el alumno con el carne {carne}");
                return BadRequest();
            }

            alumno.Apellidos = value.Apellidos;
            alumno.Nombres = value.Nombres;
            alumno.Direccion = value.Direccion;
            alumno.Telefono = value.Telefono;
            alumno.Email = value.Email;
            DbContext.Entry(alumno).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido guardados correctamente");
            return NoContent();
        }
    }
}