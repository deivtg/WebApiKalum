using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum
{
    public class KalumDbContext : DbContext
    {
        public DbSet<Alumno> Alumno { get; set; }
        public DbSet<Inscripcion> Inscripcion { get; set; }
        public DbSet<CarreraTecnica> CarreraTecnica { get; set; }
        public DbSet<Jornada> Jornada { get; set; }
        public DbSet<ExamenAdmision> ExamenAdmision { get; set; }
        public DbSet<Aspirante> Aspirante { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<CuentaPorCobrar> CuentaPorCobrar { get; set; }
        public DbSet<InversionCarreraTecnica> InversionCarreraTecnicas { get; set; }
        public DbSet<InscripcionPago> InscripcionPago { get; set;}
        public DbSet<ResultadoExamenAdmision> ResultadoExamenAdmisions { get; set; }

        public KalumDbContext(DbContextOptions options) : base(options){

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Alumno>().ToTable("Alumno").HasKey(a => new {a.Carne});
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion").HasKey(i => new {i.InscripcionId});
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new {ct.CarreraId});
            modelBuilder.Entity<Jornada>().ToTable("Jornada").HasKey(j => new {j.JornadaId});
            modelBuilder.Entity<ExamenAdmision>().ToTable("ExamenAdmision").HasKey(e => new {e.ExamenId});
            modelBuilder.Entity<Aspirante>().ToTable("Aspirante").HasKey(a => new {a.NoExpediente});
            modelBuilder.Entity<Cargo>().ToTable("Cargo").HasKey(c => new {c.CargoId});
            modelBuilder.Entity<CuentaPorCobrar>().ToTable("CuentaPorCobrar").HasKey(c => new {c.Codigo, c.Anio, c.Carne});
            modelBuilder.Entity<InversionCarreraTecnica>().ToTable("InversionCarreraTecnica").HasKey(i => new {i.InversionId});
            modelBuilder.Entity<InscripcionPago>().ToTable("InscripcionPago").HasKey(i => new {i.BoletaPago, i.NoExpediente, i.Anio});
            modelBuilder.Entity<ResultadoExamenAdmision>().ToTable("ResultadoExamenAdmision").HasKey(r => new {r.NoExpediente, r.Anio});
            
            modelBuilder.Entity<Aspirante>()
                .HasOne<ExamenAdmision>(a => a.ExamenAdmision)
                .WithMany(e => e.Aspirantes)
                .HasForeignKey(a => a.ExamenId);
            
            modelBuilder.Entity<Aspirante>()
                .HasOne<CarreraTecnica>(a => a.CarreraTecnica)
                .WithMany(ct => ct.Aspirantes)
                .HasForeignKey(a => a.CarreraId);
                
            modelBuilder.Entity<Aspirante>()
                .HasOne<Jornada>(a => a.Jornada)
                .WithMany(j => j.Aspirantes)
                .HasForeignKey(a => a.JornadaId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<CarreraTecnica>(i => i.CarreraTecnica)
                .WithMany(ct => ct.Inscripciones)
                .HasForeignKey(i => i.CarreraId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<Jornada>(i => i.Jornada)
                .WithMany(j => j.Inscripciones)
                .HasForeignKey(i => i.JornadaId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<Alumno>(i => i.Alumno)
                .WithMany(a => a.Inscripciones)
                .HasForeignKey(i => i.Carne);

            modelBuilder.Entity<CuentaPorCobrar>()
                .HasOne<Cargo>(cxc => cxc.Cargo)
                .WithMany(c => c.CuentasPorCobrar)
                .HasForeignKey(cxc => cxc.CargoId);

            modelBuilder.Entity<CuentaPorCobrar>()
                .HasOne<Alumno>(cxc => cxc.Alumno)
                .WithMany(a => a.CuentasPorCobrar)
                .HasForeignKey(cxc => cxc.Carne);

            modelBuilder.Entity<InversionCarreraTecnica>()
                .HasOne<CarreraTecnica>(i => i.CarreraTecnica)
                .WithMany(ct => ct.InversionesCarreraTecnica)
                .HasForeignKey(i => i.CarreraId);

            modelBuilder.Entity<InscripcionPago>()
                .HasOne<Aspirante>(i => i.Aspirante)
                .WithMany(a => a.InscripcionesPago)
                .HasForeignKey(i => i.NoExpediente);

            modelBuilder.Entity<ResultadoExamenAdmision>()
                .HasOne<Aspirante>(r => r.Aspirante)
                .WithMany(a => a.ResultadosExamenAdmision)
                .HasForeignKey(r => r.NoExpediente);
        }
    }
}