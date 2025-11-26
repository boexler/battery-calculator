using battery_calculator.Models;

namespace battery_calculator.Services;

/// <summary>
/// Service for simulating battery charge and discharge cycles based on energy data.
/// </summary>
public class BatterySimulationService
{
    /// <summary>
    /// Simulates battery behavior for a list of energy data records.
    /// </summary>
    /// <param name="energyData">List of daily energy data records.</param>
    /// <param name="batteryCapacityKwh">Battery capacity in kWh.</param>
    /// <param name="chargeLossPercent">Charge loss percentage (e.g., 5 for 5%).</param>
    /// <param name="dischargeLossPercent">Discharge loss percentage (e.g., 5 for 5%).</param>
    /// <returns>List of daily simulation results.</returns>
    public List<BatterySimulationResult> Simulate(
        List<EnergyDataRecord> energyData,
        double batteryCapacityKwh,
        double chargeLossPercent,
        double dischargeLossPercent)
    {
        var results = new List<BatterySimulationResult>();
        double currentBatteryCharge = 0; // Start with empty battery

        // Convert loss percentages to factors
        var chargeLossFactor = chargeLossPercent / 100.0;
        var dischargeLossFactor = dischargeLossPercent / 100.0;

        foreach (var day in energyData.OrderBy(d => d.Date))
        {
            var result = new BatterySimulationResult
            {
                Date = day.Date,
                BatteryChargeStart = currentBatteryCharge,
                OriginalGridDraw = day.EnergyDrawnFromGridKwh,
                OriginalGridFeed = day.EnergyFedToGridKwh
            };

            // Step 1: Discharge battery to reduce grid draw
            // We can discharge up to the available battery charge (accounting for losses)
            // The energy we can actually use from the battery is: batteryCharge * (1 - dischargeLoss)
            var availableEnergyFromBattery = currentBatteryCharge * (1 - dischargeLossFactor);
            var gridDrawNeeded = day.EnergyDrawnFromGridKwh;

            if (availableEnergyFromBattery > 0 && gridDrawNeeded > 0)
            {
                // Discharge as much as needed, but not more than available
                var energyToDischarge = Math.Min(availableEnergyFromBattery, gridDrawNeeded);
                
                // Calculate how much battery charge we need to discharge to get this energy
                // energyToDischarge = batteryDischarge * (1 - dischargeLoss)
                // batteryDischarge = energyToDischarge / (1 - dischargeLoss)
                var batteryDischarge = energyToDischarge / (1 - dischargeLossFactor);
                
                currentBatteryCharge -= batteryDischarge;
                result.EnergyDischarged = batteryDischarge;
                result.GridDrawAfterBattery = gridDrawNeeded - energyToDischarge;
            }
            else
            {
                result.EnergyDischarged = 0;
                result.GridDrawAfterBattery = gridDrawNeeded;
            }

            // Step 2: Charge battery from grid feed
            // We can charge the battery with energy from grid feed (accounting for losses)
            // The energy we can store in the battery is: gridFeed * (1 - chargeLoss)
            var gridFeedAvailable = day.EnergyFedToGridKwh;
            var batterySpaceAvailable = batteryCapacityKwh - currentBatteryCharge;

            if (gridFeedAvailable > 0 && batterySpaceAvailable > 0)
            {
                // Calculate how much energy we can store (after losses)
                var energyToStore = Math.Min(batterySpaceAvailable, gridFeedAvailable * (1 - chargeLossFactor));
                
                // Calculate how much grid feed energy we need to use to store this
                // energyToStore = gridFeedUsed * (1 - chargeLoss)
                // gridFeedUsed = energyToStore / (1 - chargeLoss)
                var gridFeedUsed = energyToStore / (1 - chargeLossFactor);
                
                currentBatteryCharge += energyToStore;
                result.EnergyCharged = energyToStore;
                result.GridFeedAfterBattery = gridFeedAvailable - gridFeedUsed;
            }
            else
            {
                result.EnergyCharged = 0;
                result.GridFeedAfterBattery = gridFeedAvailable;
            }

            // Ensure battery charge doesn't exceed capacity (safety check)
            currentBatteryCharge = Math.Min(currentBatteryCharge, batteryCapacityKwh);
            currentBatteryCharge = Math.Max(currentBatteryCharge, 0); // Ensure non-negative

            result.BatteryChargeEnd = currentBatteryCharge;
            results.Add(result);
        }

        return results;
    }
}

