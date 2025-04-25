using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MovieAPI.Core.HttpClients;
using MovieAPI.Core.Models;
using MovieAPI.Infrastructure.HttpClients;
using MovieAPI.ServiceModel.DTOs;

namespace MovieAPI.IntegrationTests.Utils;

internal class MovieApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly HttpMessageHandler _mockHttpMessageHandler;

    public MovieApiWebApplicationFactory(HttpMessageHandler mockHttpMessageHandler)
    {
        _mockHttpMessageHandler = mockHttpMessageHandler;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.Sources.Clear();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "HttpMovieRepository:BaseURL", "http://fakeserver:3000/" },
                { "HttpMovieRepository__BaseURL", "http://fakeserver:3000/" },
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remover los HttpClients registrados en Program.cs para evitar conflictos
            services.RemoveAll<IHttpMediaRepository<Movie>>();
            services.RemoveAll<IHttpMediaRepository<TvSerie>>();
            services.RemoveAll<IHttpMediaRepository<MediaBase>>();

            // Registrar los HttpClients con el mock
            services.AddHttpClient<IHttpMediaRepository<Movie>, HttpTmdbMovieRepository<Movie, MovieDTO>>()
                .ConfigurePrimaryHttpMessageHandler(() => _mockHttpMessageHandler);

            services.AddHttpClient<IHttpMediaRepository<TvSerie>, HttpTmdbMovieRepository<TvSerie, TvSerieDTO>>()
                .ConfigurePrimaryHttpMessageHandler(() => _mockHttpMessageHandler);

            services.AddHttpClient<IHttpMediaRepository<MediaBase>, HttpTmdbMovieRepository<MediaBase, MultiDTO>>()
                .ConfigurePrimaryHttpMessageHandler(() => _mockHttpMessageHandler);
        });

        base.ConfigureWebHost(builder);
    }
}
