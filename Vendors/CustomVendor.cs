using battery_calculator.Interfaces;

namespace battery_calculator.Vendors;

/// <summary>
/// Custom vendor implementation for user-defined inverters.
/// </summary>
public class CustomVendor : IVendor
{
    public string Name => "Custom";
    public double ChargeLossPercent { get; set; } = 5.0;
    public double DischargeLossPercent { get; set; } = 5.0;
    public List<IBattery> Batteries { get; }

    public CustomVendor()
    {
        Batteries = new List<IBattery>();
    }
}

