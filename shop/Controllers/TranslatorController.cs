using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using shop.Models.ORM.Translator;
using shop.Models.Translator;
using System.Text;
using System.Text.Json;

namespace shop.Controllers
{
    public class TranslatorController : Controller
    {
        private readonly IConfiguration _configuration;

        public TranslatorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ViewResult> Index(TranslatorFormModel? formModel)
        {
            TranslatorViewModel viewModel = new();
            viewModel.FormModel = formModel;

            if (formModel != null && (formModel.TranslateButton != null || formModel.InverseButton != null))
            {
                var section = _configuration.GetSection("Azure").GetSection("Translator");
                string key = section.GetValue<string>("Key");
                string endpoint = section.GetValue<string>("Endpoint");
                string region = section.GetValue<string>("Region");

                string route = "/translate?api-version=3.0&";
                string textToTranslate = "";

                // Звичайний переклад
                if (formModel.TranslateButton != null)
                {
                    if (string.IsNullOrEmpty(formModel.OriginalText))
                    {
                        viewModel.ErrorMessage = "Відсутній текст для перекладу! (<-)";
                    }
                    else
                    {
                        route += $"from={formModel.LangFrom}&to={formModel.LangTo}";
                        textToTranslate = formModel.OriginalText;

                        viewModel.FormModel.TranslateButton = "yes";
                        viewModel.FormModel.InverseButton = "no";
                    }
                }
                // Зворотній переклад
                else if (formModel.InverseButton != null)
                {
                    if (string.IsNullOrEmpty(formModel.TranslatedText))
                    {
                        viewModel.ErrorMessage = "Відсутній текст для перекладу! (->)";
                    }
                    else
                    {
                        route += $"from={formModel.LangTo}&to={formModel.LangFrom}";
                        textToTranslate = formModel.TranslatedText;

                        viewModel.FormModel.TranslateButton = "no";
                        viewModel.FormModel.InverseButton = "yes";
                    }
                }

                if (viewModel.ErrorMessage == null)
                {
                    object[] body = new object[] { new { Text = textToTranslate } };
                    var requestBody = JsonSerializer.Serialize(body);

                    using (var client = new HttpClient())
                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri(endpoint + route);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                        request.Headers.Add("Ocp-Apim-Subscription-Region", region);
                        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                        string result = await response.Content.ReadAsStringAsync();
                        
                        var translation = JsonSerializer.Deserialize<List<TranslationResult>>(result);

                        viewModel.FormModel.TranslatedText = translation[0].translations[0].text;
                    }
                }
            }
            return View(viewModel);
        }
    }
}
