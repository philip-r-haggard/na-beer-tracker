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
        public async Task<IActionResult> UpdateEntry(int id, [FromBody] EntryDto entryDto)
        {
            // Validate incoming data
            if (entryDto == null || string.IsNullOrEmpty(entryDto.Title) || string.IsNullOrEmpty(entryDto.Description))
            {
                return BadRequest("Invalid entry data");
            }

            // Check if the entry exists
            var existingEntry = await _context.Entries.FindAsync(id);
            if (existingEntry == null)
            {
                return NotFound("Entry not found");
            }

            // Check if the user exists (optional)
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == entryDto.UserName);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update the entry
            existingEntry.Title = entryDto.Title;
            existingEntry.Description = entryDto.Description;

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts
                return Conflict("Concurrency conflict occurred");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            await _entryRepository.DeleteEntryAsync(id);
            return NoContent();
        }
    }
}
