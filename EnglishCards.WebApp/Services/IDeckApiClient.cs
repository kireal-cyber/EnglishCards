using System.Collections.Generic;
using System.Threading.Tasks;
using EnglishCards.WebApp.Models;

namespace EnglishCards.WebApp.Services
{
    public interface IDeckApiClient
    {
        Task<IReadOnlyList<DeckViewModel>> GetAllAsync();
        Task<DeckViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(DeckViewModel deck);
        Task<bool> UpdateAsync(DeckViewModel deck);
        Task<bool> DeleteAsync(int id);
    }
}
