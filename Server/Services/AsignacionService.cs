using Microsoft.EntityFrameworkCore;
using TallerAuto.Server.Data;
using TallerAuto.Server.Models;

namespace TallerAuto.Server.Services;

public class AsignacionService
{
    private readonly AppDbContext _db;

    public AsignacionService(AppDbContext db) => _db = db;

    // Diccionario de palabras clave por categoría
    private static readonly Dictionary<CategoriaReparacion, string[]> _keywordsPorCategoria = new()
    {
        [CategoriaReparacion.MecanicaGeneral] = new[] { "motor", "frenos", "suspensión", "dirección", "aceite" },
        [CategoriaReparacion.ElectricidadElectronica] = new[] { "batería", "alternador", "luces", "sensores", "escáner", "airbag" },
        [CategoriaReparacion.EsteticaCarroceria] = new[] { "pintura", "latonería", "vidrios", "accesorios" }
    };

    public async Task<Mecanico?> AsignarMecanicoAsync(OrdenTrabajo orden)
    {
        var vehiculo = await _db.Vehiculos.FindAsync(orden.VehiculoID);
        if (vehiculo == null) throw new Exception("Vehículo no encontrado");

        var categoria = orden.TipoReparacion;
        var marcaVehiculo = vehiculo.Marca;
        var palabrasClave = _keywordsPorCategoria[categoria];

        var todos = await _db.Mecanicos.ToListAsync();

        // Filtro 1: habilidades que coincidan con la categoría
        var candidatos = todos
            .Where(m =>
                m.Habilidades.Any(h =>
                    palabrasClave.Any(p => h.Contains(p, StringComparison.OrdinalIgnoreCase))))
            .ToList();

        foreach (var m in candidatos)
        {
            int puntaje = m.AniosExperiencia;

            // Filtro 2: sumar puntos por match de marca
            if (m.MarcasExpertas.Any(marca => marca.Equals(marcaVehiculo, StringComparison.OrdinalIgnoreCase)))
                puntaje += 3;

            // Filtro 3: restar puntos por cada orden activa
            puntaje -= m.OrdenesActivas * 2;

            m.OrdenesActivas = puntaje; // lo usamos temporalmente como puntaje
        }

        var mejor = candidatos
            .OrderByDescending(m => m.OrdenesActivas)
            .FirstOrDefault();

        if (mejor != null)
        {
            mejor.OrdenesActivas++; // volver a contar como carga real
            orden.MecanicoAsignadoID = mejor.IDMecanico;
            orden.Estado = EstadoOrden.EnProceso;

            await _db.SaveChangesAsync();
        }

        return mejor;
    }
}
