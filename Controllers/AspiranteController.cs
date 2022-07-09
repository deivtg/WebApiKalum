using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Aspirantes")]
    public class AspiranteController : ControllerBase
    {

        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        private readonly IMapper Mapper;

        public AspiranteController(KalumDbContext dbContext, ILogger<AlumnoController> logger, IMapper mapper)
        {
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Aspirante>> Post([FromBody] Aspirante value)
        {
            Logger.LogDebug("Iniciando proceso pra almacenar un registro de aspirante");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);

            if(carreraTecnica == null){
                Logger.LogInformation($"No existe la carrera tecnica con el id {value.CarreraId}");
                return BadRequest();
            }

            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);

            if(jornada == null){
                Logger.LogInformation($"No existe la jornada con el id {value.JornadaId}");
                return BadRequest();
            }

            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == value.ExamenId);
            
            if(examenAdmision == null){
                Logger.LogInformation($"No existe el examen de admision con el id {value.ExamenId}");
                return BadRequest();
            }

            await DbContext.Aspirante.AddAsync(value);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Se ha creado el aspirante con exito");

            return Ok(value);
        }

        [HttpGet]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta de aspirante");
            List<Aspirante> list = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include(a => a.ExamenAdmision).ToListAsync();

            if(list == null || list.Count == 0)
            {
                return new NoContentResult();
            }

            List<AspiranteListDTO> aspirantes = Mapper.Map<List<AspiranteListDTO>>(list);
            return Ok(aspirantes);
        }
    }
}