using api.DTOs.CharacterDTO;

namespace api.Services.CharaterService;

public interface ICharacterService
{
    Task<ServiceResponse<List<GetCharacterDTO>>> GetAll();

    Task<ServiceResponse<GetCharacterDTO>> GetById(int Id);

    Task<ServiceResponse<List<GetCharacterDTO>>> Add(AddCharacterDTO character);

    Task<ServiceResponse<GetCharacterDTO>> Update(UpdateCharacterDTO character);

    Task<ServiceResponse<List<GetCharacterDTO>>> Delete(int Id);
}