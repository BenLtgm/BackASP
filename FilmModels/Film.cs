using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FilmModels;
public class Film
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String? Id { get; set; }

    [BsonElement("name")]
    public String name { get; set; }

    [BsonElement("isStreamable")]
    public bool IsStreamable { get; set; }

    [BsonElement("releaseDate")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime ReleaseDate { get; set; }

    [BsonElement("time")]
    public int Time { get; set; }

}