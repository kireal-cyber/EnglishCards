using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EnglishCards.WebApp.Models;

namespace EnglishCards.WebApp.Services
{
    public class WordCardApiClient : IWordCardApiClient
    {
        private readonly HttpClient _httpClient;

        public WordCardApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("EnglishCardsApi");
        }

        public async Task<IReadOnlyList<WordCardViewModel>> GetAllAsync()
        {
            var cards = await _httpClient.GetFromJsonAsync<List<WordCardViewModel>>("api/wordcards");
            return cards ?? new List<WordCardViewModel>();
        }

        public async Task<IReadOnlyList<WordCardViewModel>> GetByDeckAsync(int deckId)
        {
            var cards = await _httpClient.GetFromJsonAsync<List<WordCardViewModel>>($"api/wordcards/by-deck/{deckId}");
            return cards ?? new List<WordCardViewModel>();
        }

        public async Task<WordCardViewModel?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/wordcards/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<WordCardViewModel>();
        }

        public async Task<bool> CreateAsync(WordCardViewModel card)
        {
            var response = await _httpClient.PostAsJsonAsync("api/wordcards", card);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(WordCardViewModel card)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/wordcards/{card.Id}", card);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/wordcards/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
