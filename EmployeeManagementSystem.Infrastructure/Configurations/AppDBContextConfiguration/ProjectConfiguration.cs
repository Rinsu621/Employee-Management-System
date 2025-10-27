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
    public  class ProjectConfiguration:IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> entity)
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.ProjectName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(p => p.ProjectName)
                 .IsUnique();

            entity.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(p => p.ClientName)
                .HasMaxLength(200);

            entity.Property(p=>p.Status)    
                .HasMaxLength(50)
                .HasDefaultValue("Planned");

            entity.Property(p => p.Budget)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(p => p.Department)
                  .WithMany(d => d.Projects)
                  .HasForeignKey(p => p.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

          

           
        }

    }
   
}
