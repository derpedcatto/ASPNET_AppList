namespace shop.Data.Entity
{
	public class Rate
	{
		public Guid		ProductId	{ get; set; }
		public Guid		UserId	{ get; set; }
		public int		Rating	{ get; set; }
		public DateTime Moment	{ get; set; }

        // Navigation property
        public Product Product { get; set; } = null!;
	}
}
