using Microsoft.EntityFrameworkCore;

namespace Bean_API.Models;

public partial class AllthebeansContext : DbContext
{
    public AllthebeansContext()
    {
    }

    public AllthebeansContext(DbContextOptions<AllthebeansContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coffeebean> Coffeebeans { get; set; }

    public virtual DbSet<Coffeebeanoftheday> Coffeebeanofthedays { get; set; }

    public virtual DbSet<Colour> Colours { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseMySql(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection"), ServerVersion.Parse("9.2.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Coffeebean>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("coffeebeans");

            entity.HasIndex(e => e.ColourId, "ColourId");

            entity.HasIndex(e => e.CountryId, "CountryId");

            entity.Property(e => e.Id)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.Cost).HasPrecision(10, 2);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.IsBotd)
                .HasColumnType("bit(1)")
                .HasColumnName("IsBOTD");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Colour).WithMany(p => p.Coffeebeans)
                .HasForeignKey(d => d.ColourId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("coffeebeans_ibfk_1");

            entity.HasOne(d => d.Country).WithMany(p => p.Coffeebeans)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("coffeebeans_ibfk_2");
        });

        modelBuilder.Entity<Coffeebeanoftheday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("coffeebeanoftheday");

            entity.HasIndex(e => e.CoffeeBeanId, "CoffeeBeanID");

            entity.Property(e => e.CoffeeBeanId)
                .HasMaxLength(24)
                .IsFixedLength()
                .HasColumnName("CoffeeBeanID");

            entity.HasOne(d => d.CoffeeBean).WithMany(p => p.Coffeebeanofthedays)
                .HasForeignKey(d => d.CoffeeBeanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("coffeebeanoftheday_ibfk_1");
        });

        modelBuilder.Entity<Colour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("colours");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("countries");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
