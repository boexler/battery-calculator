namespace battery_calculator.Vendors;

/// <summary>
/// Represents a custom/user-created battery.
/// </summary>
public class CustomBattery : Interfaces.IBattery
{
    public string Name { get; set; }
    public double CapacityWh { get; set; }
    public double? Price { get; set; }
    public string? PriceUrl { get; set; }
    public bool IsCustom => true;

    public CustomBattery(string name, double capacityKwh, double? price = null, string? priceUrl = null)
    {
        Name = name;
        CapacityWh = capacityKwh * 1000; // Convert kWh to Wh
        Price = price;
        PriceUrl = priceUrl;
    }
}

