using Microsoft.EntityFrameworkCore;

namespace SimpleSurvey.Models
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options) : base(options)
        {

        }

        public DbSet<SurveyDefinitionModel> SurveyTemplates { get; set; }
        public DbSet<QuestionModel> Questions { get; set; }
        public DbSet<TakenSurveyModel> TakenSurveys { get; set; }
    }
}