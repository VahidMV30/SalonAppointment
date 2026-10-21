namespace SalonAppointment.Identity.Domain.ValueObjects;

public sealed partial record MobileNumber
{
    private MobileNumber(string mobile)
    {
        Value = mobile;
    }

    public string Value { get; }

    public static MobileNumber Create(string mobile)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mobile);

        mobile = mobile.Trim();

        return !MobileRegex().IsMatch(mobile)
            ? throw new ArgumentException("Mobile number format is invalid.", nameof(mobile))
            : new MobileNumber(mobile);
    }

    [GeneratedRegex(@"^09\d{9}$")]
    private static partial Regex MobileRegex();
}