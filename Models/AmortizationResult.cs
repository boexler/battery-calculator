namespace battery_calculator.Models;

/// <summary>
/// Represents the complete amortization calculation result.
/// </summary>
public class AmortizationResult
{
    /// <summary>
    /// Battery purchase price in EUR.
    /// </summary>
    public double BatteryPrice { get; set; }

    /// <summary>
    /// Annual energy cost savings in EUR.
    /// </summary>
    public double AnnualSavings { get; set; }

    /// <summary>
    /// Amortization period in years.
    /// </summary>
    public double AmortizationPeriodYears { get; set; }

    /// <summary>
    /// Amortization period in months.
    /// </summary>
    public double AmortizationPeriodMonths => AmortizationPeriodYears * 12;

    /// <summary>
    /// Total savings over the simulation period in EUR.
    /// </summary>
    public double TotalSavings { get; set; }

    /// <summary>
    /// Total energy saved from battery discharge (reduced grid purchases) in kWh.
    /// </summary>
    public double TotalEnergySaved { get; set; }

    /// <summary>
    /// Total energy used for battery charging (reduced grid feed) in kWh.
    /// </summary>
    public double TotalEnergyUsedForCharging { get; set; }

    /// <summary>
    /// List of daily simulation results.
    /// </summary>
    public List<BatterySimulationResult> DailyResults { get; set; } = new();

    /// <summary>
    /// Number of days in the simulation period.
    /// </summary>
    public int SimulationDays => DailyResults.Count;

    /// <summary>
    /// Indicates if amortization is possible (positive annual savings).
    /// </summary>
    public bool IsAmortizationPossible => AnnualSavings > 0;
}

