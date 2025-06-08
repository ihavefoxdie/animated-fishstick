using System;
using AutoMapper;
using UserManagement.Api.Entities;
using UserManagement.Models.DTOs;

namespace UserManagement.Api.Mappings.Profiles;

/// <summary>
/// Profile for <see cref="User"/> mapping to <see cref="UserDTO"/>.
/// </summary>
public class UserProfile : Profile
{
    /// <summary>
    /// A ctor; specifies how mapping from <see cref="User"/> to <see cref="UserDTO"/> must be done.
    /// </summary>
    public UserProfile()
    {
        CreateMap<User, UserDTO>()
            .ForCtorParam(ctorParamName: "Name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(ctorParamName: "Gender", opt => opt.MapFrom(src => src.Gender))
            .ForCtorParam(ctorParamName: "Birthday", opt => opt.MapFrom(src => src.Birthday))
            .ForCtorParam(ctorParamName: "Active", opt => opt.MapFrom(src => src.RevokedOn == null));

    }
}
