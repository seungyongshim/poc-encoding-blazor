using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using BlazorApp2;
using BlazorMonaco;
using BlazorMonaco.Editor;

namespace BlazorApp2
{
    public partial class App
    {
        private StandaloneDiffEditor _diffEditor = null !;
        private string _valueToSetOriginal = "";
        private string _valueToSetModified = "";
        private StandaloneDiffEditorConstructionOptions DiffEditorConstructionOptions(StandaloneDiffEditor editor) => new StandaloneDiffEditorConstructionOptions
        {
            OriginalEditable = true
        };

        private async Task EditorOnDidInit()
        {
            // Get or create the original model
            var original_model = await Global.GetModel("sample-diff-editor-originalModel");
            if (original_model == null)
            {
                var original_value = """
                    뚫둛뚝뚜땃
                    """;
                original_model = await Global.CreateModel(original_value, "javascript", "sample-diff-editor-originalModel");
            }

            // Get or create the modified model
            var modified_model = await Global.GetModel("sample-diff-editor-modifiedModel");
            if (modified_model == null)
            {
                var modified_value = """
                    뚫둛뚝뚜
                    """;
                modified_model = await Global.CreateModel(modified_value, "javascript", "sample-diff-editor-modifiedModel");
            }

            // Set the editor model
            await _diffEditor.SetModel(new DiffEditorModel
            {
                Original = original_model,
                Modified = modified_model
            });
        }

        private void EditorOnKeyUpOriginal(KeyboardEvent keyboardEvent)
        {
            Console.WriteLine("OnKeyUpOriginal : " + keyboardEvent.Code);
        }

        private void EditorOnKeyUpModified(KeyboardEvent keyboardEvent)
        {
            Console.WriteLine("OnKeyUpModified : " + keyboardEvent.Code);
        }

        private async Task SetValueOriginal()
        {
            Console.WriteLine($"setting original value to: {_valueToSetOriginal}");
            await _diffEditor.OriginalEditor.SetValue(_valueToSetOriginal);
        }

        private async Task SetValueModified()
        {
            Console.WriteLine($"setting modified value to: {_valueToSetModified}");
            await _diffEditor.ModifiedEditor.SetValue(_valueToSetModified);
        }

        private async Task GetValueOriginal()
        {
            var val = await _diffEditor.OriginalEditor.GetValue();
            Console.WriteLine($"original value is: {val}");
        }

        private async Task GetValueModified()
        {
            var val = await _diffEditor.ModifiedEditor.GetValue();
            Console.WriteLine($"modified value is: {val}");
        }

        private async Task AddCommand()
        {
            await _diffEditor.AddCommand((int)KeyMod.CtrlCmd | (int)KeyCode.Enter, (args) =>
            {
                Console.WriteLine($"Ctrl+Enter : Diff Editor command is triggered. ({_diffEditor.Id})");
            });
        }

        private async Task AddAction()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Id = "testAction",
                Label = "Test Action",
                Keybindings = new int[]
                {
                    (int)KeyMod.CtrlCmd | (int)KeyCode.KeyB
                },
                ContextMenuGroupId = "navigation",
                ContextMenuOrder = 1.5f,
                Run = (editor) =>
                {
                    Console.WriteLine($"Ctrl+B : Diff Editor action is triggered. ({editor.Id})");
                }
            };
            await _diffEditor.AddAction(actionDescriptor);
        }
    }
}
