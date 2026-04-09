using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PracFullStack.Models;

namespace PracFullStack.Contexts;

public partial class MoviesContext : DbContext
{
    public MoviesContext()
    {
    }

    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<pelicula> peliculas { get; set; }

    public virtual DbSet<pelicula_salacine> pelicula_salacines { get; set; }

    public virtual DbSet<sala_cine> sala_cines { get; set; }

    public virtual DbSet<usuario> usuarios { get; set; }

    public virtual DbSet<usuario1> usuarios1 { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<pelicula>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pelicula_pkey");

            entity.ToTable("pelicula");

            entity.Property(e => e.nombre).HasColumnType("character varying");
            entity.Property(e => e.fecha_publicacion)
          .HasColumnType("date");
        });

        modelBuilder.Entity<pelicula_salacine>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pelicula_salacine_pkey");

            entity.ToTable("pelicula_salacine");

            entity.Property(e => e.active).HasDefaultValue(true);

            entity.HasOne(d => d.id_peliculaNavigation).WithMany(p => p.pelicula_salacines)
                .HasForeignKey(d => d.id_pelicula)
                .HasConstraintName("pelicula_salacine_id_pelicula_fkey");

            entity.HasOne(d => d.id_salaNavigation).WithMany(p => p.pelicula_salacines)
                .HasForeignKey(d => d.id_sala)
                .HasConstraintName("pelicula_salacine_id_sala_fkey");
        });

        modelBuilder.Entity<sala_cine>(entity =>
        {
            entity.HasKey(e => e.id).HasName("sala_cine_pkey");

            entity.ToTable("sala_cine");

            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.estado).HasMaxLength(50);
            entity.Property(e => e.nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<usuario>(entity =>
        {
            entity.HasKey(e => e.id_usuario).HasName("usuario_pkey");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.username, "usuario_username_key").IsUnique();

            entity.Property(e => e.role).HasMaxLength(20);
            entity.Property(e => e.username).HasMaxLength(50);
        });

        modelBuilder.Entity<usuario1>(entity =>
        {
            entity.HasKey(e => e.id_usuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.username, "usuarios_username_key").IsUnique();

            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.password).HasMaxLength(255);
            entity.Property(e => e.rol).HasMaxLength(20);
            entity.Property(e => e.username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
