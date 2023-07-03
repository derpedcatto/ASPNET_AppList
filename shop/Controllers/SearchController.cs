using Microsoft.AspNetCore.Mvc;
using shop.Models.ORM.AzureSearchImage;
using shop.Models.ORM.AzureSearchNews;
using shop.Models.ORM.AzureSearchVideo;
using shop.Models.ORM.AzureSearchWeb;
using shop.Models.Search;

namespace shop.Controllers
{
    public class SearchController : Controller
    {
        private readonly IConfiguration _configuration;

        private string _SearchResponse(string search, string searchPath)
        {
            var searchSection = _configuration.GetSection("Azure").GetSection("Search");
            string key = searchSection.GetValue<string>("Key");
            string endpoint = searchSection.GetValue<string>("Endpoint");
            string path = searchSection.GetValue<string>(searchPath);

            string query = $"{endpoint}{path}?q={Uri.EscapeDataString(search)}";

            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            return httpClient.GetAsync(query).Result.Content.ReadAsStringAsync().Result;
        }

        public SearchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index([FromQuery]string? search)
        {
            SearchWebModel model = new();

            // Пошуковий запит
            if (!String.IsNullOrEmpty(search))
            {
                string response = _SearchResponse(search, "PathWeb");

                var searchResponse = System.Text.Json.JsonSerializer
                                           .Deserialize<AzureSearchWebResponse>(response);

                model.SearchWebResponse = searchResponse;
            }
            return View(model);
        }

        public ViewResult Video([FromQuery]string? search)
        {
            SearchVideoModel model = new();

            if (!String.IsNullOrEmpty(search))
            {
                string response = _SearchResponse(search, "PathVideo");

                var searchResponse = System.Text.Json.JsonSerializer
                                           .Deserialize<AzureSearchVideoResponse>(response);

                model.SearchVideoResponse = searchResponse;
            }

            return View(model);
        }

        public ViewResult News([FromQuery]string? search)
        {
            SearchNewsModel model = new();

            if (!String.IsNullOrEmpty(search))
            {
                string response = _SearchResponse(search, "PathNews");

                var searchResponse = System.Text.Json.JsonSerializer
                           .Deserialize<AzureSearchNewsResponse>(response);

                model.SearchNewsResponse = searchResponse;
            }

            return View(model);
        }

		public ViewResult Image([FromQuery] string? search)
		{
			SearchImageModel model = new();

			if (!String.IsNullOrEmpty(search))
			{
				string response = _SearchResponse(search, "PathImage");

				var searchResponse = System.Text.Json.JsonSerializer
						   .Deserialize<AzureSearchImageResponse>(response);
                
				model.SearchImageResponse = searchResponse;
			}

			return View(model);
		}
	}
}
