using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public static class IdentitySeeds
    {
        public static void Seed(this ModelBuilder builder)
        {

            builder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "ADMIN MSS", NormalizedName = "ADMIN MSS" },
                new { Id = "2", Name = "ADMIN ORGANISME", NormalizedName = "ADMIN ORGANISME" },
                new { Id = "3", Name = "SUPERVISEUR", NormalizedName = "SUPERVISEUR" },
                new { Id = "4", Name = "USER BANK", NormalizedName = "USER BANK" },
                new { Id = "5", Name = "MERCHANT", NormalizedName = "MERCHANT" },
                new { Id = "6", Name = "MAGASIN", NormalizedName = "MAGASIN" }


               );



        }
    }
}
