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
    public class DepartmentConfiguration:IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.DeptName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(d => d.DeptName)
                 .IsUnique();
        }
    }
 
}
