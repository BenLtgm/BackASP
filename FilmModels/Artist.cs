using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FilmModels;
public class Artist
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String? Id { get; set; }

    [BsonElement("name")]
    public String Name { get; set; }

    [BsonElement("birthdate")]
    public DateTime Birthdate { get; set; }

    [BsonElement("listeners")]
    public int Listeners { get; set; }

    [BsonElement("FilmIds")]
    public List<string>? FilmIds { get; set; }

    [BsonIgnore]
    public List<Film>? Films { get; set; }
}
