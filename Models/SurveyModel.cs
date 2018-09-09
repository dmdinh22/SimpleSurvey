using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleSurvey.Models
{
  // object HAS A ... (preferred)
  // object Is A ... 
  public interface ISurvey
  {
    SurveyDefinition getSurvey();
  }

  public class SurveyDefinition
  {
    [Key]
    public int Id { get; set; } // PK
    public string Name { get; set; }
    public List<QuestionDefinition> Questions { get; set; }
  }

  public class Survey : ISurvey
  {
    private SurveyDefinition surveyDefinition;

    public SurveyDefinition getSurvey()
    {
      return surveyDefinition;
    }
  }

  public class TakenSurvey : ISurvey
  {
    public int ID { get; set; }
    public string Name { get; set; }
    private SurveyDefinition surveyDefinition;
    // we could add users here in the future
    // public string userName;

    public SurveyDefinition getSurvey()
    {
      return surveyDefinition;
    }

  }
}