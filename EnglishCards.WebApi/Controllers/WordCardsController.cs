using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnglishCards.WebApi.Data;
using EnglishCards.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishCards.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WordCardsController : ControllerBase
    {
        private readonly EnglishCardsDbContext _context;

        public WordCardsController(EnglishCardsDbContext context)
        {
            _context = context;
        }

        // GET: api/wordcards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WordCard>>> GetWordCards()
        {
            var cards = await _context.WordCards
                .Include(c => c.Deck)
                .ToListAsync();

            return Ok(cards);
        }

        // GET: api/wordcards/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<WordCard>> GetWordCard(int id)
        {
            var card = await _context.WordCards
                .Include(c => c.Deck)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (card == null)
            {
                return NotFound();
            }

            return Ok(card);
        }

        // GET: api/wordcards/by-deck/3
        [HttpGet("by-deck/{deckId:int}")]
        public async Task<ActionResult<IEnumerable<WordCard>>> GetWordCardsByDeck(int deckId)
        {
            var deckExists = await _context.Decks.AnyAsync(d => d.Id == deckId);
            if (!deckExists)
            {
                return NotFound($"Deck with id {deckId} not found.");
            }

            var cards = await _context.WordCards
                .Where(c => c.DeckId == deckId)
                .ToListAsync();

            return Ok(cards);
        }

        // POST: api/wordcards
        [HttpPost]
        public async Task<ActionResult<WordCard>> CreateWordCard([FromBody] WordCard card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Проверим, что указанная тема существует
            var deckExists = await _context.Decks.AnyAsync(d => d.Id == card.DeckId);
            if (!deckExists)
            {
                return BadRequest($"Deck with id {card.DeckId} does not exist.");
            }

            _context.WordCards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWordCard),
                new { id = card.Id },
                card);
        }

        // PUT: api/wordcards/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateWordCard(int id, [FromBody] WordCard card)
        {
            if (id != card.Id)
            {
                return BadRequest("WordCard ID in URL and body do not match.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Проверим, что тема существует
            var deckExists = await _context.Decks.AnyAsync(d => d.Id == card.DeckId);
            if (!deckExists)
            {
                return BadRequest($"Deck with id {card.DeckId} does not exist.");
            }

            _context.Entry(card).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.WordCards.AnyAsync(c => c.Id == id);
                if (!exists)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/wordcards/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWordCard(int id)
        {
            var card = await _context.WordCards.FirstOrDefaultAsync(c => c.Id == id);

            if (card == null)
            {
                return NotFound();
            }

            _context.WordCards.Remove(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
