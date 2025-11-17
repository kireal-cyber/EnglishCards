using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EnglishCards.WebApp.Models;

namespace EnglishCards.WebApp.Services
{
    public class DeckApiClient : IDeckApiClient
    {
        private readonly HttpClient _httpClient;

        public DeckApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("EnglishCardsApi");
        }

        public async Task<IReadOnlyList<DeckViewModel>> GetAllAsync()
        {
            var decks = await _httpClient.GetFromJsonAsync<List<DeckViewModel>>("api/decks");
            return decks ?? new List<DeckViewModel>();
        }

        public async Task<DeckViewModel?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/decks/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<DeckViewModel>();
        }

        public async Task<bool> CreateAsync(DeckViewModel deck)
        {
            var response = await _httpClient.PostAsJsonAsync("api/decks", deck);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(DeckViewModel deck)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/decks/{deck.Id}", deck);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/decks/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
