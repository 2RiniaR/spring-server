using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RineaR.Spring.Common;

public class SpringDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySQL(EnvironmentManager.MysqlConnectionString);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var entities = from e in ChangeTracker.Entries()
            where e.State == EntityState.Added || e.State == EntityState.Modified
            select e.Entity;

        foreach (var entity in entities)
        {
            var validationContext = new ValidationContext(entity, null);
            Validator.ValidateObject(entity, validationContext, true);
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ActionBaseConfiguration());
        modelBuilder.ApplyConfiguration(new BedInConfiguration());
        modelBuilder.ApplyConfiguration(new DailyContributionConfiguration());
        modelBuilder.ApplyConfiguration(new LoginConfiguration());
        modelBuilder.ApplyConfiguration(new PraiseConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new WakeUpConfiguration());
    }
}