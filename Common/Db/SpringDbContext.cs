using Microsoft.EntityFrameworkCore;

namespace RineaR.Spring.Common;

public class SpringDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySQL(EnvironmentManager.MysqlConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventRecordConfiguration());
        modelBuilder.ApplyConfiguration(new ActionBaseConfiguration());
        modelBuilder.ApplyConfiguration(new BedInConfiguration());
        modelBuilder.ApplyConfiguration(new DailyContributionConfiguration());
        modelBuilder.ApplyConfiguration(new LoginConfiguration());
        modelBuilder.ApplyConfiguration(new PraiseConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new WakeUpConfiguration());
    }
}