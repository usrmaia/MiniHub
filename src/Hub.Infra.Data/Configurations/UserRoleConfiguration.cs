﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infra.Data.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string> { UserId = "1", RoleId = "1" },
            new IdentityUserRole<string> { UserId = "1", RoleId = "4" },

            new IdentityUserRole<string> { UserId = "2", RoleId = "2" },
            new IdentityUserRole<string> { UserId = "2", RoleId = "4" },

            new IdentityUserRole<string> { UserId = "3", RoleId = "3" },
            new IdentityUserRole<string> { UserId = "3", RoleId = "4" }
        );
    }
}
