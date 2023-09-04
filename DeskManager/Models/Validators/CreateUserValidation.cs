using DeskManager.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeskManager.Models.Validators;

public class CreateUserValidation : AbstractValidator<CreateUserDto>
{
    public  CreateUserValidation(DeskManagerDbContext dbContext)
    {
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.ConfirmEmail).Equal(x => x.Email);
        RuleFor(x => x.Password).MinimumLength(6);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
        RuleFor(x => x.Email).CustomAsync( async (value, context, cancellationToken) =>
        {
            if ( await dbContext.Users.AnyAsync(x => x.Email == value, cancellationToken))
            {
                context.AddFailure("Email", "That email is  taken");
            }

        });
    }
}