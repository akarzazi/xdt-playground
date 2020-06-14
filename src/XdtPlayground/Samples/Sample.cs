namespace XdtPlayground.Samples
{
    public class Sample
    {
        public string Category;
        public string Title;
        public string XML;
        public string XDT;

        public string DisplayTitle => ToDisplay(Title);
        public string DisplayCategory => ToDisplay(Category);

        public static string ToDisplay(string resourceName) { 
            return resourceName.Replace("_", " ").Replace("é", "(").Replace("è", ")").Replace("ù", "/");
        }
    }
}