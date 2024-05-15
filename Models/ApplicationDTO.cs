using Newtonsoft.Json;

namespace CapitalApp.Models
{

    // Program model
    public class ProgramDto
    {
        [JsonProperty("programName")]
        public string? ProgramName { get; set; }
        [JsonProperty("programDescription")]
        public string? ProgramDescription { get; set; }
    }

    // Candidate model
    public class CandidateDto
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }
        [JsonProperty("lastName")]
        public string? LastName { get; set; }
        [JsonProperty("email")]
        public string? Email { get; set; }
    }

    // Question model
    public class QuestionDto
    {
        [JsonProperty("questionID")]
        public string? QuestionID { get; set; }
        [JsonProperty("type")]
        public string? Type { get; set; } // Paragraph, YesNo, Dropdown, MultipleChoice, Date, Number
        [JsonProperty("question")]
        public string? Question { get; set; }
    }

    // Application model combining Program, Candidate, and Questions
    public class ApplicationDto
    {
        [JsonProperty("applicationid")]
        public string? ApplicationID { get; set; }
        public ProgramDto? Program { get; set; }
        public CandidateDto? Candidate { get; set; }
        public List<QuestionDto>? Questions { get; set; }
    }





 
}
