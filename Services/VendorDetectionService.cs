using battery_calculator.Interfaces;
using battery_calculator.Vendors;

namespace battery_calculator.Services;

/// <summary>
/// Service for detecting vendor/manufacturer from CSV headers.
/// </summary>
public class VendorDetectionService
{
    /// <summary>
    /// Detects the vendor from CSV headers and optionally filename.
    /// </summary>
    /// <param name="headers">List of CSV header column names.</param>
    /// <param name="filename">Optional filename for additional detection.</param>
    /// <returns>Detected vendor or CustomVendor as fallback.</returns>
    public IVendor DetectVendor(List<string> headers, string? filename = null)
    {
        // Check filename for vendor keywords
        if (!string.IsNullOrEmpty(filename))
        {
            var filenameLower = filename.ToLowerInvariant();
            if (filenameLower.Contains("fronius"))
            {
                return new FroniusVendor();
            }
        }

        // Check headers for Fronius pattern
        // Fronius format: "Datum und Uhrzeit,Gesamt Erzeugung,Gesamt Verbrauch,Eigenverbrauch,Energie ins Netz eingespeist,Energie vom Netz bezogen"
        if (IsFroniusFormat(headers))
        {
            return new FroniusVendor();
        }

        // Fallback to Custom
        return new CustomVendor();
    }

    /// <summary>
    /// Checks if the headers match the Fronius format.
    /// </summary>
    private bool IsFroniusFormat(List<string> headers)
    {
        if (headers.Count < 6)
            return false;

        var headerString = string.Join(",", headers);
        var headerLower = headerString.ToLowerInvariant();

        // Check for key Fronius header elements
        var hasDate = headerLower.Contains("datum") || headerLower.Contains("uhrzeit");
        var hasTotalGeneration = headerLower.Contains("gesamt") && headerLower.Contains("erzeugung");
        var hasTotalConsumption = headerLower.Contains("gesamt") && headerLower.Contains("verbrauch");
        var hasSelfConsumption = headerLower.Contains("eigenverbrauch");
        var hasEnergyFedToGrid = headerLower.Contains("einspeis") || headerLower.Contains("einspeist");
        var hasEnergyDrawnFromGrid = headerLower.Contains("bezogen") || headerLower.Contains("bezug");

        // Exact match check for Fronius format
        var expectedHeaders = new[]
        {
            "datum und uhrzeit",
            "gesamt erzeugung",
            "gesamt verbrauch",
            "eigenverbrauch",
            "energie ins netz eingespeist",
            "energie vom netz bezogen"
        };

        if (headers.Count == expectedHeaders.Length)
        {
            bool exactMatch = true;
            for (int i = 0; i < headers.Count; i++)
            {
                if (!headers[i].Trim().Equals(expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
                {
                    exactMatch = false;
                    break;
                }
            }
            if (exactMatch)
                return true;
        }

        // Pattern-based match
        return hasDate && hasTotalGeneration && hasTotalConsumption && 
               hasSelfConsumption && hasEnergyFedToGrid && hasEnergyDrawnFromGrid;
    }

    /// <summary>
    /// Gets all available vendors.
    /// </summary>
    public List<IVendor> GetAllVendors()
    {
        return new List<IVendor>
        {
            new FroniusVendor(),
            new CustomVendor()
        };
    }
}

