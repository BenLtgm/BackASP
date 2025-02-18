using FilmModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FilmServices;

public class FilmService
{
    private readonly IMongoCollection<Film> _filmsCollection;

    public FilmService(IOptions<ProjectDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(
            settings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            settings.Value.DatabaseName);

        _filmsCollection = mongoDatabase.GetCollection<Film>(
            settings.Value.FilmsCollectionName);
    }

    public async Task<List<Film>> GetAsync() =>
            await _filmsCollection.Find(_ => true).ToListAsync();

    public async Task<Film?> GetAsync(string id) =>
        await _filmsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Film newBook) =>
        await _filmsCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Film updatedBook) =>
        await _filmsCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _filmsCollection.DeleteOneAsync(x => x.Id == id);
}