using System.Net.Http.Headers;
using MovieAPI.Application.Configuration;
using MovieAPI.Core.HttpClients;
using MovieAPI.Infrastructure.HttpClients;
using MovieAPI.Infrastructure.MappingProfiles;
using MovieAPI.Core.Models;
using MovieAPI.ServiceModel.DTOs;
using MovieAPI.Application.Middlewares;
using MovieAPI.Core.Exceptions;
using MovieAPI.Application.Commons.Exceptions;
using MovieAPI.Application.Validators;
using MovieAPI.Application.ContractsModels;
using FluentValidation;
using Microsoft.Extensions.Logging.EventLog;

var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CA1416 // Validate platform compatibility
builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddEventLog(new EventLogSettings());
#pragma warning restore CA1416 // Validate platform compatibility
// Add services to the container.

builder.Services.AddControllers(
    opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

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

builder.Services.AddScoped<IValidator<MediaRequest>, MediaRequestValidator>();
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
