using EmployeeManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Configurations.AppDBContextConfiguration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeManagementSystem.Domain.Entities.Employee>

    {
        public void Configure(EntityTypeBuilder<EmployeeManagementSystem.Domain.Entities.Employee> entity)

        {
            entity.HasKey(e => e.Id);


            entity.Property(e => e.EmpName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Employees)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.ManagedProjects)
                  .WithOne(p => p.ProjectManager)
                  .HasForeignKey(p => p.ProjectManagerId)
                  .OnDelete(DeleteBehavior.SetNull);


            entity.HasMany(e => e.Projects)
                 .WithMany(p => p.TeamMember)
                 .UsingEntity(j => j.ToTable("EmployeeProjects"));

            entity.HasOne(e => e.User)
            .WithOne(au => au.Employee)
            .HasForeignKey<ApplicationUser>(au => au.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
