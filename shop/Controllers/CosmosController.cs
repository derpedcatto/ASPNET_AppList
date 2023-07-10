﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;   // NuGet
using shop.Models.Cosmos;
using shop.Services.Cosmos;

namespace shop.Controllers
{
    public class CosmosController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICosmosDbService _cosmosDb;

        public CosmosController(IConfiguration configuration, ICosmosDbService cosmosDb)
        {
            _configuration = configuration;
            _cosmosDb = cosmosDb;
        }

        public IActionResult Index([FromForm] FeedbackFormModel? formModel)
        {
            var itemsContainer = _cosmosDb.MainContainer;

            if (formModel?.Name != null && formModel.Message != null) 
            {
                formModel.id = Guid.NewGuid();
                formModel.partitionKey = formModel.id.ToString();
                formModel.moment = DateTime.Now;

                var response = itemsContainer.CreateItemAsync<FeedbackFormModel>(
                                    formModel, new PartitionKey(formModel.partitionKey))
                                        .Result;
            }

            var sqlQueryText = "SELECT * FROM c WHERE c.type = 'Feedback'";
            QueryDefinition queryDefinition = new(sqlQueryText);
            FeedIterator<FeedbackFormModel> queryResultSetIterator =
                itemsContainer.GetItemQueryIterator<FeedbackFormModel>(queryDefinition);

            List<FeedbackFormModel> feedbacks = new();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<FeedbackFormModel> currentResultSet = 
                    queryResultSetIterator.ReadNextAsync().Result;
                foreach (FeedbackFormModel item in currentResultSet)
                {
                    feedbacks.Add(item);
                }
            }
            ViewData["feedbacks"] = feedbacks;

            return View();
        }
    }
}
