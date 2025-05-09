using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace TallerAuto.Server.Models;

public class Mecanico
{
    [Key]
    public int IDMecanico { get; set; }

    public string Nombre { get; set; } = null!;
    public string Especialidad { get; set; } = null!;
    public int AniosExperiencia { get; set; }

    // Estas columnas SÍ deben existir en la base
    public string HabilidadesJson { get; set; } = "[]";
    public string MarcasJson { get; set; } = "[]";

    // Estas propiedades son solo para trabajar con listas
    [NotMapped]
    public List<string> Habilidades
    {
        get => JsonSerializer.Deserialize<List<string>>(HabilidadesJson) ?? new();
        set => HabilidadesJson = JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public List<string> MarcasExpertas
    {
        get => JsonSerializer.Deserialize<List<string>>(MarcasJson) ?? new();
        set => MarcasJson = JsonSerializer.Serialize(value);
    }

    public int OrdenesActivas { get; set; }

    public int CalcularPuntaje(CategoriaReparacion cat)
    {
        int basePts = AniosExperiencia;
        if (Habilidades?.Any(h => h.Contains(cat.ToString(), StringComparison.OrdinalIgnoreCase)) == true)
            basePts += 5;
        return basePts - OrdenesActivas * 2;
    }
}

