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
    public class DecksController : ControllerBase
    {
        private readonly EnglishCardsDbContext _context;

        public DecksController(EnglishCardsDbContext context)
        {
            _context = context;
        }

        // GET: api/decks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deck>>> GetDecks()
        {
            var decks = await _context.Decks.ToListAsync();
            return Ok(decks);
        }

        // GET: api/decks/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Deck>> GetDeck(int id)
        {
            var deck = await _context.Decks
                .Include(d => d.WordCards)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (deck == null)
            {
                return NotFound();
            }

            return Ok(deck);
        }

        // POST: api/decks
        [HttpPost]
        public async Task<ActionResult<Deck>> CreateDeck([FromBody] Deck deck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Decks.Add(deck);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDeck), new { id = deck.Id }, deck);
        }

        // PUT: api/decks/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDeck(int id, [FromBody] Deck deck)
        {
            if (id != deck.Id)
            {
                return BadRequest("Deck ID in URL and body do not match.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(deck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Decks.AnyAsync(d => d.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/decks/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDeck(int id)
        {
            var deck = await _context.Decks
                .Include(d => d.WordCards)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (deck == null)
            {
                return NotFound();
            }

            _context.Decks.Remove(deck);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
