using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using FilmServices;
using FilmAuth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ProjectDatabaseSettings>(
	builder.Configuration.GetSection("IntergratedProject")
);
builder.Services.AddSingleton<FilmService>();
builder.Services.AddSingleton<ArtistService>();
builder.Services.AddSingleton<DiscographyContext>();
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:4200", "http://localhost:5036")
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.Authority = "https://dev-nu3v6yngsu53i6ru.us.auth0.com/";
    options.Audience = "http://localhost:5036";
});
var domain = "https://dev-nu3v6yngsu53i6ru.us.auth0.com/";
var permisions = new String[] {
    "get:film:specific",
    "create:film",
    "update:film",
    "delete:film",
    
    "create:artist",
    "get:artist:specific",
    "add:artist:film",
    "update:artist:specific",
    "delete:artist:film",
    "delete:artist"
};
builder.Services.AddAuthorization(options => {
    foreach (var permision in permisions) {
        options.AddPolicy(permision, policy =>
            policy.Requirements.Add(new HasScopeRequirement(permision, domain)
        ));
    }
});
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
