using EnglishCards.WebApi.Data;
using EnglishCards.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishCards.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly EnglishCardsDbContext _context;

    public UsersController(EnglishCardsDbContext context)
    {
        this._context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        return await this._context.Users
            .OrderBy(u => u.Name)
            .ToListAsync();
    }
}
