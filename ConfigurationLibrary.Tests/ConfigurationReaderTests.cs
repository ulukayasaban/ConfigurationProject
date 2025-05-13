using System;
using System.Reflection;
using ConfigurationLibrary.Models;

namespace ConfigurationLibrary.Tests;

public class ConfigurationReaderTests
{
    
    [Fact]
    public void GetValue_Should_Return_Correct_String()
    {
        var cache = new List<ConfigurationItem>
        {
            new ConfigurationItem { Name = "SiteName", Value = "soty.io", Type = "string", IsActive = true, ApplicationName = "SERVICE-A" }
        };

        var reader = CreateReaderWithFakeCache(cache);

        var result = reader.GetValue<string>("SiteName");

        Assert.Equal("soty.io", result);
    }

    [Fact]
    public void GetValue_Should_Throw_If_Key_Not_Found()
    {
        var reader = CreateReaderWithFakeCache(new List<ConfigurationItem>());
        Assert.Throws<KeyNotFoundException>(() => reader.GetValue<string>("NotFoundKey"));
    }

    [Fact]
    public void GetValue_Should_Throw_If_TypeMismatch()
    {
        var cache = new List<ConfigurationItem>
        {
            new ConfigurationItem { Name = "IsBasketEnabled", Value = "true", Type = "bool", IsActive = true, ApplicationName = "SERVICE-A" }
        };

        var reader = CreateReaderWithFakeCache(cache);
        Assert.Throws<InvalidCastException>(() => reader.GetValue<int>("IsBasketEnabled"));
    }

    private ConfigurationReader CreateReaderWithFakeCache(List<ConfigurationItem> cache)
    {
        var reader = new ConfigurationReader("SERVICE-A", "mongodb://fake", 5000);

        var cacheField = typeof(ConfigurationReader)
            .GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);
        cacheField!.SetValue(reader, cache);

        return reader;
    }
}
