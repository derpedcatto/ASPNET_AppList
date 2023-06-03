namespace classwork1.Models
{
    public class RazorModel
    {
        public int IntValue { get; set; }
        public string StringValue { get; set; } = string.Empty;
        public List<String> ListValue { get; set; } = null!;
        public bool BoolValue { get; set; }
    }
}
