using System.Reflection;
using ConfigurationLibrary.Models;

namespace ConfigurationLibrary.Tests;

public class ConfigurationReaderGetValueTests
{
    [Fact]
    public void GetValue_Should_Return_StringValue_When_KeyExists()
    {
        var fakeCache = new List<ConfigurationItem>
        {
            new ConfigurationItem
            {
                Name = "SiteName",
                Value = "soty.io",
                Type = "string",
                ApplicationName = "SERVICE-A",
                IsActive = true
            }
        };

        var reader = CreateReaderWithFakeCache(fakeCache);

        var result = reader.GetValue<string>("SiteName");

        Assert.Equal("soty.io", result);
    }

    [Fact]
    public void GetValue_Should_Throw_InvalidCast_When_TypeMismatch()
    {
        var fakeCache = new List<ConfigurationItem>
        {
            new ConfigurationItem
            {
                Name = "MaxItemCount",
                Value = "test.Value",  
                Type = "int",
                ApplicationName = "SERVICE-A",
                IsActive = true
            }
        };

        var reader = CreateReaderWithFakeCache(fakeCache);

        Assert.Throws<InvalidCastException>(() => reader.GetValue<int>("MaxItemCount"));
    }

    [Fact]
    public void GetValue_Should_Throw_KeyNotFound_When_KeyMissing()
    {
        var reader = CreateReaderWithFakeCache(new List<ConfigurationItem>());

        Assert.Throws<KeyNotFoundException>(() => reader.GetValue<string>("NonExistentKey"));
    }

    private ConfigurationReader CreateReaderWithFakeCache(List<ConfigurationItem> cache)
    {
        var reader = new ConfigurationReader("SERVICE-A", "mongodb://fake", 1000);

        var cacheField = typeof(ConfigurationReader)
            .GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);

        cacheField!.SetValue(reader, cache);

        return reader;
    }
}
