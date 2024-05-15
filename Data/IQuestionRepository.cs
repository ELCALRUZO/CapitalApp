using CapitalApp.Models;

namespace CapitalApp.Data
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync();

        Task<QuestionDto> GetQuestionByIdAsync(string questionid);
         
        Task<IEnumerable<QuestionDto>> GetByTypeAsync(string type);

        Task<QuestionDto> CreateQuestionAsync(QuestionDto question);

        Task<QuestionDto> UpdateQuestionAsync(QuestionDto question);


    }
}
