namespace XdtPlayground.Monaco.Interop
{
    public class EditorOptions
    {
        public MinimapOptions Minimap { get; set; } = new MinimapOptions();
        public ScrollbarOptions Scrollbar { get; set; } = new ScrollbarOptions();
        public bool ReadOnly { get; set; }
        public bool AutomaticLayout { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }
        public bool ScrollBeyondLastLine { get; set; }
        public string AutoSurround { get; set; } = "never";
        public string AutoClosingOvertype { get; set; } = "never";
        public string AutoClosingQuotes { get; set; } = "never";
    }
}
