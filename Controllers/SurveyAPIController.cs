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
            //this.proxy = new StorageProxy(context);
        }

        // create survey template
        [HttpPost]
        public IActionResult CreateSurvey(SurveyDefinition survey)
        {
            var surveyData = new SurveyDefinition
            {
                Id = survey.Id,
                Name = survey.Name,
                Questions = new List<QuestionDefinition>()
            };

            foreach (var question in survey.Questions)
            {
                surveyData.Questions.Add(question);
            }

            //return this.proxy.CreateSurveyDefinition();
            _context.SurveyTemplates.Add(surveyData);
            _context.SaveChanges();

            return CreatedAtRoute(
                "GetSurvey", surveyData, survey
            );
        }

        [HttpGet]
        public ActionResult<List<SurveyDefinition>> GetAllSurveys()
        {
            //return this.proxy.GetAllSurveys();
            System.Console.WriteLine(_context.SurveyTemplates);
            var surveys = _context.SurveyTemplates
                .Include(survey => survey.Questions)
                .ToList();
            return surveys;
        }

        //get survey template by id
        [HttpGet("{id}", Name = "GetSurvey")]
        public ActionResult<SurveyDefinition> GetSurveyById(int id)
        {
            // get survey by id
            var item = _context.SurveyTemplates.Find(id);
            // get questions for this survey
            var questions = _context.Questions.ToList();

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // get survey template by name
        [HttpGet("name/{name}", Name = "GetSurveyByName")]
        public ActionResult<SurveyDefinition> GetSurveyByName(string name)
        {
            // get survey by id
            var item = _context.SurveyTemplates.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
            // get questions for this survey
            var questions = _context.Questions.ToList();

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // get taken survey by id
        //get survey template by id
        [HttpGet("taken/{id}", Name = "GetTakenSurvey")]
        public ActionResult<TakenSurvey> GetTakenSurveyById(int id)
        {
            // get survey by id
            var item = _context.TakenSurveys.Find(id);
            // get questions for this survey
            var questions = _context.Questions.ToList();

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }
        // get taken survey by name (most recent survey by that name)
        [HttpGet("taken/name/{name}", Name = "GetTakenSurveyByName")]
        public ActionResult<TakenSurvey> GetTakenSurveyByName(string name)
        {
            // get survey by id
            var item = _context.TakenSurveys.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
            // get questions for this survey
            var questions = _context.Questions.ToList();

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // upsert survey template -> taken survey
    }
}

/*

API Should Support:

Creating a survey
  insert into suvery[] - sends SurveyDefinition to server

Taking a Survey
  get surveyByName - sends SurveyDefinition to client
  insert takenSurvey - sends SurveyDefinition to server
    // apiController
      request = SurveyDef;
      sp.save(request.asUserTakenSurvey())

Getting Results of a Survey
  get surveyResults by name - sends most recent userTakenSurvey to client 
  get surveyResults by id - sends userTakenSurvey to client


  A survey should consist of survey questions and each question should have yes/no (true/false) answers
  Data Persistence: 

You will need to persist the data in some way.
You DO NOT need to use a database, and the easier for us to run it the better :).
But think about how you would want to do it in production and write up (one paragraph) how you would do it.

 */