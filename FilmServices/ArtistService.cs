using FilmModels;
using MongoDB.Driver;

namespace FilmServices;
public class ArtistService
{
    private readonly IMongoCollection<Artist> _artists;
    private readonly IMongoCollection<Film> _films;

    public ArtistService(DiscographyContext context)
    {
        _artists = context.Artists;
        _films = context.Films;
    }

    public async Task CreateAsync(Artist artist)
        => await _artists.InsertOneAsync(artist);

    public async Task AddFilmToArtist(string artistId, string filmId)
    {
        var filter = Builders<Artist>.Filter.Eq(a => a.Id, artistId);
        var update = Builders<Artist>.Update.Push(a => a.FilmIds, filmId);
        await _artists.UpdateOneAsync(filter, update);
    }

    public async Task DeleteFilmToArtist(string artistId, string filmId)
    {
        var filter = Builders<Artist>.Filter.Eq(a => a.Id, artistId);
        var update = Builders<Artist>.Update.Pull(a => a.FilmIds, filmId);
        await _artists.UpdateOneAsync(filter, update);
    }


    public async Task<bool> DeleteArtistAsync(string id)
    {
        var filter = Builders<Artist>.Filter.Eq(a => a.Id, id);
        var result = await _artists.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task<Artist> GetArtistByIdAsync(string id)
    {
        var filter = Builders<Artist>.Filter.Eq(a => a.Id, id);
        return await _artists.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpdateArtistAsync(string id, Artist updatedArtist)
    {
        var filter = Builders<Artist>.Filter.Eq(a => a.Id, id);
        var update = Builders<Artist>.Update
            .Set(a => a.Name, updatedArtist.Name)
            .Set(a => a.Birthdate, updatedArtist.Birthdate)
            .Set(a => a.Listeners, updatedArtist.Listeners)
            .Set(a => a.FilmIds, updatedArtist.FilmIds);

        await _artists.UpdateOneAsync(filter, update);
    }

    public async Task<Artist?> GetArtistWithFilmsAsync(string artistId)
    {
        var artist = await _artists.Find(a => a.Id == artistId)
            .FirstOrDefaultAsync();

        if (artist == null || artist.FilmIds == null || !artist.FilmIds.Any())
            return artist;

        var films = await _films.Find(f => artist.FilmIds.Contains(f.Id))
            .ToListAsync();

        artist.Films = films;
        return artist;
    }

    public async Task<List<Artist>> GetAllArtistsWithFilmsAsync()
    {
        var artists = await _artists.Find(_ => true)
            .ToListAsync();

        foreach (var artist in artists)
        {
            if (artist.FilmIds != null && artist.FilmIds.Any())
            {
                artist.Films = await _films.Find(f => artist.FilmIds.Contains(f.Id))
                    .ToListAsync();
            }
        }

        return artists;
    }
}