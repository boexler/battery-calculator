using System.Globalization;
using battery_calculator.Models;

namespace battery_calculator.Services;

/// <summary>
/// Service for parsing CSV files containing energy data.
/// Supports flexible column detection based on header names.
/// </summary>
public class CsvParserService
{
    /// <summary>
    /// Reads the header line from a CSV file stream.
    /// </summary>
    /// <param name="csvStream">The CSV file stream to read from.</param>
    /// <returns>List of header column names.</returns>
    /// <exception cref="ArgumentException">Thrown when CSV format is invalid or header is missing.</exception>
    public async Task<List<string>> ReadHeadersAsync(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        
        var headerLine = await reader.ReadLineAsync();
        if (string.IsNullOrWhiteSpace(headerLine))
        {
            throw new ArgumentException("CSV file is empty or header line is missing.");
        }

        return ParseCsvLine(headerLine).ToList();
    }

    /// <summary>
    /// Creates a column mapping with automatic detection based on header names.
    /// </summary>
    /// <param name="headers">List of CSV header column names.</param>
    /// <returns>Column mapping with suggested indices.</returns>
    public ColumnMapping CreateAutoMapping(List<string> headers)
    {
        var mapping = new ColumnMapping();
        var headerArray = headers.ToArray();

        for (int i = 0; i < headerArray.Length; i++)
        {
            var header = headerArray[i].Trim().ToLowerInvariant();

            if (mapping.DateIndex == -1 && (header.Contains("datum") || header.Contains("date") || header.Contains("zeit") || header.Contains("time")))
            {
                mapping.DateIndex = i;
            }
            else if (mapping.TotalGenerationIndex == -1 && ((header.Contains("gesamt") && header.Contains("erzeugung")) || header.Contains("generation") || header.Contains("produktion")))
            {
                mapping.TotalGenerationIndex = i;
            }
            else if (mapping.TotalConsumptionIndex == -1 && ((header.Contains("gesamt") && header.Contains("verbrauch")) || header.Contains("consumption") || header.Contains("verbrauch")))
            {
                mapping.TotalConsumptionIndex = i;
            }
            else if (mapping.SelfConsumptionIndex == -1 && (header.Contains("eigenverbrauch") || (header.Contains("self") && header.Contains("consumption"))))
            {
                mapping.SelfConsumptionIndex = i;
            }
            else if (mapping.EnergyFedToGridIndex == -1 && (header.Contains("einspeis") || (header.Contains("fed") && header.Contains("grid")) || header.Contains("export")))
            {
                mapping.EnergyFedToGridIndex = i;
            }
            else if (mapping.EnergyDrawnFromGridIndex == -1 && (header.Contains("bezogen") || header.Contains("bezug") || (header.Contains("drawn") && header.Contains("grid")) || header.Contains("import")))
            {
                mapping.EnergyDrawnFromGridIndex = i;
            }
        }

        return mapping;
    }

    /// <summary>
    /// Parses a CSV file stream using the provided column mapping and returns a list of energy data records.
    /// </summary>
    /// <param name="csvStream">The CSV file stream to parse.</param>
    /// <param name="mapping">The column mapping to use.</param>
    /// <returns>List of parsed energy data records.</returns>
    /// <exception cref="ArgumentException">Thrown when CSV format is invalid or mapping is invalid.</exception>
    public async Task<List<EnergyDataRecord>> ParseCsvAsync(Stream csvStream, ColumnMapping mapping)
    {
        if (!mapping.IsValid())
        {
            throw new ArgumentException("Column mapping is invalid. All required columns must be mapped.");
        }

        var records = new List<EnergyDataRecord>();
        using var reader = new StreamReader(csvStream);
        
        // Read and skip header line
        var headerLine = await reader.ReadLineAsync();
        if (string.IsNullOrWhiteSpace(headerLine))
        {
            throw new ArgumentException("CSV file is empty or header line is missing.");
        }

        // Convert ColumnMapping to ColumnIndices for compatibility
        var columnIndices = new ColumnIndices
        {
            DateIndex = mapping.DateIndex,
            TotalGenerationIndex = mapping.TotalGenerationIndex,
            TotalConsumptionIndex = mapping.TotalConsumptionIndex,
            SelfConsumptionIndex = mapping.SelfConsumptionIndex,
            EnergyFedToGridIndex = mapping.EnergyFedToGridIndex,
            EnergyDrawnFromGridIndex = mapping.EnergyDrawnFromGridIndex
        };

        // Skip format line if present (e.g., [dd.MM.yyyy],[Wh],...)
        var secondLine = await reader.ReadLineAsync();
        if (secondLine != null && secondLine.StartsWith('['))
        {
            // This is a format line, skip it
        }
        else
        {
            // Second line is data, process it
            if (!string.IsNullOrWhiteSpace(secondLine))
            {
                var record = ParseDataLine(secondLine, columnIndices);
                if (record != null)
                {
                    records.Add(record);
                }
            }
        }

        // Read remaining data lines
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var record = ParseDataLine(line, columnIndices);
            if (record != null)
            {
                records.Add(record);
            }
        }

