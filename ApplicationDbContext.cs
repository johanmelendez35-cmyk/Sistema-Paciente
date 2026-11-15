using Microsoft.EntityFrameworkCore;

namespace PatientSystem;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost\\SQLEXPRESS;" +
            "Database=ClinicaCalderonDB;" +
            "Trusted_Connection=True;" +
            "MultipleActiveResultSets=true;" +
            "TrustServerCertificate=True;");
    }

    public DbSet<Patient> Patients { get; set; }
}
