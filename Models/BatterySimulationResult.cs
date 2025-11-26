namespace battery_calculator.Models;

/// <summary>
/// Represents the result of a single day's battery simulation.
/// </summary>
public class BatterySimulationResult
{
    /// <summary>
    /// Date of the simulation.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Battery charge level at the start of the day in kWh.
    /// </summary>
    public double BatteryChargeStart { get; set; }

    /// <summary>
    /// Battery charge level at the end of the day in kWh.
    /// </summary>
    public double BatteryChargeEnd { get; set; }

    /// <summary>
    /// Energy discharged from battery during the day in kWh.
    /// </summary>
    public double EnergyDischarged { get; set; }

    /// <summary>
    /// Energy charged into battery during the day in kWh.
    /// </summary>
    public double EnergyCharged { get; set; }

    /// <summary>
    /// Original energy drawn from grid (before battery discharge) in kWh.
    /// </summary>
    public double OriginalGridDraw { get; set; }

    /// <summary>
    /// Energy drawn from grid after battery discharge in kWh.
    /// </summary>
    public double GridDrawAfterBattery { get; set; }

    /// <summary>
    /// Original energy fed to grid (before battery charging) in kWh.
    /// </summary>
    public double OriginalGridFeed { get; set; }

    /// <summary>
    /// Energy fed to grid after battery charging in kWh.
    /// </summary>
    public double GridFeedAfterBattery { get; set; }

    /// <summary>
    /// Energy savings from battery discharge (reduced grid purchase) in kWh.
    /// </summary>
    public double EnergySavedFromDischarge => OriginalGridDraw - GridDrawAfterBattery;

    /// <summary>
    /// Energy used for battery charging (reduced grid feed) in kWh.
    /// </summary>
    public double EnergyUsedForCharging => OriginalGridFeed - GridFeedAfterBattery;
}

