using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace classwork6.Controllers
{
	[Route("api/test")]
	[ApiController]
	public class TestController : ControllerBase
	{
		[HttpGet]
		public object DoGet()
		{
			return new { Status = "GET works" };
		}

		[HttpPost]
		public object DoPost()
		{
			return new { Status = "POST works" };
		}

		[HttpPut]
		public object DoPut([FromBody] LoginPassword model)
		{
			return new { Status = $"PUT works with {model.Login}, {model.Password}" };
		}

		[HttpDelete("{id}")]
		public IEnumerable<object> DoDelete(string id)
		{
			return new object[]
			{
				new { num = 1, data = $"data1 {id}" },
				new { num = 2, data = $"data2 {id}" },
				new { num = 3, data = $"data3 {id}" }
			};
		}
	}

	public class LoginPassword
	{
		public string Login { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
