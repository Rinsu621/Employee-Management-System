using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Validator
{
    public class EntityIdValidator<TEntity>:AbstractValidator<Guid> where TEntity : class
    {
        private readonly AppDbContext _dbContext;

        public EntityIdValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(id => id)
                .NotEmpty().WithMessage("ID is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    var exists = await _dbContext.Set<TEntity>().AnyAsync(e => EF.Property<Guid>(e, "Id") == id, cancellation);
                    if (!exists)
                    {
                        // If the entity does not exist, throw a KeyNotFoundException
                        throw new KeyNotFoundException($"{typeof(TEntity).Name} with Id '{id}' not found.");
                    }
                    return exists;
                })
                .WithMessage($"{typeof(TEntity).Name} with Id '{{PropertyValue}}' not found.");
        }

    }

}
