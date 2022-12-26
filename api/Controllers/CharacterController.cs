using api.DTOs.CharacterDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> GetAll()
    {
        return Ok(await _characterService.GetAll());
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetById(int Id)
    {
        var respone = await _characterService.GetById(Id);
        if (respone.Data == null) return NotFound(respone);
        return Ok(respone);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Add(AddCharacterDTO character)
    {
        return Ok(await _characterService.Add(character));
    }

    [HttpPut("Update")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> Update(UpdateCharacterDTO character)
    {
        var respone = await _characterService.Update(character);
        if (respone.Data == null) return NotFound(respone);
        return Ok(respone);
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Delete(int id)
    {
        var respone = await _characterService.Delete(id);
        if (respone.Data == null) return NotFound(respone);
        return Ok(respone);
    }
}