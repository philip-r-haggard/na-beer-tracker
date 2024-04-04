using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IEntryRepository
    {
        Task AddEntryAsync(Entry entry);
        Task UpdateEntryAsync(Entry entry);
        Task DeleteEntryAsync(int id);
        Task<Entry> GetEntryByIdAsync(int id);
        Task<IEnumerable<Entry>> GetEntriesAsync();
    }
}
