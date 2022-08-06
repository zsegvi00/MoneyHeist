using Microsoft.EntityFrameworkCore;
using MoneyHeist.Entities;

namespace MoneyHeist.DbContexts
{
    public class MoneyHeistContext : DbContext
    {
        public DbSet<Member> Members { get; set; } = null!;

        public DbSet<Skill> Skills { get; set; } = null!;

        public DbSet<MemberSkill> MembersSkills { get; set; } =null!;

        public DbSet<Heist> Heists { get; set; } = null!;

        public DbSet<HeistSkill> HeistSkills { get; set; } = null!;

        




        public MoneyHeistContext(DbContextOptions<MoneyHeistContext> options) : base (options)
        {

        }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasMany(m => m.Skills)
                .WithMany(s => s.Members)
                .UsingEntity<MemberSkill>(
                j => j.HasOne(ms => ms.Skill).WithMany(s => s.MemberSkills),
                j => j.HasOne(ms => ms.Member).WithMany(m => m.MemberSkills)
                ).Property(ms => ms.Level)
                .HasDefaultValue("*");


            modelBuilder.Entity<Heist>()
                .HasMany(c => c.HeistSkills)
                .WithOne(e => e.Heist);



        }

    }
}
