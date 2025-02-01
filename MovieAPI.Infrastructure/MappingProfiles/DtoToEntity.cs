using AutoMapper;
using MovieAPI.Core.Models;
using MovieAPI.ServiceModel.DTOs;

namespace MovieAPI.Infrastructure.MappingProfiles
{
    public class DtoToEntity : Profile
    {
        public DtoToEntity()
        {
            CreateMap<MovieDTO, Movie>().ReverseMap();
            CreateMap<TvSerieDTO, TvSerie>().ReverseMap();
            CreateMap<ResponseDTO<MovieDTO>, Response<Movie>>().ReverseMap();
            CreateMap<ResponseDTO<TvSerieDTO>, Response<TvSerie>>().ReverseMap();
            CreateMap<ResponseDTO<MultiDTO>, Response<Movie>>().ReverseMap();
            CreateMap<ResponseDTO<MultiDTO>, Response<TvSerie>>().ReverseMap();
        }
    }
}
