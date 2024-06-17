using Microsoft.AspNetCore.Mvc;
using ASPLesson.Dto;
using ASPLesson.Models;


namespace ASPLesson.Abstraction
{
	public interface IProductRepository
    {
        IEnumerable<ProductDto> GetAllProducts();
        int AddProduct(ProductDto productDto);
        void DeleteProduct(int id);
    }
}
