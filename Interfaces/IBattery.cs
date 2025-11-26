namespace battery_calculator.Interfaces;

/// <summary>
/// Interface for battery models.
/// </summary>
public interface IBattery
{
    /// <summary>
    /// Name of the battery model.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Battery capacity in Wh (Watt-hours).
    /// </summary>
    double CapacityWh { get; }

    /// <summary>
    /// Battery price in EUR. Can be null if price is fetched dynamically.
    /// </summary>
    double? Price { get; set; }

    /// <summary>
    /// HTTPS URL for fetching the battery price dynamically (e.g., idealo.de).
    /// </summary>
    string? PriceUrl { get; }

    /// <summary>
    /// Indicates if this is a custom/user-created battery.
    /// </summary>
    bool IsCustom { get; }
}

