namespace shop.Data.Entity
{
	public class ProductGroup
	{
		public Guid		 Id			 { get; set; }
		public string	 Title		 { get; set; } = null!;
		public string?	 Description { get; set; }
		public DateTime? DeleteDt	 { get; set; }
    }
}