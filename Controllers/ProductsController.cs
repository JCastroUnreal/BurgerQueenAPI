using BurgerQueenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using BurgerQueenAPI.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BurgerQueenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public ProductsController (ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// GET: api/products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// GET: api/products/1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Obtiene un producto buscandolo por su nombre (Parametro de consulta - Query Parameter)
        /// GET; api/products?name=nombreDelProducto
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<Product>> GetProductByName([FromQuery] string productName)
        {
            var product = await _context.Products.FindAsync($"{productName}");

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Obtine un producto buscandolo por su nombre (Paremtro de ruta - Route Parameter)
        /// GET: api/products/name/nombreDelProducto
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        [HttpGet("name/{productName}")]
        public async Task<ActionResult<Product>> GetsProductsByName(string productName)
        {
            var product = await _context.Products.FindAsync($"{productName}");

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Crea un nuevo producto, debes pasarle el cuerpo del producto
        /// POST: api/products
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("single")]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPost("multiple")]
        public async Task<ActionResult> CreateProducts([FromBody] List<Product> products)
        {
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            return Ok(products);
        }

        /// <summary>
        /// Actualiza un producto identificandolo por la id
        /// PUT: api/products/1
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id))
                {
                    return NotFound();
                } else { throw; }
            }
            return NoContent();
        }

        /// <summary>
        /// DELETE: api/products/1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// DELETE: api/products/bulk-delete
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("bulk-delete")]
        public async Task<IActionResult> DeleteProducts([FromBody] List<int> ids)
        {
            // Buscar los productos que coincidan con los IDs proporcionados
            var productsToDelete = await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();

            if (productsToDelete == null || !productsToDelete.Any())
            {
                return NotFound("No se encontraron productos con los IDs especificados.");
            }

            // Eliminar los productos de la base de datos
            _context.Products.RemoveRange(productsToDelete);
            await _context.SaveChangesAsync();

            return NoContent();

        }

    }
}
