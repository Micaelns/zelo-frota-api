using Domain.Enum;

namespace Domain.ObjectValues;

public class DriverLicense
{
    public string? NumberLicense { get; init; }
    public LicenseCategory LicenseCategory { get; init; }
    public DateTime ExpirationDate { get; set; }

    protected DriverLicense() { }

    public DriverLicense(string? numberLicense, LicenseCategory licenseCategory, DateTime expirationDate)
    {
        NumberLicense = numberLicense;
        LicenseCategory = licenseCategory;
        ExpirationDate = expirationDate;
    }

    public bool IsExpired() => ExpirationDate < DateTime.UtcNow;
    
    public bool CanDrive(LicenseCategory requiredLicenseCategory)
    {
        if (IsExpired()) return false;

        if (requiredLicenseCategory == LicenseCategory.A)
        {
            return LicenseCategory.HasFlag(LicenseCategory.A);
        }

        int maxRank = GetRank(this.LicenseCategory);
        int requiredRank = GetRank(requiredLicenseCategory);

        return maxRank >= requiredRank;
    }

    private static int GetRank(LicenseCategory category)
    {
        return category switch
        {
            _ when category.HasFlag(LicenseCategory.E) => 4,
            _ when category.HasFlag(LicenseCategory.D) => 3,
            _ when category.HasFlag(LicenseCategory.C) => 2,
            _ when category.HasFlag(LicenseCategory.B) => 1,
            _ => 0
        };
    }
}
