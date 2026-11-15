using System.ComponentModel.DataAnnotations;

namespace SistemaPacientes;

public class Paciente
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Apellido { get; set; } = "";

    [Required]
    [StringLength(11)]
    public string Cedula { get; set; } = "";

    public int Edad { get; set; }

    [Required]
    [StringLength(15)]
    public string Telefono { get; set; } = "";

    [Required]
    [StringLength(100)]
    public string Direccion { get; set; } = "";

    [Required]
    [StringLength(200)]
    public string Diagnostico { get; set; } = "";

    public bool TieneSeguro { get; set; }
    public Paciente()
    {
    }
    public Paciente(string nombre, string apellido, string cedula, int edad,
                    string telefono, string direccion, string diagnostico, bool tieneSeguro)
    {
        Nombre = nombre;
        Apellido = apellido;
        Cedula = cedula;
        Edad = edad;
        Telefono = telefono;
        Direccion = direccion;
        Diagnostico = diagnostico;
        TieneSeguro = tieneSeguro;
    }

      public string NombreCompleto => $"{Nombre} {Apellido}";
}
