using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop.Data;
using shop.Models.Rate;

namespace shop.Controllers
{
	[Route("api/rate")]
	[ApiController]
	public class RateController : ControllerBase
	{
		private readonly DataContext _dataContext;

		public RateController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		[HttpPost]
		public object DoPost([FromBody] RateModel model)
		{
			var rate = _dataContext.Rates.Where(r => r.ProductId == model.ProductId
												  && r.UserId == model.UserId)
								   .FirstOrDefault();

			if (rate != null) // Existing rating
			{
				return new { Status = false, Message = "Оцінка була дана раніше" };
			}
			else // New rating
			{
				_dataContext.Rates.Add(new()
				{
					ProductId = model.ProductId,
					UserId = model.UserId,
					Rating = model.Rating,
					Moment = DateTime.Now,
				});
				_dataContext.SaveChanges();
				// Оновлення даних про рейтинг даного товару
				var product = _dataContext.Products
										  .Include(p => p.Rates)
										  .Where(p => p.Id == model.ProductId)
										  .First();
				// Включаємо оновлені дані у відповідь для відображення
				return new
				{
					Status = true,
					Message = "OK",
					Positive = product.Rates.Count(r => r.Rating > 0),
					Negative = product.Rates.Count(r => r.Rating < 0)
				};
			}
			// return model;
		}
	}
}
