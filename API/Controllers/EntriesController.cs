using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class EntriesController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IEntryRepository _entryRepository;

        public EntriesController(DataContext context, IEntryRepository entryRepository)
        {
            _context = context;
            _entryRepository = entryRepository;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Entry>> CreateEntry(EntryDto entryDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == entryDto.UserName);

            if (user == null)
            {
                return NotFound("User not found");
            }
            
            var entry = new Entry
            {
                Title = entryDto.Title,
                Description = entryDto.Description,
                AppUserId = user.Id,
                AppUser = user
            };

            await _entryRepository.AddEntryAsync(entry);
            return CreatedAtAction(nameof(CreateEntry), new { id = entry.Id }, entry);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateEntry(EntryDto entryDto, int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == entryDto.UserName);
            
            if (id != entryDto.Id)
            {
                return BadRequest();
            }

            var entry = new Entry
            {
                Id = entryDto.Id,
                Title = entryDto.Title,
                Description = entryDto.Description
            };

            await _entryRepository.UpdateEntryAsync(entry);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            await _entryRepository.DeleteEntryAsync(id);
            return NoContent();
        }
    }
}
