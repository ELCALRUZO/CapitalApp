using Microsoft.Azure.Cosmos;

namespace CapitalApp.Services
{
    public class CosmosClientFactory : ICosmosClientFactory
    {
        public CosmosClient CreateCosmosClient(IConfiguration configuration)
        {
            var endpointUri = configuration["CosmosDbSettings:EndpointUri"];
            var primaryKey = configuration["CosmosDbSettings:PrimaryKey"];

            var cosmosClientOptions = new CosmosClientOptions
            {
                ApplicationName = configuration["CosmosDbSettings:DatabaseName"]
            };

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var cosmosClient = new CosmosClient(endpointUri, primaryKey, cosmosClientOptions);
            cosmosClient.ClientOptions.ConnectionMode = ConnectionMode.Gateway;

            return cosmosClient;
        }

    }
}
