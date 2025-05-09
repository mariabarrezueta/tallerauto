using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerAuto.Server.Data;
using TallerAuto.Server.Models;
using TallerAuto.Server.Services;
using TallerAuto.Server.DTOs;

namespace TallerAuto.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly AsignacionService _svc;

    public OrdenesController(AppDbContext db, AsignacionService svc)
    {
        _db = db;
        _svc = svc;
    }

    [HttpPost]
public async Task<IActionResult> CrearOrden([FromBody] CrearOrdenDTO dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var orden = new OrdenTrabajo
    {
        VehiculoID = dto.VehiculoID,
        TipoReparacion = (CategoriaReparacion)dto.TipoReparacion,
        FechaIngreso = DateTime.UtcNow,
        Estado = EstadoOrden.Pendiente
    };

    _db.Ordenes.Add(orden);
    await _db.SaveChangesAsync();

    // Asigna el mecánico más adecuado y guarda los cambios en la orden
    var mecanicoAsignado = await _svc.AsignarMecanicoAsync(orden);

    // Solo guardar si se asignó un mecánico
    if (mecanicoAsignado != null)
    {
        _db.Ordenes.Update(orden);
        await _db.SaveChangesAsync();
    }

    return CreatedAtAction(nameof(GetById), new { id = orden.IDOrden }, orden);
}

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var orden = await _db.Ordenes
            .Include(o => o.MecanicoAsignado)
            .Include(o => o.Vehiculo)
            .FirstOrDefaultAsync(o => o.IDOrden == id);

        return orden is null ? NotFound() : Ok(orden);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrdenTrabajo>>> GetOrdenes()
    {
        var ordenes = await _db.Ordenes
            .Include(o => o.Vehiculo)
            .Include(o => o.MecanicoAsignado)
            .ToListAsync();

        return Ok(ordenes);
    }

    // Controllers/OrdenesController.cs - NUEVO MÉTODO
[HttpPut("{id}/estado")]
public async Task<IActionResult> ActualizarEstado(int id, [FromBody] EstadoUpdateDTO dto)
{
    var orden = await _db.Ordenes.Include(o => o.MecanicoAsignado).FirstOrDefaultAsync(o => o.IDOrden == id);
    if (orden == null) return NotFound();

    // Si cambia de EnProceso a otro estado, reducir carga
    if (orden.Estado == EstadoOrden.EnProceso && dto.Estado != EstadoOrden.EnProceso && orden.MecanicoAsignado != null)
    {
        orden.MecanicoAsignado.OrdenesActivas--;
    }

    // Si cambia de otro a EnProceso, aumentar carga
    if (orden.Estado != EstadoOrden.EnProceso && dto.Estado == EstadoOrden.EnProceso && orden.MecanicoAsignado != null)
    {
        orden.MecanicoAsignado.OrdenesActivas++;
    }

    orden.Estado = dto.Estado;
    await _db.SaveChangesAsync();
    return Ok();
}

}

