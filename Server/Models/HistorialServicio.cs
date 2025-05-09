using TallerAuto.Server.Validation;
using System.ComponentModel.DataAnnotations;
namespace TallerAuto.Server.Models;

public class HistorialServicio
{
    [Key]
    public int ID { get; set; }

    public int OrdenID { get; set; }
    public OrdenTrabajo Orden { get; set; } = null!;

    public string Observacion { get; set; } = null!;
    public DateTime Fecha     { get; set; } = DateTime.UtcNow;
}
