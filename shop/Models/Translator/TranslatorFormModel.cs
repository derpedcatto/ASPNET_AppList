namespace shop.Models.Translator
{
    public class TranslatorFormModel
    {
        public string LangFrom { get; set; } = null!;
        public string LangTo { get; set;} = null!;
        public string OriginalText { get; set; } = null!;
        public string? TranslatedText { get; set;}
        public string? TranslateButton { get; set; }
        public string? InverseButton { get; set;}
    }
}
