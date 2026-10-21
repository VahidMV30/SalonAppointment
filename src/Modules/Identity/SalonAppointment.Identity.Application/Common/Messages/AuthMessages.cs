namespace SalonAppointment.Identity.Application.Common.Messages;

public static class AuthMessages
{
    public const string AlreadyRegistered = "شما قبلا ثبت نام کرده اید. لطفا وارد شوید.";
    public const string InvalidCredentials = "شماره موبایل یا گذرواژه صحیح نیست.";
    public const string AccountDeactivated = "حساب کاربری شما غیر فعال شده است.";
    public const string InvalidRefreshToken = "نشست شما معتبر نیست یا منقضی شده است. لطفا دوباره وارد شوید.";
    public const string Unauthorized = "دسترسی غیرمجاز. لطفا وارد حساب کاربری خود شوید.";
}