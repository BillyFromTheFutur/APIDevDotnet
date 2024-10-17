using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Models.EntityFramework
{
    public partial class SeriesDbContext : DbContext
    {
        public SeriesDbContext()
        {
        }

        public SeriesDbContext(DbContextOptions<SeriesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }
        public virtual DbSet<Serie> Series { get; set; }
        public virtual DbSet<Notation> Notations { get; set; }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseLoggerFactory(MyLoggerFactory)
        //                     .EnableSensitiveDataLogging()
        //                     .UseNpgsql("Server=localhost;port=5432;Database=SeriesDB;uid=postgres;password=postgres;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<Serie>(entity =>
            {
                entity.HasKey(e => e.SerieId).HasName("pk_serie");
                //entity.HasIndex(e => e.Titre).HasDatabaseName("ix_t_e_serie_ser_serie");


                //entity.Property(e => e.Titre)
                //      .HasMaxLength(100)
                //      .IsRequired(); 

                entity.HasMany(e => e.NotesSerie)
                      .WithOne(n => n.SerieNotee)
                      .HasForeignKey(n => n.SerieId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_notation_serie");
            });

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasKey(e => e.UtilisateurId).HasName("pk_utilisateur");
                entity.Property(e => e.CodePostal)
              .HasColumnType("char(5)");

                entity.Property(e => e.Pays)
                      .HasColumnType("character varying(50)")
                      .HasDefaultValue("France");

                entity.Property(e => e.DateCreation)
                      .HasColumnType("date")
                      .HasDefaultValueSql("now()");

                //entity.Property(e => e.Mail)
                //      .HasMaxLength(100)
                //      .IsRequired(); 

                //entity.HasIndex(e => e.Mail).IsUnique().HasDatabaseName("uq_utl_mail");


                entity.HasMany(e => e.NotesUtilisateur)
                      .WithOne(n => n.UtilisateurNotant)
                      .HasForeignKey(n => n.UtilisateurId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_notation_utilisateur");
            });

            modelBuilder.Entity<Notation>(entity =>
            {
                entity.HasKey(e => new { e.UtilisateurId, e.SerieId }).HasName("pk_notation");

                //entity.Property(e => e.Note)
                //      .IsRequired();
                //entity.HasIndex(e => e.SerieId).HasDatabaseName("IX_t_j_notation_not_ser_id");


            });
            modelBuilder.Entity<Serie>().HasIndex(e => e.Titre);
            modelBuilder.Entity<Utilisateur>().HasIndex(e => e.Mail).IsUnique();
            modelBuilder.Entity<Notation>().HasIndex(e => e.SerieId);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
