namespace classwork4.Middleware
{
    public class SessionAuthMiddleware
    {
        /*
         Ланцюг викликів (pipeline) утворюється через те, що
         кожен клас Middleware викликає наступний клас.
         Контейнер ASP передає кожному класу посилання на наступний шар.
         Клас має зберігти це посилання та використати у своїх кодах.
        */
        private readonly RequestDelegate _next;

        public SessionAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /*
         Хоча класи Middleware не є нащадками якихось базових класів,
         вони повинні містити саме такий метод: InvokeAsync
         У старих схемах вживається синхронний варіант (Invoke), але
         він вважається застарілим и не радиться до вживання.
         Частиною методу має бути виклик наступного шару: await _next(context)
         Те, що передає _next, виконується на "прямій" ділянці оброблення
         запиту, після цієї інструкції - "зворотна" ділянка.
         Якщо _next не виконувати, то це припинить оброблення запиту. Таке може
         бути корисним, якщо подальша робота неможлива 
         (напр. зафіксовано неможливість підключення до БД)
         Оскільки конструктор задіяний для створення ланцюга викликів,
         інжекція служб (залежностей) здійснюється через метод
        */
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);   // Передача роботи наступній ланці
        }
    }

    /*
     Традиційно Middleware є створення розширень, які дозволять у Program.cs
     підключати цей Middleware за допомогою команди UseXxxx, де 
     Xxxx - це назва Middleware. У нашому випадку -
      UseSessionAuth()
     Без розширення це буде команда 
      app.UseMiddleware<SessionAuthMiddleware>();
     */
    public static class SessionAuthMiddlewareExtension
    {
        public static IApplicationBuilder UseSessionAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SessionAuthMiddleware>();
        }
    }
}
