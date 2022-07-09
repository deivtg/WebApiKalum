using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CarrerasTecnicas")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        private readonly IMapper Mapper;
        public CarreraTecnicaController(KalumDbContext dbContext, ILogger<CarreraTecnicaController> logger, IMapper mapper)
        {
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarreraTecnicaListDTO>>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            Logger.LogDebug("Iniciando proceso de consulta de carreras tecnicas en la base de datos");
            carrerasTecnicas = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).ToListAsync();

            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0){
                Logger.LogWarning("No existen carreras tecnicas");
                return new NoContentResult();
            }

            List<CarreraTecnicaListDTO> carreras = Mapper.Map<List<CarreraTecnicaListDTO>>(carrerasTecnicas);
            
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(carreras);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<CarreraTecnica>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.CarreraTecnica.Include(ct => ct.Aspirantes).Include(ct => ct.Inscripciones).AsQueryable();
            var paginacion = new HttpResponsePaginacion<CarreraTecnica>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0)
            {
                return NoContent();
            }
            else{
                return Ok(paginacion);
            }
        }

        [HttpGet("{id}", Name = "GetCarreraTecnica")]
        public async Task<ActionResult<CarreraTecnica>> GetCarreraTecnica(string id){
            Logger.LogDebug($"Iniciando el proceso de busqueda con el id {id}");

            var carrera = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).FirstOrDefaultAsync(ct => ct.CarreraId == id);

            if (carrera == null){
                Logger.LogWarning($"No existe la carrera tecnica con el id {id}");
                return new NoContentResult();
            }

            Logger.LogInformation($"Finalizando el proceso de busqueda de forma exitosa");
            return Ok(carrera);
        }

        [HttpPost]
        public async Task<ActionResult<CarreraTecnica>> Post([FromBody] CarreraTecnicaCreateDTO value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una carrera tenica nueva");

            CarreraTecnica nuevo = Mapper.Map<CarreraTecnica>(value);

            nuevo.CarreraId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.CarreraTecnica.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Finalizando el proceso de agregar una carrera tecnica");
            return new CreatedAtRouteResult("GetCarreraTecnica", new {id = nuevo.CarreraId}, nuevo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CarreraTecnica>> Delete(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de eliminacion de carrera tecnica con id {id}");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);

            if(carreraTecnica == null){
                Logger.LogWarning($"No se encontro ninguna carrera tecnica con el id {id}");
                return NotFound();
            }else{
                DbContext.CarreraTecnica.Remove(carreraTecnica);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamenta la carrera tecnica con el id {id}");
                return carreraTecnica;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] CarreraTecnica value)
        {
            Logger.LogDebug($"Iniciando proceso de actualizacion de la carrera tenica con el id {id}");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);

            if(carreraTecnica == null){
                Logger.LogWarning($"No existe la carrera tecnica con el id {id}");
                return BadRequest();
            }

            carreraTecnica.Nombre = value.Nombre;
            DbContext.Entry(carreraTecnica).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
    }
}