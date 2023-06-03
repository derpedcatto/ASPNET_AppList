using userauth.Data;
using userauth.Models.User;
using userauth.Services.Hash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace userauth.Controllers
{
    public class UserController : Controller
    {
        // Підключення БД - інжекція залежності від контексту (зареєстрованого у Program.cs)
        private readonly DataContext _dataContext;
        private readonly IHashService _hashService;

        public UserController(DataContext dataContext, IHashService hashService)
        {
            _dataContext = dataContext;
            _hashService = hashService;
        }

        public IActionResult SignUp(UserSignupModel? model)
        {
            if (HttpContext.Request.Method == "POST")
            {
                ViewData["form"] = _ValidateModel(model);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult LogIn([FromForm]string login, [FromForm]string password)
        {
            var user = _dataContext.Users121.FirstOrDefault (x => x.Login == login);
            if (user != null) 
            {
                if (user.PasswordHash == _hashService.HashString(password))
                {
                    // Автентифікацію пройдено, зберігаємо сесії id користувача
                    HttpContext.Session.SetString("AuthUserId", user.Id.ToString());
                    return Json(new { status = "OK" });
                }
            }
            return Json(new { status = "NO" });
        }

        // Перевіряє валідність даних у моделі, прийнятої з форми
        // Повертає повідомлення про помилку валідації або String.Empty
        // Якщо перевірка успішна
        private string _ValidateModel(UserSignupModel? model)
        {
            if (model == null) { return "Дані не передані"; }
            if (String.IsNullOrEmpty(model.Login)) { return "Логін не може бути порожнім"; }
            if (!System.Text.RegularExpressions.Regex.IsMatch(model.Password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~]).{8,}$")) { return "Пароль как мінімум має містити:\n- 8 знаків;\n- 1 велику літеру;\n- 1 цифру;\n- 1 спеціальний символ;"; }
            if (model.Password != model.RepeatPassword) { return "Пароль не підтверджен."; }
            if (!System.Text.RegularExpressions.Regex.IsMatch(model.Email, "^[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*$")) { return "Email не валідний."; }
            if (!model.Agree) { return "Необхідно дотримуватись правил сайту"; }

            string? newName = null;
            if (model.AvatarFile != null)
            {
                string ext = Path.GetExtension(model.AvatarFile.FileName);
                if (ext != ".png" && ext != ".jpg") { return "Аватар повинен бути у форматі .png або .jpg."; }

                newName = Guid.NewGuid().ToString() + ext;
                model.AvatarFile.CopyTo(new FileStream($"wwwroot/uploads/{newName}", FileMode.Create));
            }

            // Check if login is already used
            bool queryLoginIsMatch = _dataContext.Users121
                                     .Any(row => row.Login == model.Login);
            if (queryLoginIsMatch) { return "Користувач з цим логіном вже зареєстрований.";  }

            // Додаємо користувача до БД
            _dataContext.Users121.Add(new Data.Entity.User
            {
                Id = Guid.NewGuid(),
                Login = model.Login,
                PasswordHash = _hashService.HashString(model.Password),
                Email = model.Email,
                Avatar = newName,
                RealName = model.RealName,
                RegisteredDt = DateTime.Now
            });
            _dataContext.SaveChanges(); // PlanetScale не підтримує асинхронні запити

            return String.Empty;
        }
    }
}
