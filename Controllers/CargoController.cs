using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Cargos")]
    public class CargoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CargoController> Logger;
        public CargoController(KalumDbContext dbContext, ILogger<CargoController> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> Get()
        {
            List<Cargo> cargos = null;

            Logger.LogDebug("Iniciando consulta de cargos en la base de datos.");
            cargos = await DbContext.Cargo.Include(c => c.CuentasPorCobrar).ToListAsync();

            if(cargos == null || cargos.Count == 0)
            {
                Logger.LogWarning("No existen cargos.");
                return new NoContentResult();
            }

            Logger.LogInformation("La peticion de ejecuto de forma exitosa.");
            return Ok(cargos);
        }

        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<Cargo>> GetCargo(string id)
        {
            Logger.LogDebug($"Inicia la busqueda del cargo con id: {id}");

            var cargo = await DbContext.Cargo.Include(c => c.CuentasPorCobrar).FirstOrDefaultAsync(c => c.CargoId == id);

            if(cargo == null)
            {
                Logger.LogWarning($"No existe el cargo con id: {id}");
                return new NoContentResult();
            }

            Logger.LogInformation("Finaliza la busqueda de forma exitosa.");
            return Ok(cargo);
        }

        [HttpPost]
        public async Task<ActionResult<Cargo>> Post([FromBody] Cargo value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un cargo nuevo");
            value.CargoId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Cargo.AddAsync(value);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Finalizando el proceso de agregar un cargo nuevo");
            return new CreatedAtRouteResult("GetCargo", new {id = value.CargoId}, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Cargo>> Delete(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de eliminacion del cargo con id {id}");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);

            if(cargo == null){
                Logger.LogWarning($"No se encontro ningun cargo con el id {id}");
                return NotFound();
            }else{
                DbContext.Cargo.Remove(cargo);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamenta el cargo con el id {id}");
                return cargo;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Cargo value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion del cargo con el id {id}");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);

            if(cargo == null){
                Logger.LogWarning($"No existe el cargo con el id {id}");
                return BadRequest();
            }

            cargo.Descripcion = value.Descripcion;
            cargo.Prefijo = value.Prefijo;
            cargo.Monto = value.Monto;
            cargo.GeneraMora = value.GeneraMora;
            cargo.PorcentajeMora = value.PorcentajeMora;
            DbContext.Entry(cargo).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
    }
}