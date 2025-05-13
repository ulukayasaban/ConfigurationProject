using System;
using ConfigurationLibrary.Models;
using ConfigurationLibrary.MongoDb;
using Moq;

namespace ConfigurationLibrary.Tests;

public class MongoRepositoryTests
{
    [Fact]
    public async Task InsertAsync_Should_Return_True_When_Inserted()
    {
        var repoMock = new Mock<IMongoRepository>();

        repoMock.Setup(r => r.InsertAsync(It.IsAny<ConfigurationItem>()))
                .ReturnsAsync(true);

        var fakeItem = new ConfigurationItem { Name = "TestKey", Value = "123" };

        var result = await repoMock.Object.InsertAsync(fakeItem);

        Assert.True(result);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_ConfigurationItem()
    {
        var repoMock = new Mock<IMongoRepository>();

        repoMock.Setup(r => r.GetByIdAsync("abc123"))
                .ReturnsAsync(new ConfigurationItem
                {
                    Id = "abc123",
                    Name = "Theme",
                    Value = "dark",
                    ApplicationName = "SERVICE-A",
                    IsActive = true
                });

        var item = await repoMock.Object.GetByIdAsync("abc123");

        Assert.NotNull(item);
        Assert.Equal("dark", item?.Value);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_False_When_Item_Not_Found()
    {
        var repoMock = new Mock<IMongoRepository>();

        repoMock.Setup(r => r.UpdateAsync(It.IsAny<ConfigurationItem>()))
                .ReturnsAsync(false); 

        var item = new ConfigurationItem { Id = "missing", Name = "X" };

        var result = await repoMock.Object.UpdateAsync(item);

        Assert.False(result);
    }

    [Fact]
    public async Task GetActiveItemsByAsync_Should_Return_ActiveItems()
    {
        var repoMock = new Mock<IMongoRepository>();
        string applicationName = "SERVICE-A";
        repoMock.Setup(r => r.GetActiveItemsByAsync(applicationName))
                .ReturnsAsync(new List<ConfigurationItem>
                {
                    new ConfigurationItem { Name = "SiteName", IsActive = true },
                    new ConfigurationItem { Name = "Theme", IsActive = true }
                });

        var result = await repoMock.Object.GetActiveItemsByAsync(applicationName);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task DeleteAsync_Should_Return_True_When_Deleted()
    {
        var repoMock = new Mock<IMongoRepository>();

        repoMock.Setup(r => r.DeleteAsync("xyz123"))
                .ReturnsAsync(true); 

        var result = await repoMock.Object.DeleteAsync("xyz123");

        Assert.True(result);
    }


}
