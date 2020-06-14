using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace XdtPlayground.Monaco.Interop
{
    public class EditorContext
    {
        public string Id { get; }

        public string CurrentValue { get; private set; }

        private MonacoInterop _monacoInterop;

        public EventHandler<string> OnValueChanged { get; set; }

        public EditorContext(string id, string currentValue, MonacoInterop monacoInterop)
        {
            Id = id;
            CurrentValue = currentValue;
            _monacoInterop = monacoInterop;
        }

        [JSInvokable]
        public void ValueUpdated(string newValue)
        {
            CurrentValue = newValue;
            OnValueChanged?.Invoke(this, newValue);
        }

        public async Task UpdateValue(string newValue)
        {
            await _monacoInterop.InvokeVoidAsync("setValue", Id, newValue);
        }
    }
}
