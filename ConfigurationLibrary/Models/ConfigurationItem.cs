using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConfigurationLibrary.Models;

public class ConfigurationItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }  
    public string Value { get; set; }
    public bool IsActive { get; set; }
    public string ApplicationName { get; set; }
}
