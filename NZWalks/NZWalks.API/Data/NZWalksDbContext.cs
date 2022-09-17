using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext

    {
        internal readonly object Roles;

        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.Role)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.RoleId);
            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.User)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.UserId);

        }

        internal Task User_Roles(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public DbSet<Region> Regions{ get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<User_Role> User_Roles { get; set; }
       

    }

}
