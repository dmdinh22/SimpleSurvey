using Microsoft.EntityFrameworkCore;

namespace SimpleSurvey.Models
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options) : base(options)
        {

        }

        public DbSet<SurveyDefinition> SurveyTemplates { get; set; }
        public DbSet<QuestionDefinition> Questions { get; set; }
        public DbSet<TakenSurvey> TakenSurveys { get; set; }
    }
}