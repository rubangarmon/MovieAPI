using MovieAPI.Application.Configuration;
using MovieAPI.Core.HttpClients;
using MovieAPI.Infrastructure.HttpClients;
using MovieAPI.Infrastructure.MappingProfiles;
using System.Net.Http.Headers;
using MovieAPI.Core.Models;
using MovieAPI.ServiceModel.DTOs;
using MovieAPI.Application.Middlewares;
using MovieAPI.Core.Exceptions;
using MovieAPI.Application.Commons.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders().AddConsole();
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<DtoToEntity>();
});
var httpMovieRepoOptions = builder.Configuration.GetSection(HttpMovieRepositoryOptions.OptionName).Get<HttpMovieRepositoryOptions>()!;
Action<HttpClient> fc = client =>
{
    client.BaseAddress = new Uri(httpMovieRepoOptions.BaseURL);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.UserAgent.ParseAdd(httpMovieRepoOptions.UserAgent);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", httpMovieRepoOptions.Token);
};

builder.Services.AddTransient<IMovieApiProblemDetailsFactory, MovieApiProblemDetailsFactory>();
builder.Services.AddHttpClient<IHttpMediaRepository<Movie>, HttpTmdbMovieRepository<Movie, MovieDTO>>(fc);
builder.Services.AddHttpClient<IHttpMediaRepository<TvSerie>, HttpTmdbMovieRepository<TvSerie, TvSerieDTO>>(fc);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
