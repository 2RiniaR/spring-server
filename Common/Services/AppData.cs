using Microsoft.EntityFrameworkCore;

namespace RineaR.Spring.Common;

public static class AppData
{
    private static DbSet<T> Entries<T>() where T : class
    {
        using var db = new SpringDbContext();
        return db.Set<T>();
    }

    public static DbSet<User> User => Entries<User>();
}