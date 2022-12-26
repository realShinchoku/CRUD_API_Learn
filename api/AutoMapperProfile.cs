using api.DTOs.CharacterDTO;
using api.DTOs.User;
using AutoMapper;

namespace api;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Character, GetCharacterDTO>();
        CreateMap<AddCharacterDTO, Character>();
        CreateMap<UpdateCharacterDTO, Character>();
        CreateMap<UserDto, User>();
    }
}