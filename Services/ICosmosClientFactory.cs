using Microsoft.Azure.Cosmos;

namespace CapitalApp.Services
{
    public interface ICosmosClientFactory
    {
        CosmosClient CreateCosmosClient(IConfiguration configuration);
    }
}
