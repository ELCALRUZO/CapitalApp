using CapitalApp.Data;
using CapitalApp.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register CosmosClient with dependency injection
builder.Services.AddSingleton<ICosmosClientFactory, CosmosClientFactory>();
builder.Services.AddSingleton<CosmosClient>(provider =>
{
    var cosmosClientFactory = provider.GetRequiredService<ICosmosClientFactory>();
    return cosmosClientFactory.CreateCosmosClient(configuration);
});

// Register repositories
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

var app = builder.Build();

// Initialize Cosmos DB containers
var cosmosClient = app.Services.GetRequiredService<CosmosClient>();

await InitializeCosmosDBContainers(cosmosClient, configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Method to initialize Cosmos DB containers
async Task InitializeCosmosDBContainers(CosmosClient cosmosClient, IConfiguration configuration)
{
    var databaseName = configuration["CosmosDbSettings:DatabaseName"];

    // Define container properties for Application container
    var applicationContainerProperties = new ContainerProperties
    {
        Id = "Application",
        PartitionKeyPath = "/applicationid",
        UniqueKeyPolicy = new UniqueKeyPolicy
        {
            UniqueKeys = { new UniqueKey { Paths = { "/ProgramName" } } }
        }
    };

    var cancellationToken = new CancellationToken();
    // Create Application container
    //await cosmosClient.GetDatabase(databaseName).CreateContainerIfNotExistsAsync(applicationContainerProperties);

    var containerResponse = await cosmosClient.GetDatabase(databaseName)
    .CreateContainerIfNotExistsAsync(applicationContainerProperties, cancellationToken: cancellationToken);

    if (containerResponse == null && containerResponse.Container == null)
    {
        // // Handle the error appropriately

        throw new Exception("Failed to create Cosmos DB container: Container response is null.");

    }


    // Define container properties for Question container
    var questionContainerProperties = new ContainerProperties
    {
        Id = "Question",
        PartitionKeyPath = "/questionID" // Assuming 'id' is the partition key
    };

    // Create Question container
    // await cosmosClient.GetDatabase(databaseName).CreateContainerIfNotExistsAsync(questionContainerProperties);

    // Create Question container
    var questionContainerResponse = await cosmosClient.GetDatabase(databaseName)
        .CreateContainerIfNotExistsAsync(questionContainerProperties, cancellationToken: cancellationToken);


    if (questionContainerResponse == null && questionContainerResponse.Container == null)
    {
        // // Handle the error appropriately 
            // Log the error or throw a custom exception
           // _logger.LogError("Failed to create Cosmos DB container: Container response is null.");
            throw new Exception("Failed to create Cosmos DB container: Container response is null.");
       
    }



}
