
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
            CreateMap<ResponseDTO, Response>().ReverseMap();
        }
    }
}
