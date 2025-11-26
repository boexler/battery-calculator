namespace battery_calculator.Models;

/// <summary>
/// Represents the mapping of CSV columns to data fields.
/// </summary>
public class ColumnMapping
{
    /// <summary>
    /// Index of the date column (-1 if not mapped).
    /// </summary>
    public int DateIndex { get; set; } = -1;

    /// <summary>
    /// Index of the total generation column (-1 if not mapped).
    /// </summary>
    public int TotalGenerationIndex { get; set; } = -1;

    /// <summary>
    /// Index of the total consumption column (-1 if not mapped).
    /// </summary>
    public int TotalConsumptionIndex { get; set; } = -1;

    /// <summary>
    /// Index of the self-consumption column (-1 if not mapped, optional).
    /// </summary>
    public int SelfConsumptionIndex { get; set; } = -1;

    /// <summary>
    /// Index of the energy fed to grid column (-1 if not mapped).
    /// </summary>
    public int EnergyFedToGridIndex { get; set; } = -1;

    /// <summary>
    /// Index of the energy drawn from grid column (-1 if not mapped).
    /// </summary>
    public int EnergyDrawnFromGridIndex { get; set; } = -1;

    /// <summary>
    /// Validates that all required columns are mapped.
    /// </summary>
    public bool IsValid()
    {
        return DateIndex >= 0 &&
               TotalGenerationIndex >= 0 &&
               TotalConsumptionIndex >= 0 &&
               EnergyFedToGridIndex >= 0 &&
               EnergyDrawnFromGridIndex >= 0;
    }
}

