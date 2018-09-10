using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleSurvey.Models
{

  public class SurveyDefinition
  {
    [Key, Required]
    public int Id { get; set; } // PK
    [Required]
    public string Name { get; set; }

    [Required]
    public List<QuestionDefinition> Questions { get; set; }
  }

  public class Survey
  {
    [Required]
    private SurveyDefinition surveyDefinition { get; set; }
  }

  public class TakenSurvey
  {
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }

    [Required]
    public SurveyDefinition surveyDefinition { get; set; }
    // We can extend to be able to add users in the future if needed
    // public string userName;

  }
}