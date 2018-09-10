using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleSurvey.Models;

namespace SimpleSurvey.Controllers
{
    [Route("api/survey")]
    [ApiController]
    public class SurveyAPIController : ControllerBase
    {
        //private StorageProxy proxy;
        private readonly SurveyContext _context;

        public SurveyAPIController(SurveyContext context)
        {
            _context = context;
            //_survey = survey;
            //this.proxy = new StorageProxy(context);
        }

        // Creating a survey
        [HttpPost]
        [Route("create")]
        public IActionResult CreateSurvey(SurveyDefinition payload)
        {
            try
            {
                var surveyData = new SurveyDefinition
                {
                    Id = payload.Id,
                    Name = payload.Name,
                    Questions = new List<QuestionDefinition>()
                };

                // add multiple records with AddRange() instead of looping through list
                surveyData.Questions.AddRange(payload.Questions);

                _context.SurveyTemplates.Add(surveyData);
                _context.SaveChanges();

                return Ok(surveyData);
            }
            catch (Exception ex)
            {
                // TODO: This is where I would log the error to have the stack trace
                // for the engineers to inspect. Since we don't have a logger implemented,
                // we will write it out to console for now and return a BadRequest for the client
                Console.WriteLine("Exception: " + ex);
                return BadRequest($"Error creating the {payload.Name} survey.");
            }
        }

        [HttpGet]
        public ActionResult<List<SurveyDefinition>> GetAllSurveyResults()
        {
            try
            {
                var surveys = _context.SurveyTemplates
                    .Include(survey => survey.Questions)
                    .ToList();

                return Ok(surveys);
            }
            catch (Exception ex)
            {
                // TODO: Log error
                Console.WriteLine("Exception: " + ex);
                return BadRequest("Unable to get all surveys");
            }
        }

        #region Get Surveys
        //get survey template by id
        [HttpGet("id/{id}", Name = "GetSurveyTemplateById")]
        public ActionResult<SurveyDefinition> GetSurveyTemplateById(int id)
        {
            try
            {
                // get survey by id
                var survey = _context.SurveyTemplates.Find(id);

                // get questions for this survey
                // weird EF Core, have to invoke these before surveyTemplates can link them
                _context.Questions.ToList();

                //var survey = this.proxy.getSurvey(id);

                if (survey == null)
                {
                    return NotFound();
                }

                return Ok(survey);
            }
            catch (Exception ex)
            {
                // TODO: Log error
                Console.WriteLine("Exception: " + ex);
                return BadRequest($"Error getting this survey with id of {id}");
            }
        }

        // get survey template by name
        [HttpGet("name/{name}", Name = "GetSurveyTemplateByName")]
        public ActionResult<SurveyDefinition> GetSurveyTemplateByName(string name)
        {
            try
            {
                // get survey by name
                var survey = _context.SurveyTemplates.LastOrDefault(s => s.Name.ToLower() == name.ToLower());
                // get questions for this survey - EF core weirdness
                var questions = _context.Questions.ToList();

                if (survey == null)
                {
                    return NotFound();
                }

                return Ok(survey);
            }
            catch (Exception ex)
            {
                // TODO: Log error
                Console.WriteLine("Exception: " + ex);
                return BadRequest($"Error getting this survey with name of {name}");
            }
        }
        #endregion

        #region Get Taken Surveys

        // Getting Results of a Survey
        //   get surveyResults by name - sends most recent userTakenSurvey to client 
        //   because we don't have a user, it's harder to manage the taken surveys
        [HttpGet("taken/id/{id}", Name = "GetTakenSurveyById")]
        public ActionResult<TakenSurvey> GetTakenSurveyById(int id)
        {

            // this is the unique id for the taken survey (not related to template id)
            var takenSurveyId = id;
            try
            {
                // get survey by id
                var survey = _context.TakenSurveys.Find(id);
                // get questions for this survey
                var questions = _context.Questions.ToList();

                // get answers for this survey 
                var surveyAnswers = new List<bool?>();

                foreach (var question in questions)
                {
                    surveyAnswers.Add(question.Answer);
                }

                if (survey == null)
                {
                    return NotFound();
                }

                return Ok(survey);
            }
            catch (Exception ex)
            {
                // TODO: Log error
                Console.WriteLine("Exception: " + ex);
                return BadRequest($"Error getting this survey with id of {id}");
            }
        }

        [HttpGet("taken/id/{id}", Name = "GetTakenSurveyByTemplateId")]
        public ActionResult<TakenSurvey> GetTakenSurveyByTemplateId(int templateid)
        {
            // get survey by id
            var surveys = _context.TakenSurveys;
            var surveysByTemplateType = surveys.Where(s => s.Id == templateid);

            return surveysByTemplateType.LastOrDefault();
        }

        // Getting Results of a Survey (most recent survey by that name)
        //   get surveyResults by name - sends most recent userTakenSurvey to client 
        [HttpGet("taken/name/{name}", Name = "GetTakenSurveyByName")]
        public ActionResult<TakenSurvey> GetTakenSurveyByName(string name)
        {
            try
            { // get survey by id
                var survey = _context.TakenSurveys.LastOrDefault(s => s.Name.ToLower() == name.ToLower());
                // get questions for this survey
                var questions = _context.Questions.ToList();

                if (survey == null)
                {
                    return NotFound();
                }

                return Ok(survey);
            }
            catch (Exception ex)
            {
                // TODO: Log error
                Console.WriteLine("Exception: " + ex);
                return BadRequest($"Error getting this survey with name of {name}");
            }
        }
        #endregion

        // Insert taken survey into db
        [HttpPost]
        public IActionResult TakeSurvey(SurveyDefinition payload)
        {
            try
            {
                var takenSurvey = new TakenSurvey();

                takenSurvey.surveyDefinition = new SurveyDefinition
                {
                    Id = payload.Id,
                    Name = payload.Name,
                    Questions = new List<QuestionDefinition>()
                };

                // do a null check for answers
                if (payload.Questions.Any(q => q.Answer == null))
                {
                    return BadRequest("test");
                }

                // add multiple records with AddRange instead of looping through
                takenSurvey.surveyDefinition.Questions.AddRange(payload.Questions);

                //return this.proxy.CreateSurveyDefinition();
                _context.TakenSurveys.Add(takenSurvey);
                _context.SaveChanges();

                return Ok(takenSurvey);
            }
            catch (Exception ex)
            {
                // TODO: Log error
                Console.WriteLine("Exception: " + ex);
                return BadRequest($"Error updating the {payload.Name} survey.");
            }
        }
    }
}

/*
API Should Support:

  A survey should consist of survey questions and each question should have yes/no (true/false) answers
  Data Persistence: 

You will need to persist the data in some way.
You DO NOT need to use a database, and the easier for us to run it the better :).
But think about how you would want to do it in production and write up (one paragraph) how you would do it.

 */

// ## QUESTIONS
/*
    - What is the different bt TakenSurvey and SurveyDefinition - how to identify when sending to server/client 
    - when do you fill out the TakenSurvey DbSet?
    - How does taking survey work - Used to making a GET to display the info, and POST to send the user data(input) - how do you combine into one endpoint?
    - Implementing the Interface
    - Implementing the SP
 */