using ASPLesson.Dto;
using ASPLesson.Models;

namespace ASPLesson.Abstraction
{
	public interface IProductGroupRepository
	{
		IEnumerable<ProductGroupDto> GetAllProductGroups();
		int AddProductGroup(ProductGroupDto productGroupDto);
		void DeleteProductGroup(int id);
	}
}
