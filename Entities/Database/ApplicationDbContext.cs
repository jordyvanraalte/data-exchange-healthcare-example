using Microsoft.EntityFrameworkCore;

namespace healthcare.Entities.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<PatientData> PatientDatas { get; set; } = null!;

    public string DbPath { get; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        DbPath = System.IO.Path.Join("./", "healthcare.db");        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}