using System.Linq;
using System.Threading.Tasks;
using EnglishCards.WebApp.Models;
using EnglishCards.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnglishCards.WebApp.Controllers
{
    public class WordCardsController : Controller
    {
        private readonly IWordCardApiClient _wordCardApiClient;
        private readonly IDeckApiClient _deckApiClient;

        public WordCardsController(IWordCardApiClient wordCardApiClient, IDeckApiClient deckApiClient)
        {
            _wordCardApiClient = wordCardApiClient;
            _deckApiClient = deckApiClient;
        }

       
        private async Task PopulateDecksAsync(int? selectedDeckId = null)
        {
            var decks = await _deckApiClient.GetAllAsync();
            ViewBag.Decks = decks.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Selected = selectedDeckId.HasValue && d.Id == selectedDeckId.Value
            }).ToList();
        }

        
        public async Task<IActionResult> Index(int? deckId)
        {
            var cards = deckId.HasValue
                ? await _wordCardApiClient.GetByDeckAsync(deckId.Value)
                : await _wordCardApiClient.GetAllAsync();

            ViewBag.SelectedDeckId = deckId;

            return View(cards);
        }

        
        public async Task<IActionResult> Create(int? deckId)
        {
            await PopulateDecksAsync(deckId);
            var model = new WordCardViewModel
            {
                DeckId = deckId ?? 0
            };
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WordCardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDecksAsync(model.DeckId);
                return View(model);
            }

            var success = await _wordCardApiClient.CreateAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при создании карточки.");
                await PopulateDecksAsync(model.DeckId);
                return View(model);
            }

            return RedirectToAction(nameof(Index), new { deckId = model.DeckId });
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var card = await _wordCardApiClient.GetByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            await PopulateDecksAsync(card.DeckId);
            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WordCardViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await PopulateDecksAsync(model.DeckId);
                return View(model);
            }

            var success = await _wordCardApiClient.UpdateAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при обновлении карточки.");
                await PopulateDecksAsync(model.DeckId);
                return View(model);
            }

            return RedirectToAction(nameof(Index), new { deckId = model.DeckId });
        }

        // GET: /WordCards/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var card = await _wordCardApiClient.GetByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _wordCardApiClient.GetByIdAsync(id);
            var deckId = card?.DeckId;

            var success = await _wordCardApiClient.DeleteAsync(id);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при удалении карточки.");
                return View("Delete", card);
            }

            return RedirectToAction(nameof(Index), new { deckId });
        }
    }
}
