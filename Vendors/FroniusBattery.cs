namespace battery_calculator.Vendors;

/// <summary>
/// Represents a Fronius battery model.
/// </summary>
public class FroniusBattery : Interfaces.IBattery
{
    public string Name { get; }
    public double CapacityWh { get; }
    public double? Price { get; set; }
    public string? PriceUrl { get; }
    public bool IsCustom => false;

    public FroniusBattery(string name, double capacityKwh, string? priceUrl = null)
    {
        Name = name;
        CapacityWh = capacityKwh * 1000; // Convert kWh to Wh
        PriceUrl = priceUrl;
        Price = null; // Will be fetched dynamically
    }
}

