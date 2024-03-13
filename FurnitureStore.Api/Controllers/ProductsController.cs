using FurnitureStore.Data;
using FurnitureStore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ProductsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(pd=>pd.Id == id);

            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("category/{categoryID:int}")]
        public async Task<IEnumerable<Product>> GetByCategoryId(int categoryID)
        {

            return await _context.Products
                .Where(pd=>pd.CategoryId == categoryID)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            if(product == null)
            {
                return BadRequest();
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new {id = product.Id}, product);
        }


        [HttpPut]
        public async Task<IActionResult> Put(Product product)
        {
            if( product == null)
            {
                return BadRequest();
            }

            var pdUpdate = await _context.Products.FirstOrDefaultAsync(pd=>pd.Id == product.Id);
            if(pdUpdate == null)
            {
                return NotFound();
            }

            _context.Products.Update(pdUpdate);
            await _context.SaveChangesAsync();  

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(pd=>pd.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
