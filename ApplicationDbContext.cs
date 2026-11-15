using Microsoft.EntityFrameworkCore;

namespace SistemaPacientes;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost\\SQLEXPRESS;" +
            "Database=RegistroPacientesDB;" +
            "Trusted_Connection=True;" +
            "MultipleActiveResultSets=true;" +
            "TrustServerCertificate=True;");
    }

    public DbSet<Paciente> Pacientes { get; set; }
}
