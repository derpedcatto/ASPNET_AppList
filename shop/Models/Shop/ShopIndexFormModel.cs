using Microsoft.AspNetCore.Mvc;

namespace shop.Models.Shop
{
	public class ShopIndexFormModel
	{
		[FromForm(Name = "productName")]
		public string Title { get; set; }

		[FromForm(Name = "productDescription")]
		public string? Description { get; set; }

		[FromForm(Name = "productGroup")]
		public Guid ProductGroupId { get; set; }

		[FromForm(Name = "productPrice")]
		public float Price { get; set; }

		[FromForm(Name = "productImage")]
		public IFormFile Image { get; set; }
	}
}
