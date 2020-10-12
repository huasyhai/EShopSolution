using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace EShopSolution.Data.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Entities.Transaction>
    {

        public void Configure(EntityTypeBuilder<Entities.Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.AppUser).WithMany(x => x.Transactions).HasForeignKey(x => x.UserId);
        }
    }
}
