namespace shop.Models.Cosmos
{
    public class FeedbackFormModel
    {
        public string Name      { get; set; } = null!;
        public string Message   { get; set; } = null!;
        public string Rating    { get; set; } = null!;

        public Guid     id           { get; set; }
        public DateTime moment       { get; set; }
        public string   type         { get; set; } = "Feedback";
        public string   partitionKey { get; set; }
    }
}
