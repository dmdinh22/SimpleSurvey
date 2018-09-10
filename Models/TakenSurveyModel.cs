using System.ComponentModel.DataAnnotations;

namespace SimpleSurvey.Models
{
    public class TakenSurveyModel
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        public SurveyDefinitionModel SurveyDefinitionModel { get; set; }

        // We can extend to be able to add users in the future if needed
        // public string userName;
    }
}