using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Domain;
using System.Reflection.Emit;

namespace NZWalksAPI.Data
{
    public class NZWalksAuthDbContext:IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options):base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "60956af8-e950-4132-9562-9a2d0602673d";
            var writerRoleId = "9a4a7bdc-f7d2-48cb-acef-51e6a3e4da62";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
            {
                Id = readerRoleId,
                ConcurrencyStamp="readerRoleId",
                Name="Reader",
                NormalizedName="Reader".ToUpper()
            },
            new IdentityRole
            {
                Id = writerRoleId,
                ConcurrencyStamp = "writerRoleId",
                Name = "Writer",
                NormalizedName = "Writer".ToUpper()
            }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
