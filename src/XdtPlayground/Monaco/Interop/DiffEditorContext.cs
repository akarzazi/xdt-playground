using System;
using System.Threading.Tasks;

namespace XdtPlayground.Monaco.Interop
{
    public class DiffEditorContext
    {
        /// <summary>
        /// Gets the generated id of the editor.
        /// </summary>
        public string Id { get; }

        private MonacoInterop _monacoInterop;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeEditor"/> class.
        /// </summary>
        /// <param name="id">The editor ID.</param>
        /// <param name="interop">The shared monaco interop wrapper.</param>
        public DiffEditorContext(string id, MonacoInterop monacoInterop)
        {
            Id = id;
            _monacoInterop = monacoInterop;
        }

        public async Task SetDiffValue(string original, string modified)
        {
            await _monacoInterop.InvokeVoidAsync("setDiffValue", Id, original, modified);
        }
    }
}
