using System.Collections.Generic;
using System.Threading.Tasks;
using EnglishCards.WebApp.Models;

namespace EnglishCards.WebApp.Services
{
    public interface IWordCardApiClient
    {
        Task<IReadOnlyList<WordCardViewModel>> GetAllAsync();
        Task<IReadOnlyList<WordCardViewModel>> GetByDeckAsync(int deckId);
        Task<WordCardViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(WordCardViewModel card);
        Task<bool> UpdateAsync(WordCardViewModel card);
        Task<bool> DeleteAsync(int id);
    }
}
