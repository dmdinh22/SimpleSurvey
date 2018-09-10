namespace SimpleSurvey.Models
{
	public class QuestionModel
	{
		public int Id { get; set; } // PK
		public string QuestionText { get; set; }
		public bool? Answer { get; set; }
	}
}
// questionDefiniton

/*
TEST JSON OBJ
{
    "Name": "Survey",
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