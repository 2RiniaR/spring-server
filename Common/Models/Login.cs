using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class Login : ActionBase
{
    /// <summary>
    /// どの日分のログインか
    /// </summary>
    public DateTime ApplicationDate { get; set; }
}

public class LoginConfiguration : IEntityTypeConfiguration<Login>
{
    public void Configure(EntityTypeBuilder<Login> builder)
    {
    }
}