using FurnitureStore.Data;
using FurnitureStore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ClientController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _context.Clients.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Client>> Get(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if(client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Client client)
        {
            if(client == null)
            {
                return BadRequest();
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new {id = client.Id}, client);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(cl=>cl.Id == id);

            if (client == null) return BadRequest();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
