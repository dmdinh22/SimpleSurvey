using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleSurvey.Models;

namespace SimpleSurvey.Controllers
{
    [Route("api/survey")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly SurveyContext _context;

        public SurveyController(SurveyContext context)
        {
            _context = context;

            if (_context.SurveyItems.Count() == 0)
            {
                _context.SurveyItems.Add(new SurveyItem { Name = "Survey 1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<SurveyItem>> GetAllSurveys()
        {
            return _context.SurveyItems.ToList();
        }

        [HttpGet("{id}", Name = "GetSurvey")]
        public ActionResult<SurveyItem> GetById(int id)
        {
            var item = _context.SurveyItems.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }
    }
}