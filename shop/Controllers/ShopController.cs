﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop.Data;
using shop.Models.Shop;
using System.Globalization;

namespace shop.Controllers
{
    public class ShopController : Controller
    {
        private readonly DataContext _dataContext;

		public ShopController(DataContext dataContext)
		{
            _dataContext = dataContext;
		}

		public IActionResult Index()
        {
			ShopIndexViewModel model = new()
			{
				ProductGroups = _dataContext.ProductGroups
											.Where(g => g.DeleteDt == null)
											.ToList(),
				Products = _dataContext.Products
									   .Include(p => p.Rates) // заповнює навінаційну властивість (включає до SQL запиту)
									   .Where(p => p.DeleteDt == null)
									   .ToList()
            };

			if (HttpContext.Session.Keys.Contains("AddMessage"))
			{
				model.AddMessage = HttpContext.Session.GetString("AddMessage");
				HttpContext.Session.Remove("AddMessage");
			}

            return View(model);
        }

        [HttpPost]
        public RedirectToActionResult AddProduct(ShopIndexFormModel model)
        {
            // Перевіряємо модель, зберігаємо файл, додаємо до БД, повертаємо повідомлення
			string errorMessage = _ValidateModel(model);
			if (errorMessage != string.Empty) 
			{
				HttpContext.Session.SetString("AddMessage", errorMessage);
			}
			else
			{
				HttpContext.Session.SetString("AddMessage", "Додано успішно");
			}
            return RedirectToAction(nameof(Index));
        }

		private string _ValidateModel(ShopIndexFormModel model)
		{
			if (model == null) { return "Дані не передані"; }

			if (model.ProductGroupId == Guid.Empty) { return "Виберіть категорію!"; }

			if (String.IsNullOrEmpty(model.Title)) { return "Назва не може бути порожньою"; }

			if (model.Price == 0)
			{
				// Можлива помилка декодування через локалізацію (1.5 | 1,5)
				model.Price = Convert.ToSingle(
					Request.Form["productPrice"].First()?.Replace(',', '.'),
					CultureInfo.InvariantCulture);
			}
			if (model.Price <= 0)
			{
				return "Ціна не може бути 0 або нижче!";
			}
			 
			string? newName = null;
			if (model.Image != null)
			{
				string ext = Path.GetExtension(model.Image.FileName);
				if (ext != ".png" && ext != ".jpg" && ext != ".jpeg") { return "Аватар повинен бути у форматі .png або .jpg."; }

				newName = Guid.NewGuid().ToString() + ext;
				model.Image.CopyTo(new FileStream(
					$"wwwroot/uploads/{newName}",
					FileMode.Create));
			}
			else
			{
				return "Файл-картинка необхідна";
			}

			// Додаємо користувача до БД
			_dataContext.Products.Add(new Data.Entity.Product
			{
				Id = Guid.NewGuid(),
				Title = model.Title,
				Description = model.Description,
				ProductGroupId = model.ProductGroupId,
				CreateDt = DateTime.Now,
				Price = model.Price,
				ImageUrl = newName
			});
			_dataContext.SaveChanges(); // PlanetScale не підтримує асинхронні запити

			return String.Empty;
		}
	}
}
