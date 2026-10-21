namespace SalonAppointment.Identity.Application.Features.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("لطفا شماره موبایل را وارد نمایید.")
            .Matches(@"^09\d{9}$").WithMessage("شماره موبایل نامعتبر است.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("لطفا گذرواژه را وارد نمایید.");
    }
}