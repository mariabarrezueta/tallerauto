using Microsoft.EntityFrameworkCore;
using TallerAuto.Server.Models;
using System.Text.Json;

namespace TallerAuto.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    public DbSet<Cliente>       Clientes  => Set<Cliente>();
    public DbSet<Vehiculo>      Vehiculos { get; set; }
    public DbSet<Mecanico>      Mecanicos { get; set; }
    public DbSet<OrdenTrabajo>  Ordenes   { get; set; }
    public DbSet<HistorialServicio> HistorialServicios => Set<HistorialServicio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}



