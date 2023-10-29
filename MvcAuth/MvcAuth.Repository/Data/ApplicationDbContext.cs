using Microsoft.EntityFrameworkCore;

namespace MvcAuth.Repository.Data;
public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(x => x.GetProperties()
                               .Where(x => x.ClrType == typeof(string))))
        {
            property.SetColumnType("varchar(90)");
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        foreach (var property in modelBuilder.Model.GetEntityTypes()
          .SelectMany(x => x.GetForeignKeys()))
        {

            property.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        base.OnModelCreating(modelBuilder);
    }

}
