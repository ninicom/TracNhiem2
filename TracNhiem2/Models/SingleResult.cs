namespace TracNhiem2.Models
{
    public class SingleResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int IdQuestionGroup { get; set; }
        public int Score { get; set; }
        public DateTime ComCompletionTime { get; set; }

    }
}
