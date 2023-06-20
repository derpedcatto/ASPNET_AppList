namespace shop.Data.Entity
{
	public class Product
	{
		public Guid		 Id				{ get; set; }
		public Guid		 ProductGroupId { get; set; }
		public string	 Title			{ get; set; } = null!;
		public string?	 Description	{ get; set; }
		public float	 Price			{ get; set; }
		public DateTime  CreateDt		{ get; set; }
		public DateTime? DeleteDt		{ get; set; }
		public string	 ImageUrl		{ get; set; } = null!;

		// Navigation property
		public List<Rate> Rates { get; set; } = null!;
	}
}
