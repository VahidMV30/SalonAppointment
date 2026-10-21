namespace SalonAppointment.Identity.Application.Features.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("لطفا نام و نام خانوادگی را وارد نمایید.")
            .Length(3, 50).WithMessage("نام و نام خانوادگی باید حداقل 3 و حداکثر 50 کاراکتر باشد.");

        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("لطفا شماره موبایل را وارد نمایید.")
            .Matches(@"^09\d{9}$").WithMessage("شماره موبایل نامعتبر است.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا گذرواژه را وارد نمایید.")
            .MinimumLength(8).WithMessage("گذرواژه باید حداقل ۸ کاراکتر باشد.")
            .MaximumLength(64).WithMessage("گذرواژه نباید بیشتر از ۶۴ کاراکتر باشد.")
            .Matches("[A-Z]").WithMessage("گذرواژه باید حداقل یک حرف بزرگ انگلیسی داشته باشد.")
            .Matches("[a-z]").WithMessage("گذرواژه باید حداقل یک حرف کوچک انگلیسی داشته باشد.")
            .Matches("[0-9]").WithMessage("گذرواژه باید حداقل یک عدد داشته باشد.")
            .Matches("[^a-zA-Z0-9]").WithMessage("گذرواژه باید حداقل یک نماد (مثل !@#$) داشته باشد.");
    }
}