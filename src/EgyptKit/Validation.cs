using System.ComponentModel.DataAnnotations;

namespace EgyptKit;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EgyptianMobileAttribute : ValidationAttribute
{
    public EgyptianMobileAttribute() : base("{0} must be an Egyptian mobile number, like 01012345678.") { }

    public override bool IsValid(object? value) =>
        value is null || value is string s && (s.Length == 0 || EgyptianMobile.IsValid(s));
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EgyptianNationalIdAttribute : ValidationAttribute
{
    public EgyptianNationalIdAttribute() : base("{0} must be a valid 14-digit Egyptian national ID.") { }

    public override bool IsValid(object? value) =>
        value is null || value is string s && (s.Length == 0 || EgyptianNationalId.IsValid(s));
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EgyptianGovernorateAttribute : ValidationAttribute
{
    public EgyptianGovernorateAttribute() : base("{0} isn't an Egyptian governorate.") { }

    public override bool IsValid(object? value) =>
        value is null || value is string s && (s.Length == 0 || Governorates.Find(s) is not null);
}