        if (records.Count == 0)
        {
            throw new ArgumentException("No valid data records found in CSV file.");
        }

        return records;
    }

    /// <summary>
    /// Helper class to store column indices (for internal use with ParseDataLine).
    /// </summary>
    private class ColumnIndices
    {
        public int DateIndex { get; set; } = -1;
        public int TotalGenerationIndex { get; set; } = -1;
        public int TotalConsumptionIndex { get; set; } = -1;
        public int SelfConsumptionIndex { get; set; } = -1;
        public int EnergyFedToGridIndex { get; set; } = -1;
        public int EnergyDrawnFromGridIndex { get; set; } = -1;
    }

    /// <summary>
    /// Parses a single CSV line into an array of values.
    /// </summary>
    private string[] ParseCsvLine(string line)
    {
        var values = new List<string>();
        var currentValue = new System.Text.StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                values.Add(currentValue.ToString().Trim());
                currentValue.Clear();
            }
            else
            {
                currentValue.Append(c);
            }
        }

        // Add last value
        values.Add(currentValue.ToString().Trim());

        return values.ToArray();
    }

    /// <summary>
    /// Parses a data line into an EnergyDataRecord.
    /// </summary>
    private EnergyDataRecord? ParseDataLine(string line, ColumnIndices indices)
    {
        var values = ParseCsvLine(line);

        if (values.Length <= Math.Max(
            Math.Max(indices.DateIndex, indices.TotalGenerationIndex),
            Math.Max(indices.TotalConsumptionIndex, Math.Max(indices.EnergyFedToGridIndex, indices.EnergyDrawnFromGridIndex))))
        {
            return null; // Not enough columns
        }

        try
        {
            // Parse date (format: dd.MM.yyyy)
            var dateStr = values[indices.DateIndex].Trim();
            if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                // Try other common formats
                if (!DateTime.TryParse(dateStr, CultureInfo.GetCultureInfo("de-DE"), DateTimeStyles.None, out date))
                {
                    return null; // Invalid date format
                }
            }

            // Parse numeric values (remove any formatting and convert from Wh)
            var totalGeneration = ParseDouble(values[indices.TotalGenerationIndex]);
            var totalConsumption = ParseDouble(values[indices.TotalConsumptionIndex]);
            var selfConsumption = indices.SelfConsumptionIndex >= 0 ? ParseDouble(values[indices.SelfConsumptionIndex]) : 0;
            var energyFedToGrid = ParseDouble(values[indices.EnergyFedToGridIndex]);
            var energyDrawnFromGrid = ParseDouble(values[indices.EnergyDrawnFromGridIndex]);

            return new EnergyDataRecord
            {
                Date = date,
                TotalGeneration = totalGeneration,
                TotalConsumption = totalConsumption,
                SelfConsumption = selfConsumption,
                EnergyFedToGrid = energyFedToGrid,
                EnergyDrawnFromGrid = energyDrawnFromGrid
            };
        }
        catch
        {
            return null; // Skip invalid lines
        }
    }

    /// <summary>
    /// Parses a double value from a string, handling various formats.
    /// </summary>
    private double ParseDouble(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;

        // Remove any whitespace and replace comma with dot if needed
        value = value.Trim().Replace(",", ".");

        if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        return 0;
    }
}

