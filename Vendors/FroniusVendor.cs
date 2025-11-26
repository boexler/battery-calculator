using battery_calculator.Interfaces;

namespace battery_calculator.Vendors;

/// <summary>
/// Fronius inverter vendor implementation.
/// </summary>
public class FroniusVendor : IVendor
{
    public string Name => "Fronius";
    public double ChargeLossPercent => 5.0;
    public double DischargeLossPercent => 5.0;
    public List<IBattery> Batteries { get; }

    public FroniusVendor()
    {
        Batteries = new List<IBattery>
        {
            new FroniusBattery("HVS 5.1", 5.1, "https://www.idealo.de/preisvergleich/OffersOfProduct/201960731.html"),
            new FroniusBattery("HVS 7.7", 7.7, "https://www.idealo.de/preisvergleich/OffersOfProduct/201960731.html"),
            new FroniusBattery("HVS 10.2", 10.2, "https://www.idealo.de/preisvergleich/OffersOfProduct/201960731.html"),
            new FroniusBattery("HVS 12.8", 12.8, "https://www.idealo.de/preisvergleich/OffersOfProduct/201960731.html")
        };
    }
}

