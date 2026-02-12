using AutoMapper;
using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;
using AccountRegistrationApiDemo.Models.Entities;

namespace AccountRegistrationApiDemo.Mappings;

/// <summary>
/// AutoMapper profile for Account entity ↔ DTOs.
/// </summary>
public class AccountProfile : Profile
{
    public AccountProfile()
    {
        // Entity → Response DTOs
        CreateMap<Account, AccountResponse>();
        CreateMap<Account, AccountDetailResponse>();

        // Request DTOs → Entity
        CreateMap<CreateAccountRequest, Account>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<UpdateAccountRequest, Account>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
