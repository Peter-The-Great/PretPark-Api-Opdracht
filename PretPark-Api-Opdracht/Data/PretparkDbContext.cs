namespace PretPark_Api_Opdracht.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
public class PretparkContext : IdentityDbContext
{
    public PretparkContext (DbContextOptions<PretparkContext> options): base(options){}

    public DbSet<Attractie> Attractie { get; set; } = default!;

    public DbSet<GebruikerMetWachwoord> Gebruikers {get;set;} = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 
        builder.Entity<Attractie>()
        .HasMany(p => p.UserLikes)
        .WithMany(p => p.LikedAttractions)
        .UsingEntity<Dictionary<string, object>>(
        "AttractieGebruiker",
        j => j
        .HasOne<GebruikerMetWachwoord>()
        .WithMany()
        .HasForeignKey("GebruikerId")
        .HasConstraintName("FK_AttractieGebruiker_Gebruikers_GebruikerId")
        .OnDelete(DeleteBehavior.Cascade),
        j => j
        .HasOne<Attractie>()
        .WithMany()
        .HasForeignKey("AttractieId")
        .HasConstraintName("FK_AttractieGebruiker_Attracties_AttractieId")
        .OnDelete(DeleteBehavior.ClientCascade));
    }
}