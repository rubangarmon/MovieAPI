using MovieAPI.Application.Configuration;
using MovieAPI.Core.HttpClients;
using MovieAPI.Infrastructure.HttpClients;
using MovieAPI.Infrastructure.MappingProfiles;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieAPI.Core.Models;
using MovieAPI.ServiceModel.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<DtoToEntity>();
});
//var config = builder.Services.Configure<HttpMovieRepositoryOptions>(conf => conf.getse)
var httpMovieRepoOptions = builder.Configuration.GetSection(HttpMovieRepositoryOptions.OptionName).Get<HttpMovieRepositoryOptions>()!;
Action<HttpClient> fc = client =>
{
    client.BaseAddress = new Uri(httpMovieRepoOptions.BaseURL);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.UserAgent.ParseAdd(httpMovieRepoOptions.UserAgent);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", httpMovieRepoOptions.Token);
};

builder.Services.AddHttpClient<IHttpMediaRepository<Movie>, HttpTmdbMovieRepository<Movie, MovieDTO>>(fc);
builder.Services.AddHttpClient<IHttpMediaRepository<TvSerie>, HttpTmdbMovieRepository<TvSerie, TvSerieDTO>>(fc);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
