using Microsoft.EntityFrameworkCore;

namespace SimpleSurvey.Models
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options) : base(options)
        {

        }

        public DbSet<SurveyItem> SurveyItems { get; set; }
    }
}