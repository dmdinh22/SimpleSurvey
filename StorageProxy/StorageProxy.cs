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

  public void createSurveyDef(SurveyDefinitionModel surveyData)
  {
    dbContext.SurveyTemplates.Add(surveyData);
    dbContext.SaveChanges();

    return;
  }

  public List<SurveyDefinitionModel> getAllSurveyDefs()
  {
    var surveys = dbContext.SurveyTemplates
      .Include(survey => survey.Questions)
      .ToList();

    return surveys;
  }

  public SurveyDefinitionModel getSurveyDefById(int surveyId)
  {

    // get survey by id
    var survey = dbContext.SurveyTemplates.Find(surveyId);

    // get questions for this survey
    // weird EF Core, have to invoke these before surveyTemplates can link them
    dbContext.Questions.ToList();

    return survey;
  }

  // public Survey getTakenSurveyByName(string surveyName)
  // {
  //   // return latest one by name
  //   return context.TakenSurveyItems.find(surveyName).asc().first();
  // }
  // public Survey createSurveyDefinition(Survey survey)
  // {
  //   // create uuid

  //   // if _context == null, create

  //   _context.SurveyItems.Add(survey);
  //   _context.SaveChanges();
  // }

  // public Survey saveSurvey(Survey survey)
  // {
  //   _context.TakenSurveyItems.Add(survey);
  //   _context.SaveChanges();
  // }
}