using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleSurvey.Models
{
  public class SurveyModel
  {
    [Required]
    private SurveyDefinitionModel SurveyDefinitionModel { get; set; }
  }
}