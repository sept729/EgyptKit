using System.ComponentModel.DataAnnotations;

namespace EgyptKit.Tests;

public class ValidationTests
{
    private sealed class Customer
    {
        [EgyptianMobile] public string? Phone { get; set; }
        [EgyptianNationalId] public string? NationalId { get; set; }
        [EgyptianGovernorate] public string? Governorate { get; set; }
    }

    private static List<ValidationResult> Validate(Customer c)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(c, new ValidationContext(c), results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void Valid_values_pass() =>
        Assert.Equal(0, Validate(new Customer { Phone = "+20 100 000 0000", NationalId = "30105151501234", Governorate = "الجيزة" }).Count);

    [Fact]
    public void Empty_values_are_left_to_Required() => Assert.Equal(0, Validate(new Customer()).Count);

    [Fact]
    public void Invalid_values_fail_with_a_message()
    {
        var results = Validate(new Customer { Phone = "12345", NationalId = "123", Governorate = "Paris" });

        Assert.Equal(3, results.Count);
        Assert.Contains("Egyptian mobile", results[0].ErrorMessage!);
    }
}
