using AutoMapper;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<CarreraTecnicaCreateDTO, CarreraTecnica>();
            CreateMap<CarreraTecnica, CarreraTecnicaCreateDTO>();
            CreateMap<Jornada, JornadaCreateDTO>();
            CreateMap<ExamenAdmision, ExamenAdmisionCreateDTO>();
            CreateMap<Aspirante, AspiranteListDTO>().ConstructUsing(e => new AspiranteListDTO{NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<Aspirante, AspiranteCreateDTO>().ConstructUsing(a => new AspiranteCreateDTO{NombreCompleto = $"{a.Apellidos} {a.Nombres}"});
            CreateMap<Inscripcion, InscripcionesCreateDTO>();
            CreateMap<CarreraTecnica, CarreraTecnicaListDTO>();
            CreateMap<InscripcionPago, InscripcionPagoListDTO>();
        }
    }
}