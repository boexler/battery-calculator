using battery_calculator.Interfaces;
using System.Text.RegularExpressions;

namespace battery_calculator.Services;

/// <summary>
/// Service for fetching battery prices from external sources (e.g., idealo.de).
/// </summary>
public class BatteryPriceService
{
    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, double?> _priceCache = new();

    public BatteryPriceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Set user agent to avoid blocking
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
    }

    /// <summary>
    /// Fetches the price for a battery from its PriceUrl.
    /// Uses caching to avoid multiple requests for the same URL.
    /// </summary>
    /// <param name="battery">The battery to fetch the price for.</param>
    /// <returns>The price in EUR, or null if fetching failed.</returns>
    public async Task<double?> FetchPriceAsync(IBattery battery)
    {
        if (string.IsNullOrEmpty(battery.PriceUrl))
            return battery.Price;

        // Check cache first
        if (_priceCache.TryGetValue(battery.PriceUrl, out var cachedPrice))
        {
            return cachedPrice;
        }

        // If battery already has a price, use it and cache it
        if (battery.Price.HasValue)
        {
            _priceCache[battery.PriceUrl] = battery.Price.Value;
            return battery.Price.Value;
        }

        try
        {
            // Note: Web scraping from Blazor WASM is limited due to CORS
            // This is a placeholder implementation that can be extended
            // For production, consider using a backend proxy or API
            
            // For now, return null and let the UI handle it
            // In a real implementation, you would:
            // 1. Fetch the HTML from the URL
            // 2. Parse the HTML to extract the price
            // 3. Cache the result
            
            // Example parsing (commented out due to CORS limitations):
            /*
            var response = await _httpClient.GetStringAsync(battery.PriceUrl);
            var price = ExtractPriceFromHtml(response);
            _priceCache[battery.PriceUrl] = price;
            return price;
            */
            
            return null;
        }
        catch (Exception)
        {
            // Log error in production
            return null;
        }
    }

    /// <summary>
    /// Extracts price from HTML content (placeholder for future implementation).
    /// </summary>
    private double? ExtractPriceFromHtml(string html)
    {
        // This would parse the HTML to find the price
        // Example regex pattern (would need to be adjusted for actual idealo.de structure):
        // var match = Regex.Match(html, @"(\d+[.,]\d+)\s*€");
        // if (match.Success)
        // {
        //     var priceStr = match.Groups[1].Value.Replace(",", ".");
        //     if (double.TryParse(priceStr, out var price))
        //         return price;
        // }
        return null;
    }

    /// <summary>
    /// Clears the price cache.
    /// </summary>
    public void ClearCache()
    {
        _priceCache.Clear();
    }
}

