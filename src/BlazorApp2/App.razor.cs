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
            OriginalEditable = true,
            Contextmenu = false,
            CodeLens = false,
            IgnoreTrimWhitespace = false,
            
        };

        private async Task EditorOnDidInit()
        {
            // Get or create the original model
            var original_model = await Global.GetModel("sample-diff-editor-originalModel");
            if (original_model == null)
            {
                var original_value = "";
                original_model = await Global.CreateModel(original_value, "text/plain", "sample-diff-editor-originalModel");
            }

            // Get or create the modified model
            var modified_model = await Global.GetModel("sample-diff-editor-modifiedModel");
            if (modified_model == null)
            {
                var modified_value = "";
                modified_model = await Global.CreateModel(modified_value, "text/plain", "sample-diff-editor-modifiedModel");
            }

            // Set the editor model
            await _diffEditor.SetModel(new DiffEditorModel
            {
                Original = original_model,
                Modified = modified_model
            });
        }

        static System.Text.Encoding EucKrEncoding = System.Text.Encoding.GetEncoding("euc-kr");

        int CountKR { get; set; }
        int Count { get; set; }




        private async Task EditorOnKeyUpOriginal(KeyboardEvent keyboardEvent)
        {
            var m = await _diffEditor.OriginalEditor.GetValue();

            Count = System.Text.Encoding.UTF8.GetByteCount(m);
            CountKR = EucKrEncoding.GetByteCount(m);
            _valueToSetModified = EucKrEncoding.GetString(
                System.Text.Encoding.Convert(
                    System.Text.Encoding.UTF8,
                    EucKrEncoding,
                    System.Text.Encoding.UTF8.GetBytes(m)));

            await _diffEditor.ModifiedEditor.SetValue(_valueToSetModified);
        }

        private void EditorOnKeyUpModified(KeyboardEvent keyboardEvent)
        {
        }
    }
}
