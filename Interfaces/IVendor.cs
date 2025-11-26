namespace battery_calculator.Interfaces;

/// <summary>
/// Interface for inverter vendor/manufacturer.
/// </summary>
public interface IVendor
{
    /// <summary>
    /// Name of the vendor/manufacturer.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Charge loss percentage (e.g., 5 for 5%).
    /// </summary>
    double ChargeLossPercent { get; }

    /// <summary>
    /// Discharge loss percentage (e.g., 5 for 5%).
    /// </summary>
    double DischargeLossPercent { get; }

    /// <summary>
    /// List of available batteries for this vendor.
    /// </summary>
    List<IBattery> Batteries { get; }
}

