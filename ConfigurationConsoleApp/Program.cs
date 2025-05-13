using ConfigurationLibrary;

var configReader = new ConfigurationReader(
    "SERVICE-A",
    "mongodb://localhost:27017",
    5000
);

try
{
    var siteName = configReader.GetValue<string>("SiteName");
    var isBasketEnabled = configReader.GetValue<bool>("IsBasketEnabled");
    var maxItemCount = configReader.GetValue<int>("MaxItemCount");

    Console.WriteLine($"SiteName: {siteName}");
    Console.WriteLine($"IsBasketEnabled: {isBasketEnabled}");
    Console.WriteLine($"MaxItemCount: {maxItemCount}");
}
catch (Exception ex)
{
    Console.WriteLine($"Hata: {ex.Message}");
}
