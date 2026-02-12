using AutoMapper;
using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;
using AccountRegistrationApiDemo.Models.Entities;

namespace AccountRegistrationApiDemo.Mappings;

/// <summary>
/// AutoMapper profile for Registration entity ↔ DTOs.
/// </summary>
public class RegistrationProfile : Profile
{
    public RegistrationProfile()
    {
        // Entity → Response DTO
        CreateMap<Registration, RegistrationResponse>();

        // Request DTO → Entity
        CreateMap<CreateRegistrationRequest, Registration>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore());
    }
}
