using AutoMapper;
using Models = Auth.Models;
using Entities = Auth.Entities;

namespace Auth.Services.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.AppException, Models.AppException>();
        }
    }
}
