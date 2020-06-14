using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace XdtPlayground.Monaco.Interop
{
    public class MonacoInterop
    {
        private const string InteropPrefix = "monacoInterop.";

        private readonly IJSRuntime jsRuntime;
        private readonly ILogger logger;

        private static int _nextId = 0;
        private static int NewId()
        {
            return _nextId++;
        }

        public MonacoInterop(IJSRuntime jsRuntime, ILoggerFactory logFactory)
        {
            this.jsRuntime = jsRuntime;
            this.logger = logFactory.CreateLogger<MonacoInterop>();
        }

        public async ValueTask<EditorContext> CreateEditor(ElementReference element, EditorOptions editorOptions)
        {
            var editorId = NewId();
            var editorContext = new EditorContext(editorId.ToString(), editorOptions.Value, this);
            await InvokeVoidAsync("createEditor", editorId, element, editorOptions, DotNetObjectReference.Create(editorContext));
            return editorContext;
        }

        public async ValueTask<DiffEditorContext> CreateDiffEditor(ElementReference element, string original, string modified)
        {
            var editorId = NewId();
            var editorContext = new DiffEditorContext(editorId.ToString(), this);
            await InvokeVoidAsync("createDiffEditor", editorId, element, original, modified);
            return editorContext;
        }

        public ValueTask<TResult> InvokeAsync<TResult>(string methodName, params object[] args)
        {
            var fullname = InteropPrefix + methodName;
            logger.LogDebug("InvokeAsync: {0}", fullname);
            return jsRuntime.InvokeAsync<TResult>(fullname, args);
        }

        public ValueTask InvokeVoidAsync(string methodName, params object[] args)
        {
            var fullname = InteropPrefix + methodName;
            logger.LogDebug("InvokeAsync: {0}", fullname);
            return jsRuntime.InvokeVoidAsync(fullname, args);
        }
    }
}