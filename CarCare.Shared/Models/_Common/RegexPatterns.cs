namespace CarCare.Shared.Models._Common
{
    public static class RegexPatterns
    {

        public const string PhoneNumber = @"^(\+2)?(01[0-2,5]\d{8}|02\d{8}|03\d{7})$";
        public const string Email = @"^[a-zA-Z0-9._%+-]+@(gmail\.com|googlemail\.com|google\.com|[a-zA-Z0-9-]+\.edu\.eg)$";
        public const string Password = @"^(?=.*\d)[\w!@#$%^&*()\-+={}[\]:;""'<>,.?/\\|`~]{8,}$";
        public const string NationalId = @"^[23]\d{13}$";
	}
}
