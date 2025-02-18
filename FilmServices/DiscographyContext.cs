using FilmModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FilmServices;

public class DiscographyContext : DbContext
{
    private readonly IMongoDatabase _database;

    public DiscographyContext(IOptions<ProjectDatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<Artist> Artists => _database.GetCollection<Artist>("Artists");
    public IMongoCollection<Film> Films => _database.GetCollection<Film>("Films");
}
