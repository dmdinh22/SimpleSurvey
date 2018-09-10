using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleSurvey.Models
{
    public class SurveyDefinitionModel
    {
        [Key, Required]
        public int Id { get; set; } // PK
        [Required]
        public string Name { get; set; }

        [Required]
        public List<QuestionModel> Questions { get; set; }
    }
}