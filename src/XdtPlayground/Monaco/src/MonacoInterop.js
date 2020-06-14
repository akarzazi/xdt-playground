"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("monaco-editor/esm/vs/editor/browser/controller/coreCommands.js");
var monaco = require("monaco-editor/esm/vs/editor/editor.api.js");
require("monaco-editor/esm/vs/basic-languages/xml/xml.contribution.js");
//without this monaco fallbak on synchronous worker
//@ts-ignore
//self.MonacoEnvironment = {
//    getWorkerUrl: function (moduleId, label) {
//        console.log("getWorkerUrl", label);
//        return "/XdtPlayground/dist/monaco-editor/editor.worker.bundle.js";
//    }
//};
var MonacoInterop = /** @class */ (function () {
    function MonacoInterop() {
        this.editors = {};
        this.diffEditors = {};
    }
    MonacoInterop.prototype.createEditor = function (id, container, options, EditorContextRef) {
        var editor = monaco.editor.create(container, options);
        this.editors[id] = editor;
        var model = editor.getModel();
        var timer = 0;
        editor.getModel().onDidChangeContent(function (_e) {
            if (timer) {
                clearTimeout(timer);
                timer = 0;
            }
            //timer = setTimeout(() => {
            //    timer = 0;
            EditorContextRef.invokeMethodAsync("ValueUpdated", model.getValue());
            //  }, 500);
        });
    };
    MonacoInterop.prototype.setValue = function (id, value) {
        var editor = this.editors[id];
        if (!editor) {
            throw "Editor not found" + id;
        }
        var model = editor.getModel();
        model.setValue(value);
    };
    MonacoInterop.prototype.createDiffEditor = function (id, container, original, modified) {
        var diffEditor = monaco.editor.createDiffEditor(container, {
            readOnly: true,
            automaticLayout: true,
            scrollBeyondLastLine: false,
            scrollbar: {
                alwaysConsumeMouseWheel: false
            },
        });
        this.diffEditors[id] = diffEditor;
        diffEditor.setModel({
            original: monaco.editor.createModel(original, "xml"),
            modified: monaco.editor.createModel(modified, "xml")
        });
    };
    MonacoInterop.prototype.setDiffValue = function (id, original, modified) {
        var diffEditor = this.diffEditors[id];
        if (!diffEditor) {
            throw "DiffEditor not found" + id;
        }
        diffEditor.setModel({
            original: monaco.editor.createModel(original, "xml"),
            modified: monaco.editor.createModel(modified, "xml")
        });
    };
    return MonacoInterop;
}());
window['monacoInterop'] = new MonacoInterop();
//# sourceMappingURL=MonacoInterop.js.map