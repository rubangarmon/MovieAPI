using MovieAPI.Core.Models;
using MovieAPI.ServiceModel.DTOs;

namespace MovieAPI.Infrastructure.MappingProfiles;

public static class MultiMapper
{
    public static Response<MediaBase> MapMultiToMediaBase(ResponseDTO<MultiDTO> responseDTO)
    {
        var results = responseDTO.Results.Select(item => CreateMediaBase((MultiDTO)(object)item)).ToList();

        return new Response<MediaBase>
        {
            Results = results,
            TotalResults = responseDTO.TotalResults,
            TotalPages = responseDTO.TotalPages,
            Page = responseDTO.Page
        };
    }

    private static MediaBase CreateMediaBase(MultiDTO item)
    {
        return item.MediaType switch
        {
            "movie" => new Movie
            {
                Id = item.Id,
                OriginalTitle = item.Title,
                Overview = item.Overview,
                PosterPath = item.PosterPath,
                ReleaseDate = item.ReleaseDate,
                IsAdult = item.IsAdult
            },
            "tv" => new TvSerie
            {
                Id = item.Id,
                Name = item.Name,
                OriginalName = item.OriginalName,
                Overview = item.Overview,
                PosterPath = item.PosterPath,
                FirstAirDate = item.FirstAirDate,
                IsAdult = item.IsAdult
            },
            _ => throw new ArgumentException("Unknown media type")
        };
    }
}
