using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class EntryRepository : IEntryRepository
{
    private readonly DataContext _context;

    public EntryRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddEntryAsync(Entry entry)
    {
        try
        {
            // Add the entry to the context
            await _context.Entries.AddAsync(entry);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur
            Console.WriteLine($"An error occurred while adding entry: {ex.Message}");
            throw; // Re-throw the exception to propagate it upwards
        }
    }

    public async Task UpdateEntryAsync(Entry entry)
    {
        _context.Entry(entry).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEntryAsync(int id)
    {
        var entryToDelete = await _context.Entries.FindAsync(id);
        if (entryToDelete != null)
        {
            _context.Entries.Remove(entryToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Entry> GetEntryByIdAsync(int id)
    {
        return await _context.Entries.FindAsync(id);
    }

    public async Task<IEnumerable<Entry>> GetEntriesAsync()
    {
        return await _context.Entries.ToListAsync();
    }
}
