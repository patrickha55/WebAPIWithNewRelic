using System.Text.Json.Serialization;

namespace WebAPIWithNewRelic.Models;

public record Weather([property: JsonPropertyName("location")] Location Location,
                      [property: JsonPropertyName("current")] Current Current);

public record Location([property: JsonPropertyName("name")] string Name,
                       [property: JsonPropertyName("country")] string Country,
                       [property: JsonPropertyName("tz_id")] string TimeZoneID,
                       [property: JsonPropertyName("localtime")] string LocalTime);

public record Current([property: JsonPropertyName("last_updated")] string LastUpdated,
                      [property: JsonPropertyName("temp_c")] Decimal Temperature,
                      [property: JsonPropertyName("condition")] Condition Condition,
                      [property: JsonPropertyName("wind_kph")] Decimal WindKPH,
                      [property: JsonPropertyName("wind_dir")] string WindDirection,
                      [property: JsonPropertyName("uv")] Decimal UVIndex,
                      [property: JsonPropertyName("air_quality")] AirQuality AirQuality);

public record Condition([property: JsonPropertyName("text")] string Text,
                        [property: JsonPropertyName("icon")] string Icon,
                        [property: JsonPropertyName("code")]  int Code);

public record AirQuality([property: JsonPropertyName("pm2_5")] Decimal PM2Dot5,
                         [property: JsonPropertyName("pm10")] Decimal PM10);
