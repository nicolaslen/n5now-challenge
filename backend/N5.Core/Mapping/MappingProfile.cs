using System;
using AutoMapper;
using N5.Core.Domain.Entities;
using N5.Core.DTOs;

namespace N5.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Permission, PermissionDto>()
                .ReverseMap()
                    .ForMember(x => x.Tipo, opt => opt.Ignore())
                .ForMember(x => x.FechaPermiso, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<PermissionType, PermissionTypeDto>()
                .ReverseMap();
        }
    }
}