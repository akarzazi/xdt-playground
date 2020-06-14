import 'monaco-editor/esm/vs/editor/browser/controller/coreCommands.js';
import * as monaco from 'monaco-editor/esm/vs/editor/editor.api.js';
import 'monaco-editor/esm/vs/basic-languages/xml/xml.contribution.js';
import { IBlazorInteropObject } from './IBlazorInteropObject';

//without this monaco fallbak on synchronous worker
//@ts-ignore
//self.MonacoEnvironment = {
//    getWorkerUrl: function (moduleId, label) {
//        console.log("getWorkerUrl", label);
//        return "/XdtPlayground/dist/monaco-editor/editor.worker.bundle.js";
//    }
//};


class MonacoInterop {

    private editors: { [id: string]: monaco.editor.IStandaloneCodeEditor } = {};
    private diffEditors: { [id: string]: monaco.editor.IStandaloneDiffEditor } = {};

    constructor() {
    }

    createEditor(
        id: string,
        container: HTMLElement,
        options: monaco.editor.IStandaloneEditorConstructionOptions,
        EditorContextRef: IBlazorInteropObject) {

        const editor = monaco.editor.create(container, options);

        this.editors[id] = editor;

        let model = editor.getModel();
        let timer = 0;
        editor.getModel().onDidChangeContent(_e => {

            if (timer) {
                clearTimeout(timer);
                timer = 0;
            }

            //timer = setTimeout(() => {
            //    timer = 0;
                EditorContextRef.invokeMethodAsync("ValueUpdated", model.getValue());
          //  }, 500);
        });
    }

    setValue(id: string, value: string) {
        let editor = this.editors[id];

        if (!editor) {
            throw "Editor not found" + id;
        }

        let model = editor.getModel();
        model.setValue(value);
    }

    createDiffEditor(id: string, container: HTMLElement, original: string, modified: string) {

        let diffEditor = monaco.editor.createDiffEditor(container, {
            readOnly: true,
            automaticLayout: true,
            scrollBeyondLastLine :false,
            scrollbar: {
                alwaysConsumeMouseWheel: false
            },
        });
        this.diffEditors[id] = diffEditor;

        diffEditor.setModel({
            original: monaco.editor.createModel(original, "xml"),
            modified: monaco.editor.createModel(modified, "xml")
        });
    }

    setDiffValue(id: string, original: string, modified: string) {
        let diffEditor = this.diffEditors[id];

        if (!diffEditor) {
            throw "DiffEditor not found" + id;
        }

        diffEditor.setModel({
            original: monaco.editor.createModel(original, "xml"),
            modified: monaco.editor.createModel(modified, "xml")
        });
    }
}

window['monacoInterop'] = new MonacoInterop();
