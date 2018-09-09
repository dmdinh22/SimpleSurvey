namespace SimpleSurvey.Models
{
    public class QuestionDefinition
    {
        public int Id { get; set; } // PK
        public string QuestionText { get; set; }
        public bool? Answer { get; set; }
    }

}
// questionDefiniton
// id - PK
// string QuestionText
// boolean? answer

/*

{
    "Name": "Survey 3",
    "Questions": 
    [
    	{
    	"QuestionText": "Kiki do you love me?",
    	"Answer": false
    	},
    	{
    	"QuestionText": "Are you ridin?",
    	"Answer": false
    	},
    	{
    	"QuestionText": "Said you'll never ever leave from beside me?",
    	"Answer": true
    	}
    ]
}

 */