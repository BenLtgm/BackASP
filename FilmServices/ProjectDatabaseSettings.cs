namespace FilmServices;

public class ProjectDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string FilmsCollectionName { get; set; } = null!;
    public string ArtistsCollectionName { get; set; } = null!;
}

