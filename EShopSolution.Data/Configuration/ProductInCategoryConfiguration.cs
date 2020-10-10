﻿using EShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopSolution.Data.Configuration
{
    public class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.CategoryId });

            builder.ToTable("ProductInCategories");

            builder.HasOne(x => x.Product).WithMany(pc => pc.ProductInCategories)
                .HasForeignKey(pc => pc.ProductId);

            builder.HasOne(x => x.Category).WithMany(pc => pc.ProductInCategories)
                .HasForeignKey(pc => pc.CategoryId);
        }
    }
}
