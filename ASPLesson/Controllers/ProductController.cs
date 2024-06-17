using ASPLesson.Data;
using ASPLesson.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ASPLesson.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly StorageContext _context;

		public ProductController(StorageContext context)
		{
			_context = context;
		}

		[HttpPost]
		public ActionResult<int> AddProduct(string name, string desc, decimal price)
		{
			if (_context.Products.Any(p => p.Name == name))
				return StatusCode(409);

			var product = new Product() { Name = name, Price = price, Description = desc };
			_context.Products.Add(product);
			_context.SaveChanges();
			return Ok(product.Id);
		}

		[HttpGet("get_all_products")]
		public ActionResult<IEnumerable<Product>> GetProducts()
		{
			var list = _context.Products.Select(p =>
				new Product { Description = p.Description, Price = p.Price, Name = p.Name }).ToList();
			return Ok(list);
		}

		[HttpDelete("delete_product/{id}")]
		public ActionResult DeleteProduct(int id)
		{
			var product = _context.Products.Find(id);
			if (product == null)
				return NotFound();

			_context.Products.Remove(product);
			_context.SaveChanges();
			return Ok();
		}

		[HttpDelete("delete_group/{id}")]
		public ActionResult DeleteProductGroup(int id)
		{
			var productGroup = _context.ProductGroups.Include(pg => pg.Products).FirstOrDefault(pg => pg.Id == id);
			if (productGroup == null)
				return NotFound();

			_context.ProductGroups.Remove(productGroup);
			_context.SaveChanges();
			return Ok();
		}

		[HttpPut("set_price/{id}")]
		public ActionResult SetPrice(int id, decimal newPrice)
		{
			var product = _context.Products.Find(id);
			if (product == null)
				return NotFound();

			product.Price = newPrice;
			_context.SaveChanges();
			return Ok();
		}

		[HttpGet("export_csv")]
		public IActionResult ExportProductsToCsv()
		{
			var products = _context.Products.ToList();

			var csvBuilder = new StringBuilder();
			csvBuilder.AppendLine("Id,Name,Price,Description");
			foreach (var product in products)
			{
				csvBuilder.AppendLine($"{product.Id},{product.Name},{product.Price},{product.Description}");
			}

			var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
			return File(csvBytes, "text/csv", "products.csv");
		}
	}
}
