using battery_calculator.Models;

namespace battery_calculator.Services;

/// <summary>
/// Service for calculating battery amortization based on energy cost savings.
/// </summary>
public class AmortizationCalculatorService
{
    /// <summary>
    /// Calculates the amortization result based on simulation results and energy prices.
    /// </summary>
    /// <param name="simulationResults">List of daily battery simulation results.</param>
    /// <param name="originalEnergyData">Original energy data (without battery).</param>
    /// <param name="pricePerKwhPurchase">Price per kWh for purchasing energy from grid in EUR.</param>
    /// <param name="pricePerKwhFeed">Price per kWh for feeding energy to grid in EUR.</param>
    /// <param name="batteryPrice">Battery purchase price in EUR.</param>
    /// <returns>Complete amortization calculation result.</returns>
    public AmortizationResult Calculate(
        List<BatterySimulationResult> simulationResults,
        List<EnergyDataRecord> originalEnergyData,
        double pricePerKwhPurchase,
        double pricePerKwhFeed,
        double batteryPrice)
    {
        var result = new AmortizationResult
        {
            BatteryPrice = batteryPrice,
            DailyResults = simulationResults
        };

        // Create a dictionary for quick lookup of original energy data by date
        var originalDataByDate = originalEnergyData.ToDictionary(d => d.Date.Date, d => d);

        double totalCostWithoutBattery = 0;
        double totalCostWithBattery = 0;
        double totalEnergySaved = 0;
        double totalEnergyUsedForCharging = 0;

        foreach (var simResult in simulationResults)
        {
            // Get original data for this day
            if (!originalDataByDate.TryGetValue(simResult.Date.Date, out var originalData))
            {
                continue; // Skip if no original data found
            }

            // Calculate cost without battery
            var costWithoutBattery = (originalData.EnergyDrawnFromGridKwh * pricePerKwhPurchase)
                                   - (originalData.EnergyFedToGridKwh * pricePerKwhFeed);
            totalCostWithoutBattery += costWithoutBattery;

            // Calculate cost with battery
            var costWithBattery = (simResult.GridDrawAfterBattery * pricePerKwhPurchase)
                                - (simResult.GridFeedAfterBattery * pricePerKwhFeed);
            totalCostWithBattery += costWithBattery;

            // Track energy metrics
            totalEnergySaved += simResult.EnergySavedFromDischarge;
            totalEnergyUsedForCharging += simResult.EnergyUsedForCharging;
        }

        // Calculate total savings
        result.TotalSavings = totalCostWithoutBattery - totalCostWithBattery;
        result.TotalEnergySaved = totalEnergySaved;
        result.TotalEnergyUsedForCharging = totalEnergyUsedForCharging;

        // Calculate annual savings
        if (simulationResults.Count > 0)
        {
            var daysInSimulation = simulationResults.Count;
            var dailySavings = result.TotalSavings / daysInSimulation;
            result.AnnualSavings = dailySavings * 365.0;
        }
        else
        {
            result.AnnualSavings = 0;
        }

        // Calculate amortization period
        if (result.AnnualSavings > 0)
        {
            result.AmortizationPeriodYears = batteryPrice / result.AnnualSavings;
        }
        else
        {
            result.AmortizationPeriodYears = double.PositiveInfinity; // Never amortizes
        }

        return result;
    }
}

