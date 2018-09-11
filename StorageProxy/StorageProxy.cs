using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SimpleSurvey.Models;

public class StorageProxy
{
  private SurveyContext dbContext;

  public StorageProxy(SurveyContext context)
  {
    dbContext = context;
  }

  public void CreateSurveyDef(SurveyDefinitionModel surveyData)
  {
    dbContext.SurveyTemplates.Add(surveyData);
    dbContext.SaveChanges();

    return;
  }

  public List<SurveyDefinitionModel> GetAllSurveyDefs()
  {
    var surveys = dbContext.SurveyTemplates
      .Include(survey => survey.Questions)
      .ToList();

    return surveys;
  }

  public SurveyDefinitionModel GetSurveyDefById(int surveyId)
  {

    // get survey by id
    var survey = dbContext.SurveyTemplates.Find(surveyId);

    // get questions for this survey
    // weird EF Core, have to invoke these before surveyTemplates can link them
    dbContext.Questions.ToList();

    return survey;
  }

  public SurveyDefinitionModel GetSurveyDefByName(string surveyName)
  {

    // get survey by name
    var survey = dbContext.SurveyTemplates.LastOrDefault(s => s.Name.ToLower() == surveyName.ToLower());
    // get questions for this survey - EF core weirdness
    dbContext.Questions.ToList();

    return survey;
  }

  public TakenSurveyModel GetTakenSurveyById(int surveyId)
  {
    // get survey by name - latest one by name
    var survey = dbContext.TakenSurveys.Find(surveyId);
    // get questions for this survey
    dbContext.Questions.ToList();

    return survey;
  }

  public TakenSurveyModel GetTakenSurveyByName(string surveyName)
  {
    // get survey by name - latest one by name
    var survey = dbContext.TakenSurveys.LastOrDefault(s => s.Name.ToLower() == surveyName.ToLower());
    // get questions for this survey
    dbContext.Questions.ToList();

    return survey;
  }

  public TakenSurveyModel GetTakenSurveyBySurveyTemplate(int templateId)
  {
    var survey = dbContext.TakenSurveys;
    var surveyBySurveyTemplate = survey.Where(s => s.Id == templateId);

    return surveyBySurveyTemplate.LastOrDefault();
  }

  public void SaveTakenSurvey(TakenSurveyModel takenSurvey)
  {
    dbContext.TakenSurveys.Add(takenSurvey);
    dbContext.SaveChanges();

    return;
  }
}