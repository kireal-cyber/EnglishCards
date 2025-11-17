using System.Threading.Tasks;
using EnglishCards.WebApp.Models;
using EnglishCards.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCards.WebApp.Controllers
{
    public class DecksController : Controller
    {
        private readonly IDeckApiClient _deckApiClient;

        public DecksController(IDeckApiClient deckApiClient)
        {
            _deckApiClient = deckApiClient;
        }

        // GET: /Decks
        public async Task<IActionResult> Index()
        {
            var decks = await _deckApiClient.GetAllAsync();
            return View(decks);
        }

        // GET: /Decks/Create
        public IActionResult Create()
        {
            return View(new DeckViewModel());
        }

        // POST: /Decks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeckViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _deckApiClient.CreateAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при создании темы.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Decks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var deck = await _deckApiClient.GetByIdAsync(id);
            if (deck == null)
            {
                return NotFound();
            }

            return View(deck);
        }

        // POST: /Decks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DeckViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _deckApiClient.UpdateAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при обновлении темы.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Decks/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var deck = await _deckApiClient.GetByIdAsync(id);
            if (deck == null)
            {
                return NotFound();
            }

            return View(deck);
        }

        // POST: /Decks/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _deckApiClient.DeleteAsync(id);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при удалении темы.");
                var deck = await _deckApiClient.GetByIdAsync(id);
                return View("Delete", deck);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
