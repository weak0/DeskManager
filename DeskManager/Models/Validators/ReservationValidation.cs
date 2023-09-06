using FluentValidation;

namespace DeskManager.Models.Validators;

public class ReservationValidation : AbstractValidator<CreateReservationDto>
{
    public ReservationValidation()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .Must(startDate => startDate >= DateTime.Now)
            .WithMessage("Start date have to be in future");
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .Must((model, enDate) => IsWithinOneWeek(model.StartDate, enDate))
            .Must((model, enDate) => EndIsLaterThanStart(model.StartDate, enDate))
            .WithMessage("The reservation must be one week or less");
    }

    private bool IsWithinOneWeek(DateTime startDate, DateTime endDate)
    {
        var reservationTime = endDate - startDate;
        return reservationTime.TotalDays <= 7;
    }

    private bool EndIsLaterThanStart(DateTime startDate, DateTime endDate)
    {
        var reservationTime = endDate - startDate;
        return reservationTime.TotalDays > 0;
    }
}