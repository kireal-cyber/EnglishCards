using EnglishCards.WebApi.Data;
using EnglishCards.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishCards.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly EnglishCardsDbContext _context;

    public TasksController(EnglishCardsDbContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        return await this._context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.Deck)
            .Include(t => t.WordCard)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await this._context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.Deck)
            .Include(t => t.WordCard)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return this.NotFound();
        }

        return task;
    }

    [HttpGet("by-deck/{deckId}")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetByDeck(int deckId)
    {
        return await this._context.TaskItems
            .Where(t => t.DeckId == deckId)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] TaskItem task)
    {
        if (!this.ModelState.IsValid)
        {
            return this.ValidationProblem(this.ModelState);
        }

        this._context.TaskItems.Add(task);
        await this._context.SaveChangesAsync();

        return this.CreatedAtAction(nameof(this.GetById), new { id = task.Id }, task);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskItem task)
    {
        if (id != task.Id)
        {
            return this.BadRequest();
        }

        if (!this.ModelState.IsValid)
        {
            return this.ValidationProblem(this.ModelState);
        }

        this._context.Entry(task).State = EntityState.Modified;
        await this._context.SaveChangesAsync();

        return this.NoContent();
    }


    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromQuery] TStatus status)
    {
        var task = await this._context.TaskItems.FindAsync(id);
        if (task == null)
        {
            return this.NotFound();
        }

        task.Status = status;
        task.CompletedAt = status == TStatus.Done ? DateTime.UtcNow : null;

        await this._context.SaveChangesAsync();
        return this.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await this._context.TaskItems.FindAsync(id);

        if (task == null)
        {
            return this.NotFound();
        }

        this._context.TaskItems.Remove(task);
        await this._context.SaveChangesAsync();

        return this.NoContent();
    }

    [HttpGet("assigned")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAssigned(
        [FromQuery] int? userId,
        [FromQuery] TStatus? status,
        [FromQuery] string? sort = "due")
    {
        var query = this._context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.Deck)
            .AsQueryable();


        query = query.Where(t => t.AssignedUserId != null);

        if (userId.HasValue)
        {
            query = query.Where(t => t.AssignedUserId == userId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        query = sort switch
        {
            "status" => query.OrderBy(t => t.Status).ThenBy(t => t.DueDate),
            _        => query.OrderBy(t => t.DueDate),
        };

        var tasks = await query.ToListAsync();
        return tasks;
    }
}
