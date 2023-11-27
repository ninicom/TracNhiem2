namespace TracNhiem2.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string CorrectAnswer { get; set; }
        public int GroupId { get; set; }
        // Các thuộc tính khác nếu cần
    }

}
