using DeskManager.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeskManager.Models.Validators;

public class CreateLocationValidation : AbstractValidator<CreateLocationDto>
{
    public CreateLocationValidation(DeskManagerDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).CustomAsync(async (value, context, cancellationToken) =>
        {
            if (await dbContext.Locations.AnyAsync(x => x.Name == value, cancellationToken))
            {
                context.AddFailure("Name","That location name is taken");
            }
        });

    }
}