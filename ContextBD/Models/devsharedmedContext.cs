using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContextBD.Models
{
    public partial class devsharedmedContext : DbContext
    {
        public devsharedmedContext()
        {
        }

        public devsharedmedContext(DbContextOptions<devsharedmedContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bancos> Bancos { get; set; }
        public virtual DbSet<Clinicas> Clinicas { get; set; }
        public virtual DbSet<EspecialidadMadre> EspecialidadMadre { get; set; }
        public virtual DbSet<ExperticiasEspecialidades> ExperticiasEspecialidades { get; set; }
        public virtual DbSet<ExperticiaUser> ExperticiaUser { get; set; }
        public virtual DbSet<Nacionalidad> Nacionalidad { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<PrecioHoraAtencion> PrecioHoraAtencion { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Servicio> Servicio { get; set; }
        public virtual DbSet<SubEspAnMedCr> SubEspAnMedCr { get; set; }
        public virtual DbSet<SubEspGobs> SubEspGobs { get; set; }
        public virtual DbSet<SubEspMed> SubEspMed { get; set; }
        public virtual DbSet<SubEspOdo> SubEspOdo { get; set; }
        public virtual DbSet<SubEspPed> SubEspPed { get; set; }
        public virtual DbSet<SubEspQx> SubEspQx { get; set; }
        public virtual DbSet<SubEspRad> SubEspRad { get; set; }
        public virtual DbSet<SubEspTec> SubEspTec { get; set; }
        public virtual DbSet<TipoUser> TipoUser { get; set; }
        public virtual DbSet<UserBanco> UserBanco { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server= sharedmed.database.windows.net;Initial Catalog= devsharedmed;Persist Security Info=False;User ID=sharedmed;Password=shapp2020-;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bancos>(entity =>
            {
                entity.HasKey(e => e.IdBanco);

                entity.Property(e => e.IdBanco).HasColumnName("idBanco");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<Clinicas>(entity =>
            {
                entity.HasKey(e => e.IdClinica);

                entity.Property(e => e.IdClinica).HasColumnName("idClinica");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion");

                entity.Property(e => e.IdPais).HasColumnName("idPais");

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Clinicas)
                    .HasForeignKey(d => d.IdPais)
                    .HasConstraintName("FK_Clinicas_Pais");
            });

            modelBuilder.Entity<EspecialidadMadre>(entity =>
            {
                entity.HasKey(e => e.IdEspMad);

                entity.Property(e => e.IdEspMad).HasColumnName("idEspMad");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExperticiasEspecialidades>(entity =>
            {
                entity.HasKey(e => e.IdExp);

                entity.Property(e => e.IdExp).HasColumnName("idExp");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion");

                entity.Property(e => e.IdEspMad).HasColumnName("idEspMad");

                entity.Property(e => e.IdSubEspAnMedCr).HasColumnName("idSubEspAnMedCr");

                entity.Property(e => e.IdSubEspGobs).HasColumnName("idSubEspGObs");

                entity.Property(e => e.IdSubEspMed).HasColumnName("idSubEspMed");

                entity.Property(e => e.IdSubEspOdo).HasColumnName("idSubEspOdo");

                entity.Property(e => e.IdSubEspPed).HasColumnName("idSubEspPed");

                entity.Property(e => e.IdSubEspQx).HasColumnName("idSubEspQx");

                entity.Property(e => e.IdSubEspRad).HasColumnName("idSubEspRad");

                entity.Property(e => e.IdSubEspTec).HasColumnName("idSubEspTec");

                entity.HasOne(d => d.IdEspMadNavigation)
                    .WithMany(p => p.ExperticiasEspecialidades)
                    .HasForeignKey(d => d.IdEspMad)
                    .HasConstraintName("FK_ExperticiasEspecialidades_EspecialidadMadre");

                entity.HasOne(d => d.IdSubEspAnMedCrNavigation)
                    .WithMany(p => p.ExperticiasEspecialidades)
                    .HasForeignKey(d => d.IdSubEspAnMedCr)
                    .HasConstraintName("FK_ExperticiasEspecialidades_SubEspAnMedCr");

                entity.HasOne(d => d.IdSubEspGobsNavigation)
                    .WithMany(p => p.ExperticiasEspecialidades)
                    .HasForeignKey(d => d.IdSubEspGobs)
                    .HasConstraintName("FK_ExperticiasEspecialidades_SubEspGObs");

                entity.HasOne(d => d.IdSubEspMedNavigation)
                    .WithMany(p => p.ExperticiasEspecialidades)
                    .HasForeignKey(d => d.IdSubEspMed)
                    .HasConstraintName("FK_ExperticiasEspecialidades_SubEspMed");

                entity.HasOne(d => d.IdSubEspOdoNavigation)
                    .WithMany(p => p.ExperticiasEspecialidades)
                    .HasForeignKey(d => d.IdSubEspOdo)
                    .HasConstraintName("FK_ExperticiasEspecialidades_SubEspOdo");

                entity.HasOne(d => d.IdSubEspPedNavigation)
                    .WithMany(p => p.ExperticiasEspecialidades)
                    .HasForeignKey(d => d.IdSubEspPed)
                    .HasConstraintName("FK_ExperticiasEspecialidades_SubEspPed");

                entity.HasOne(d => d.IdSubEspQxNavigation)
                    .WithMany(p => p.ExperticiasEspecialidades)
                    .HasForeignKey(d => d.IdSubEspQx)
                    .HasConstraintName("FK_ExperticiasEspecialidades_SubEspQx");

                entity.HasOne(d => d.IdSubEspRadNavigation)
                    .WithMany(p => p.ExperticiasEspecialidades)
                    .HasForeignKey(d => d.IdSubEspRad)
                    .HasConstraintName("FK_ExperticiasEspecialidades_SubEspRad");

                entity.HasOne(d => d.IdSubEspTecNavigation)
                    .WithMany(p => p.ExperticiasEspecialidades)
                    .HasForeignKey(d => d.IdSubEspTec)
                    .HasConstraintName("FK_ExperticiasEspecialidades_SubEspTec");
            });

            modelBuilder.Entity<ExperticiaUser>(entity =>
            {
                entity.HasKey(e => e.IdExpUser);

                entity.Property(e => e.IdExpUser).HasColumnName("idExpUser");

                entity.Property(e => e.IdExp).HasColumnName("idExp");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Result)
                    .IsRequired()
                    .HasColumnName("result");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.ExperticiaUser)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExperticiaUser_Users");
            });

            modelBuilder.Entity<Nacionalidad>(entity =>
            {
                entity.HasKey(e => e.IdNacionalidad);

                entity.Property(e => e.IdNacionalidad)
                    .HasColumnName("idNacionalidad")
                    .ValueGeneratedNever();

                entity.Property(e => e.GentilicioNac)
                    .IsRequired()
                    .HasColumnName("gentilicioNac");

                entity.Property(e => e.IsoNac)
                    .IsRequired()
                    .HasColumnName("isoNac");

                entity.Property(e => e.PaisNac)
                    .IsRequired()
                    .HasColumnName("paisNac");
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.HasKey(e => e.IdPais);

                entity.Property(e => e.IdPais)
                    .HasColumnName("idPais")
                    .ValueGeneratedNever();

                entity.Property(e => e.Iso)
                    .IsRequired()
                    .HasColumnName("iso")
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrecioHoraAtencion>(entity =>
            {
                entity.HasKey(e => e.IdPrecio);

                entity.Property(e => e.IdPrecio).HasColumnName("idPrecio");

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => e.IdRegion);

                entity.Property(e => e.IdRegion).HasColumnName("idRegion");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.HasKey(e => e.IdServicio);

                entity.Property(e => e.IdServicio).HasColumnName("idServicio");

                entity.Property(e => e.Descripcioncaso).HasColumnName("descripcioncaso");

                entity.Property(e => e.Experticias).HasColumnName("experticias");

                entity.Property(e => e.IdEspMad).HasColumnName("idEspMad");

                entity.Property(e => e.IdSubEsp).HasColumnName("idSubEsp");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdUserService).HasColumnName("idUserService");

                entity.Property(e => e.Idioma).HasColumnName("idioma");

                entity.Property(e => e.Prioridad).HasColumnName("prioridad");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(50);

                entity.Property(e => e.Tiposervicio).HasColumnName("tiposervicio");

                entity.Property(e => e.Tituloservicio).HasColumnName("tituloservicio");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Servicio)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Servicio_Users");
            });

            modelBuilder.Entity<SubEspAnMedCr>(entity =>
            {
                entity.HasKey(e => e.IdSubEspAnMedCr);

                entity.Property(e => e.IdSubEspAnMedCr).HasColumnName("idSubEspAnMedCr");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<SubEspGobs>(entity =>
            {
                entity.HasKey(e => e.IdSubEspGobs);

                entity.ToTable("SubEspGObs");

                entity.Property(e => e.IdSubEspGobs).HasColumnName("idSubEspGObs");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubEspMed>(entity =>
            {
                entity.HasKey(e => e.IdSubEspMed);

                entity.Property(e => e.IdSubEspMed).HasColumnName("idSubEspMed");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubEspOdo>(entity =>
            {
                entity.HasKey(e => e.IdSubEspOdo);

                entity.Property(e => e.IdSubEspOdo).HasColumnName("idSubEspOdo");

                entity.Property(e => e.Descrpcion)
                    .IsRequired()
                    .HasColumnName("descrpcion");
            });

            modelBuilder.Entity<SubEspPed>(entity =>
            {
                entity.HasKey(e => e.IdSubEspPed);

                entity.Property(e => e.IdSubEspPed).HasColumnName("idSubEspPed");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubEspQx>(entity =>
            {
                entity.HasKey(e => e.IdSubEspQx);

                entity.Property(e => e.IdSubEspQx).HasColumnName("idSubEspQx");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubEspRad>(entity =>
            {
                entity.HasKey(e => e.IdSubEspRad);

                entity.Property(e => e.IdSubEspRad).HasColumnName("idSubEspRad");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<SubEspTec>(entity =>
            {
                entity.HasKey(e => e.IdSubEspTec);

                entity.Property(e => e.IdSubEspTec).HasColumnName("idSubEspTec");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<TipoUser>(entity =>
            {
                entity.HasKey(e => e.IdTipoUser);

                entity.Property(e => e.IdTipoUser).HasColumnName("idTipoUser");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionInterna)
                    .IsRequired()
                    .HasColumnName("descripcionInterna")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserBanco>(entity =>
            {
                entity.HasKey(e => e.IdUserBanco);

                entity.Property(e => e.IdUserBanco).HasColumnName("idUserBanco");

                entity.Property(e => e.IdBanco).HasColumnName("idBanco");

                entity.Property(e => e.IdPrecio).HasColumnName("idPrecio");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.NroCuenta).IsRequired();

                entity.HasOne(d => d.IdBancoNavigation)
                    .WithMany(p => p.UserBanco)
                    .HasForeignKey(d => d.IdBanco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBanco_Bancos");

                entity.HasOne(d => d.IdPrecioNavigation)
                    .WithMany(p => p.UserBanco)
                    .HasForeignKey(d => d.IdPrecio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBanco_PrecioHoraAtencion");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserBanco)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBanco_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Disponible).HasColumnName("disponible");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .IsUnicode(false);

                entity.Property(e => e.FechaNac)
                    .HasColumnName("fechaNac")
                    .HasColumnType("date");

                entity.Property(e => e.Helper).HasColumnName("helper");

                entity.Property(e => e.IdClinica).HasColumnName("idClinica");

                entity.Property(e => e.IdEspMad).HasColumnName("idEspMad");

                entity.Property(e => e.IdNacionalidad).HasColumnName("idNacionalidad");

                entity.Property(e => e.IdPais).HasColumnName("idPais");

                entity.Property(e => e.IdRegion).HasColumnName("idRegion");

                entity.Property(e => e.IdSubEspAnMedCr).HasColumnName("idSubEspAnMedCr");

                entity.Property(e => e.IdSubEspGobs).HasColumnName("idSubEspGObs");

                entity.Property(e => e.IdSubEspMed).HasColumnName("idSubEspMed");

                entity.Property(e => e.IdSubEspOdo).HasColumnName("idSubEspOdo");

                entity.Property(e => e.IdSubEspPed).HasColumnName("idSubEspPed");

                entity.Property(e => e.IdSubEspQx).HasColumnName("idSubEspQx");

                entity.Property(e => e.IdSubEspRad).HasColumnName("idSubEspRad");

                entity.Property(e => e.IdSubEspTec).HasColumnName("idSubEspTec");

                entity.Property(e => e.IdTipoUser).HasColumnName("idTipoUser");

                entity.Property(e => e.Idioma)
                    .IsRequired()
                    .HasColumnName("idioma");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasColumnName("nombres")
                    .IsUnicode(false);

                entity.Property(e => e.NroCelular)
                    .IsRequired()
                    .HasColumnName("nroCelular")
                    .IsUnicode(false);

                entity.Property(e => e.OtraEsp)
                    .HasColumnName("otraEsp")
                    .IsUnicode(false);

                entity.Property(e => e.OtroLugTrab)
                    .HasColumnName("otroLugTrab")
                    .IsUnicode(false);

                entity.Property(e => e.PApellido)
                    .IsRequired()
                    .HasColumnName("pApellido")
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .IsUnicode(false);

                entity.Property(e => e.RutPass)
                    .IsRequired()
                    .HasColumnName("rutPass")
                    .IsUnicode(false);

                entity.Property(e => e.SApellido)
                    .IsRequired()
                    .HasColumnName("sApellido")
                    .IsUnicode(false);

                entity.Property(e => e.SaltCode)
                    .IsRequired()
                    .HasColumnName("saltCode")
                    .IsUnicode(false);

                entity.Property(e => e.Sexo)
                    .IsRequired()
                    .HasColumnName("sexo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClinicaNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdClinica)
                    .HasConstraintName("FK_Users_Clinicas");

                entity.HasOne(d => d.IdEspMadNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdEspMad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_EspecialidadMadre");

                entity.HasOne(d => d.IdNacionalidadNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdNacionalidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Nacionalidad");

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Pais");

                entity.HasOne(d => d.IdRegionNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRegion)
                    .HasConstraintName("FK_Users_Region");

                entity.HasOne(d => d.IdSubEspAnMedCrNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdSubEspAnMedCr)
                    .HasConstraintName("FK_Users_SubEspAnMedCr");

                entity.HasOne(d => d.IdSubEspGobsNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdSubEspGobs)
                    .HasConstraintName("FK_Users_SubEspGObs");

                entity.HasOne(d => d.IdSubEspMedNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdSubEspMed)
                    .HasConstraintName("FK_Users_SubEspMed");

                entity.HasOne(d => d.IdSubEspOdoNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdSubEspOdo)
                    .HasConstraintName("FK_Users_SubEspOdo");

                entity.HasOne(d => d.IdSubEspPedNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdSubEspPed)
                    .HasConstraintName("FK_Users_SubEspPed");

                entity.HasOne(d => d.IdSubEspQxNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdSubEspQx)
                    .HasConstraintName("FK_Users_SubEspQx");

                entity.HasOne(d => d.IdSubEspRadNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdSubEspRad)
                    .HasConstraintName("FK_Users_SubEspRad");

                entity.HasOne(d => d.IdSubEspTecNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdSubEspTec)
                    .HasConstraintName("FK_Users_SubEspTec");

                entity.HasOne(d => d.IdTipoUserNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdTipoUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_TipoUser");
            });
        }
    }
}
