using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MijnCopilot.Model;

namespace MijnCopilot.DataAccess;

public class MijnCopilotDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }

    public MijnCopilotDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetValue<string>("CONNECTION_STRING"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entityBuilder =>
        {
            entityBuilder.ToTable("CHATS");
            entityBuilder.HasKey(x => x.Id).IsClustered(false);
            entityBuilder.Property(x => x.SysId).ValueGeneratedOnAdd();
            entityBuilder.HasIndex(x => x.SysId).IsClustered();
            entityBuilder.HasIndex(x => x.StartedOn);
        });

        modelBuilder.Entity<Message>(entityBuilder =>
        {
            entityBuilder.ToTable("MESSAGES");
            entityBuilder.HasKey(x => x.Id).IsClustered(false);
            entityBuilder.Property(x => x.SysId).ValueGeneratedOnAdd();
            entityBuilder.HasIndex(x => x.SysId).IsClustered();
            entityBuilder.HasIndex(x => x.PostedOn);
        });
    }
}