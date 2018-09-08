using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleSurvey.Models;

namespace SimpleSurvey.Controllers
{
    [Route("api/[controller]")]
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
    }
}