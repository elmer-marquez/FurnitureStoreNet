using FurnitureStore.Data;
using FurnitureStore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public OrdersController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.Include(o=>o.OrderItems).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Order order)
        {
            if(order == null || order.OrderItems == null)
            {
                return BadRequest("Order should have at least one item");
            }

            await _context.Orders.AddAsync(order);
            await _context.OrderItems.AddRangeAsync(order.OrderItems);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Order order)
        {
            if(order == null || order.Id <= 0)
            {
                return BadRequest();
            }
            var ordr = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if(ordr == null)
            {
                return NotFound();
            }

            ordr.ClientId = order.ClientId;
            ordr.OrderNumber = order.OrderNumber;
            ordr.OrderDate = order.OrderDate;
            ordr.DeliveryDate = order.DeliveryDate; 
            ordr.OrderItems = order.OrderItems;

            _context.OrderItems.RemoveRange(ordr.OrderItems);

            _context.Orders.Update(order);
            _context.OrderItems.AddRange(order.OrderItems);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o=>o.OrderItems)
                .FirstOrDefaultAsync(o=>o.Id == id);
            if(order == null)
            {
                return NotFound();
            }

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
