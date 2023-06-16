namespace shop.Models.Rate
{
	public class RateModel
	{
		public Guid ProductId { get; set; }
		public Guid UserId { get; set; }
		public int Rating { get; set; }
		public DateTime Moment { get; set; }
	}
}
