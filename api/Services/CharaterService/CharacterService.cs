using api.Data;
using api.DTOs.CharacterDTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.Services.CharaterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetuserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAll()
        {
            var respone = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharracter = await _context.Characters
                .Where(c => c.User.Id == GetuserId())
                .ToListAsync();
            respone.Data = dbCharracter.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            return respone;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetById(int Id)
        {
            var respone = new ServiceResponse<GetCharacterDTO>();
            try
            {
                var dbCharracter = await _context.Characters.FirstAsync(c => c.Id == Id);
                respone.Data = _mapper.Map<GetCharacterDTO>(dbCharracter);
            }
            catch (Exception ex)
            {
                respone.Success = false;
                respone.Message = ex.Message;
            }
            return respone;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> Add(AddCharacterDTO newCharacter)
        {
            var respone = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetuserId());
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            respone.Data = await _context.Characters
                .Where(c => c.User.Id == GetuserId())
                .Select(c => _mapper.Map<GetCharacterDTO>(c))
                .ToListAsync();
            return respone;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> Update(UpdateCharacterDTO updatedCharacter)
        {
            var respone = new ServiceResponse<GetCharacterDTO>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                _mapper.Map(updatedCharacter, character);
                await _context.SaveChangesAsync();
                respone.Data = _mapper.Map<GetCharacterDTO>(character);
            }
            catch (Exception ex)
            {
                respone.Success = false;
                respone.Message = ex.Message;
            }
            return respone;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> Delete(int Id)
        {
            var respone = new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                var character = await _context.Characters.FirstAsync(c => c.Id == Id);
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                respone.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                respone.Success = false;
                respone.Message = ex.Message;
            }
            return respone;
        }
    }
}