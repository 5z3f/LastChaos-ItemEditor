/*
    Original Class Author: 
    Copyright (c) 2013, Jorge Monasterio
    All rights reserved.

    Github Repository:
    https://github.com/jmonasterio/AceWinforms/
*/

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ItemEditor.Controls
{
    public partial class CodeEditor : WebBrowser
    {
        public string bIEVersion, HighlighterMode, Theme;
        private bool isLoaded = false;

        public CodeEditor()
        {
            SetDefaults();
            InitializeComponent();
            DocumentCompleted += CodeEditor_DocumentCompleted;
        }

        void CodeEditor_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            isLoaded = true;
            EditorText = _textBeforeHtmlLoaded;
        }

        private void SetDefaults()
        {
            /*
                SYNTAX: https://github.com/ajaxorg/ace/tree/master/lib/ace/mode
                THEME:  https://github.com/ajaxorg/ace/tree/master/lib/ace/theme
            */

            HighlighterMode = "lua";
            Theme = "xcode"; 
            IEVersion = "10";
        }

        public string IEVersion
        {
            get { return bIEVersion; }
            set { if (value != "9" && value != "10" && value != "11") throw new Exception("IE 9 is required."); bIEVersion = value; } 
        }

        public void Load()
        {
            isLoaded = false;
            var template = GenerateEditorHtmlFromProps();
            DocumentText = template;
        }

        private string _textBeforeHtmlLoaded;

        public string Text
        {
            get { if (isLoaded) return EditorText; else return _textBeforeHtmlLoaded; }
            set { _textBeforeHtmlLoaded = value; if (isLoaded) EditorText = value; }
        }

        private string EditorText
        {
            set { this.Document.InvokeScript("setAceEditorText", new object[] {value} ); }
            get { return this.Document.InvokeScript("getAceEditorText") as string; }
        }

        private string GenerateEditorHtmlFromProps()
        {
            var template = ReadEmbeddedHtmlEditorTemplate();
            template = ReplaceTemplateField(template, "{{highlighter}}", HighlighterMode);
            template = ReplaceTemplateField(template, "{{theme}}", Theme);
            template = ReplaceTemplateField(template, "{{minIeVersion}}", IEVersion);
            return template;
        }

        private string ReplaceTemplateField(string template, string field, string value)
        {
            if (!string.IsNullOrEmpty(value))
                template = template.Replace(field, value);

            return template;
        }

        private string ReadEmbeddedHtmlEditorTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var xd = GetType().Assembly.GetManifestResourceNames();
            using (var imageStream = assembly.GetManifestResourceStream("ItemEditor.Controls.HTML.index.html"))
            {
                using (var textStreamReader = new StreamReader(imageStream))
                    return textStreamReader.ReadToEnd();
            }
        }


    }
}
