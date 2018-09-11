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
        private StorageProxy _storageProxy;
        private readonly SurveyContext _context;

        public SurveyAPIController(SurveyContext context)
        {
            _context = context;
            this._storageProxy = new StorageProxy(context);
        }

        // Create a Survey Template
        [HttpPost]
        [Route("create")]
        public IActionResult CreateSurvey(SurveyDefinitionModel payload)
        {
            try
            {
                var surveyData = new SurveyDefinitionModel
                {
                    Id = payload.Id,
                    Name = payload.Name,
                    Questions = new List<QuestionModel>()
                };

                // add multiple records with AddRange() instead of looping through list
                surveyData.Questions.AddRange(payload.Questions);

                this._storageProxy.CreateSurveyDef(surveyData);

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

        // Get All Survey Templates
        [HttpGet]
        public ActionResult<List<SurveyDefinitionModel>> GetAllSurveyResults()
        {
            try
            {
                var surveys = this._storageProxy.GetAllSurveyDefs();

                return Ok(surveys);
            }
            catch (Exception ex)
            {
                // TODO: Log error
                Console.WriteLine("Exception: " + ex);
                return BadRequest("Unable to get all surveys.");
            }
        }

        #region Get Surveys
        // Get Survey Template by ID
        [HttpGet("id/{id}", Name = "GetSurveyTemplateById")]
        public ActionResult<SurveyDefinitionModel> GetSurveyTemplateById(int id)
        {
            try
            {
                // call storage _storageProxy and set it to a local var
                var survey = this._storageProxy.GetSurveyDefById(id);

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
                return BadRequest($"Error getting this survey with id of {id}.");
            }
        }

        // Get Survey Template by Name
        [HttpGet("name/{name}", Name = "GetSurveyTemplateByName")]
        public ActionResult<SurveyDefinitionModel> GetSurveyTemplateByName(string name)
        {
            try
            {
                var survey = this._storageProxy.GetSurveyDefByName(name);

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
                return BadRequest($"Error getting this survey with name of {name}.");
            }
        }
        #endregion

        #region Get Taken Surveys

        // Getting Results of a Taken Survey by Id  
        //   because we don't have a user, it's harder to manage the taken surveys
        [HttpGet("taken/id/{id}", Name = "GetTakenSurveyById")]
        public ActionResult<TakenSurveyModel> GetTakenSurveyById(int id)
        {
            try
            {
                // get survey by id
                var survey = this._storageProxy.GetTakenSurveyById(id);

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
                return BadRequest($"Error getting this survey with id of {id}.");
            }
        }

        // Getting Results of a Taken Survey by Name (most recent survey by that name)
        [HttpGet("taken/name/{name}", Name = "GetTakenSurveyByName")]
        public ActionResult<TakenSurveyModel> GetTakenSurveyByName(string name)
        {
            try
            {
                var survey = this._storageProxy.GetTakenSurveyByName(name);

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
                return BadRequest($"Error getting the latest survey with Name of {name}.");
            }
        }

        // Getting Results of a Taken Survey by a certain Template (by ID) (most recent input)
        // In the case that we would want to retrieve results of a certain survey template
        [HttpGet("taken/templateid/{id}", Name = "GetTakenSurveyBySurveyTemplate")]
        public ActionResult<TakenSurveyModel> GetTakenSurveyBySurveyTemplate(int templateId)
        {
            try
            {
                var survey = this._storageProxy.GetTakenSurveyBySurveyTemplate(templateId);

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
                return BadRequest($"Error getting the latest survey with Template ID of {templateId}.");
            }
        }
        #endregion

        // Insert Taken Survey into DB
        [HttpPost]
        public IActionResult TakeSurvey(SurveyDefinitionModel payload)
        {
            try
            {
                var takenSurvey = new TakenSurveyModel();

                takenSurvey.SurveyDefinitionModel = new SurveyDefinitionModel
                {
                    Id = payload.Id,
                    Name = payload.Name,
                    Questions = new List<QuestionModel>()
                };

                // do a null check for answers
                if (payload.Questions.Any(q => q.Answer == null))
                {
                    return BadRequest("test");
                }

                // add multiple records with AddRange instead of looping through
                takenSurvey.SurveyDefinitionModel.Questions.AddRange(payload.Questions);

                this._storageProxy.SaveTakenSurvey(takenSurvey);

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