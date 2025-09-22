using EmployeeCRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Configurations
{
    public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
    {
        public void Configure(EntityTypeBuilder<Salary> builder)
        {
            // Primary key
            builder.HasKey(s => s.Id);

            // Properties
            builder.Property(s => s.EmployeeId)
                   .IsRequired();

            builder.Property(s => s.BasicSalary)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(s => s.Conveyance)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(s => s.Tax)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(s => s.PF)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(s => s.ESI)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(s => s.PaymentMode)
                .HasConversion<string>()   //converting enum to string while saving in db
                .HasMaxLength(50)
                .IsRequired();

            // Computed columns
            builder.Ignore(s => s.GrossSalary);
            builder.Ignore(s => s.NetSalary);

            // Timestamps
            builder.Property(s => s.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.Property(s => s.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
         }
    }
}
