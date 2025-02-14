using Microsoft.Extensions.Options;
using MongoDB.Driver;
using FilmStore.Models;

namespace ArtistStore.Services;

public class ArtistsService
{
	private readonly IMongoCollection<Artist> _artistsCollection;

	public ArtistsService(IOptions<ProjectDatabaseSettings> settings)
	{
		var mongoClient = new MongoClient(
			settings.Value.ConnectionString);

		var mongoDatabase = mongoClient.GetDatabase(
			settings.Value.DatabaseName);

		_artistsCollection = mongoDatabase.GetCollection<Artist>(
			settings.Value.ArtistsCollectionName);
	}
	public async Task<List<Artist>> GetAsync() => 
		await _artistsCollection.Find(_ => true).ToListAsync();

	public async Task<Artist?> GetAsync(string id) =>
		await _artistsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

	public async Task CreateAsync(Artist newBook) =>
		await _artistsCollection.InsertOneAsync(newBook);

	public async Task UpdateAsync(string id, Artist updatedBook) =>
		await _artistsCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

	public async Task RemoveAsync(string id) =>
		await _artistsCollection.DeleteOneAsync(x => x.Id == id);
}
