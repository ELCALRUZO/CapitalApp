using CapitalApp.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CapitalApp.Data
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly CosmosClient cosmosClient;
        private readonly IConfiguration configuration;
        private readonly Container _questionContainer; 
        public QuestionRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
                this.cosmosClient= cosmosClient;    
                this.configuration= configuration;
            var databaseName = configuration["CosmosDbSettings:DatabaseName"];
            var questionContainerName = "Question";
            _questionContainer = cosmosClient.GetContainer(databaseName, questionContainerName);
        }

        public async Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync()
        {
            var query = _questionContainer.GetItemLinqQueryable<QuestionDto>().ToFeedIterator(); 

            var questions = new List<QuestionDto>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                questions.AddRange(response);
            }

            return questions; 
        }



        public async Task<QuestionDto> GetQuestionByIdAsync(string questionid)
        {
            var query = _questionContainer.GetItemLinqQueryable<QuestionDto>()
                    .Where(t => t.QuestionID == questionid)
                    .ToFeedIterator();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                var question = response.FirstOrDefault();
                if (question != null)
                {
                    return question;
                }
            }

            return null;
        }


        public async Task<IEnumerable<QuestionDto>> GetByTypeAsync(string type)
        {
            var query = _questionContainer.GetItemLinqQueryable<QuestionDto>().Where(t => t.Type == type)
                        .ToFeedIterator();

            var questions = new List<QuestionDto>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                questions.AddRange(response);
            }

            return questions;
        }


        public async Task<QuestionDto> CreateQuestionAsync(QuestionDto question)
        {
            var response = await _questionContainer.CreateItemAsync(question);  
            return response.Resource;
        }


        public async Task<QuestionDto> UpdateQuestionAsync(QuestionDto question)
        {
            var response = await _questionContainer.ReplaceItemAsync(question,question.Type);
            return response.Resource;
        }








    }
}
