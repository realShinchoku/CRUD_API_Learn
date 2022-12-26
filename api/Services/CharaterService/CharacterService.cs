using System.Security.Claims;
using api.Data;
using api.DTOs.CharacterDTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Services.CharaterService;

public class CharacterService : ICharacterService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAll()
    {
        var response = new ServiceResponse<List<GetCharacterDTO>>();
        var dbCharacter = await _context.Characters
            .Where(c => c.User.Id == GetuserId())
            .ToListAsync();
        response.Data = dbCharacter.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDTO>> GetById(int Id)
    {
        var response = new ServiceResponse<GetCharacterDTO>();
        try
        {
            var dbCharracter = await _context.Characters.FirstAsync(c => c.Id == Id);
            response.Data = _mapper.Map<GetCharacterDTO>(dbCharracter);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDTO>>> Add(AddCharacterDTO newCharacter)
    {
        var response = new ServiceResponse<List<GetCharacterDTO>>();
        var character = _mapper.Map<Character>(newCharacter);
        character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetuserId());
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        response.Data = await _context.Characters
            .Where(c => c.User.Id == GetuserId())
            .Select(c => _mapper.Map<GetCharacterDTO>(c))
            .ToListAsync();
        return response;
    }

    public async Task<ServiceResponse<GetCharacterDTO>> Update(UpdateCharacterDTO updatedCharacter)
    {
        var response = new ServiceResponse<GetCharacterDTO>();
        try
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            _mapper.Map(updatedCharacter, character);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDTO>(character);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDTO>>> Delete(int Id)
    {
        var response = new ServiceResponse<List<GetCharacterDTO>>();
        try
        {
            var character = await _context.Characters.FirstAsync(c => c.Id == Id);
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            response.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    private int GetuserId()
    {
        return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}