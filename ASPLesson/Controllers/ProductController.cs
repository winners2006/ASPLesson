using ASPLesson.Data;
using ASPLesson.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPLesson.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		[HttpPost]
		public ActionResult<int> AddProduct(string name, string desc, decimal price)
		{
			using (StorogeContext storogeContext = new StorogeContext())
			{
				if (storogeContext.Products.Any(p => p.Name == name))
					return StatusCode(409);

				var product = new Product() { Name = name, Price = price, Description = desc };
				storogeContext.Products.Add(product);
				storogeContext.SaveChanges();
				return Ok(product.Id);
			}
				
		}
		[HttpGet("get_all_products")]
		public ActionResult<IEnumerable<Product>> GetProducts()
		{
			using (StorogeContext storogeContext = new StorogeContext())
			{
				var list = storogeContext.Products.Select(p => 
					new Product { Description = p.Description, Price = p.Price, Name = p.Name }).ToList();

				return Ok(list);
			}

		}
		[HttpDelete("delete_product/{id}")]
		public ActionResult DeleteProduct(int id)
		{
			using (StorogeContext storogeContext = new StorogeContext())
			{
				var product = storogeContext.Products.Find(id);
				if (product == null)
				{
					return NotFound();
				}

				storogeContext.Products.Remove(product);
				storogeContext.SaveChanges();
				return Ok();
			}
		}

		[HttpDelete("delete_group/{id}")]
		public ActionResult DeleteProductGroup(int id)
		{
			using (StorogeContext storogeContext = new StorogeContext())
			{
				var productGroup = storogeContext.ProductGroups.Include(pg => pg.Products).FirstOrDefault(pg => pg.Id == id);
				if (productGroup == null)
				{
					return NotFound();
				}

				storogeContext.ProductGroups.Remove(productGroup);
				storogeContext.SaveChanges();
				return Ok();
			}
		}

		[HttpPut("set_price/{id}")]
		public ActionResult SetPrice(int id, decimal newPrice)
		{
			using (StorogeContext storogeContext = new StorogeContext())
			{
				var product = storogeContext.Products.Find(id);
				if (product == null)
				{
					return NotFound();
				}

				product.Price = newPrice;
				storogeContext.SaveChanges();
				return Ok();
			}
		}
	}
}
