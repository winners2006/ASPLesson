using ASPLesson.Dto;
using ASPLesson.Models;
using AutoMapper;

namespace ASPLesson.Mapper
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Product, ProductDto>().ReverseMap();
			CreateMap<ProductGroup, ProductGroupDto>().ReverseMap();
		}
	}
}
