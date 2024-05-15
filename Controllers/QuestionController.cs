using CapitalApp.Data;
using CapitalApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapitalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        public QuestionController(IQuestionRepository questionRepository)
        {
              this._questionRepository = questionRepository;
        }

        //Get All Questions
        [HttpGet("questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllQuestions()
        {
           var questions= await  _questionRepository.GetAllQuestionsAsync();

            return Ok(questions);
        }


        //POST method to correctly store the different types of questions we have.
        [HttpPost]
        public async Task<IActionResult> CreateQuestion(QuestionDto questionDto)
        {
            try
            {
                var createdQuestion = await _questionRepository.CreateQuestionAsync(questionDto);
                return CreatedAtAction(nameof(GetQuestionById), new { questionId = createdQuestion.QuestionID }, createdQuestion);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        //PUT method if the user wants to edit the question after creating the application.

        [HttpPut("{questionId}")]
        public async Task<IActionResult> UpdateQuestion(string questionId, QuestionDto questionDto)
        {
            try
            {
                var existingquestion = await _questionRepository.GetQuestionByIdAsync(questionId);
                if (existingquestion == null)
                {
                    return BadRequest("Question ID mismatch.");
                }
                questionDto.QuestionID = existingquestion.QuestionID;

                var updatequestion =await _questionRepository.UpdateQuestionAsync(questionDto);
                return Ok(updatequestion);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        //Render the questions based on the question type.
        [HttpGet]
        public async Task<IActionResult> GetQuestions([FromQuery] string type)
        {
            try
            {
                var questions = await _questionRepository.GetByTypeAsync(type);
                return Ok(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        //Render the questions based on the questionID.
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetQuestionById(string questionId)
        {
            try
            {
                var question = await _questionRepository.GetQuestionByIdAsync(questionId);
                if (question == null)
                {
                    return NotFound();
                }

                return Ok(question);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }






























    }


}
