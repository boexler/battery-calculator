namespace battery_calculator.Models;

/// <summary>
/// Represents a single energy data record from CSV file.
/// All energy values are stored in Wh (Watt-hours) as provided in the CSV.
/// </summary>
public class EnergyDataRecord
{
    /// <summary>
    /// Date and time of the measurement.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Total energy generation in Wh.
    /// </summary>
    public double TotalGeneration { get; set; }

    /// <summary>
    /// Total energy consumption in Wh.
    /// </summary>
    public double TotalConsumption { get; set; }

    /// <summary>
    /// Self-consumption in Wh.
    /// </summary>
    public double SelfConsumption { get; set; }

    /// <summary>
    /// Energy fed into the grid in Wh.
    /// </summary>
    public double EnergyFedToGrid { get; set; }

    /// <summary>
    /// Energy drawn from the grid in Wh.
    /// </summary>
    public double EnergyDrawnFromGrid { get; set; }

    /// <summary>
    /// Converts Wh to kWh for display purposes.
    /// </summary>
    public double TotalGenerationKwh => TotalGeneration / 1000.0;

    /// <summary>
    /// Converts Wh to kWh for display purposes.
    /// </summary>
    public double TotalConsumptionKwh => TotalConsumption / 1000.0;

    /// <summary>
    /// Converts Wh to kWh for display purposes.
    /// </summary>
    public double EnergyFedToGridKwh => EnergyFedToGrid / 1000.0;

    /// <summary>
    /// Converts Wh to kWh for display purposes.
    /// </summary>
    public double EnergyDrawnFromGridKwh => EnergyDrawnFromGrid / 1000.0;
}

