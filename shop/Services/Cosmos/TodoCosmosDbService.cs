using Microsoft.Azure.Cosmos;

namespace shop.Services.Cosmos
{
    public class TodoCosmosDbService : ICosmosDbService
    {
        private readonly IConfiguration _configuration;
        private Microsoft.Azure.Cosmos.Database todoDatabase;
        private Microsoft.Azure.Cosmos.Container itemsContainer;

        public TodoCosmosDbService(IConfiguration configuration)
        {
            _configuration = configuration;
            todoDatabase = null!;
            itemsContainer = null!;
        }

        public Container MainContainer
        {
            get
            {
                if (itemsContainer == null) // Перше звернення - треба підключатись
                {
                    var cosmos = _configuration.GetSection("Azure").GetSection("Cosmos");
                    string key = cosmos.GetValue<string>("Key");
                    string endpoint = cosmos.GetValue<string>("Endpoint");
                    string databaseId = cosmos.GetValue<string>("DatabaseId");
                    string containerId = cosmos.GetValue<string>("ContainerId");

                    todoDatabase = new CosmosClient(endpoint, key, new CosmosClientOptions() { ApplicationName = "shop" })
                                        .CreateDatabaseIfNotExistsAsync(databaseId)
                                        .Result;

                    itemsContainer = todoDatabase.CreateContainerIfNotExistsAsync(containerId, "/partitionKey")
                                        .Result;
                }
                return itemsContainer;
            }
        }
    }
}
