using CapitalApp.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CapitalApp.Data
{
    public class ApplicationRepository: IApplicationRepository
    {
        private readonly CosmosClient cosmosClient;
        private readonly IConfiguration configuration;
        private readonly Container _applicationContainer;

        public ApplicationRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.cosmosClient = cosmosClient;
            this.configuration = configuration;
            var databaseName = configuration["CosmosDbSettings:DatabaseName"];
            var applicationContainerName = "Application";
            _applicationContainer = cosmosClient.GetContainer(databaseName, applicationContainerName);
        }

        public async Task<ApplicationDto> CreateApplicationAsync(ApplicationDto applicationDto)
        {
            var response = await _applicationContainer.CreateItemAsync(applicationDto);
            return response.Resource;
        }


        // 
        public async Task<ApplicationDto> GetApplicationByIdAsync(string applicationId)
        {
            var query = _applicationContainer.GetItemLinqQueryable<ApplicationDto>()
                    .Where(t => t.ApplicationID == applicationId)
                    .ToFeedIterator();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                var applications = response.ToList();

                if (applications.Count > 0)
                {
                    return applications.First(); // Return the first application found (assuming applicationId is unique)
                }
            }

            return null; // Return null if application with the given id is not found
        }


    }
}
