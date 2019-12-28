using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Viking.Rich
{
    /// <summary>
    /// Simple editor (with a few bugs) for manipulating rich text.
    /// </summary>
    public class VikingRichEditor : EditorWindow
    {
        private static VikingRichEditor window;

        /// <summary>
        /// Editor for the current text.
        /// </summary>
        private TextEditor editor;

        /// <summary>
        /// Current text for editing.
        /// </summary>
        private string text = "";

        /// <summary>
        /// Target size.
        /// </summary>
        private int size = 9;

        /// <summary>
        /// Target color.
        /// </summary>
        private Color color = Color.black;

        /// <summary>
        /// Rich text codes; Name, Prefix, Suffix.
        /// </summary>
        private Dictionary<string, RichCode> codes = new Dictionary<string, RichCode>()
        {
            { "bold", new RichCode("<b>", "</b>") },
            { "italic", new RichCode("<i>", "</i>") },
            { "size", new RichCode("<size=x>", "</size>") },
            { "color", new RichCode("<color=#x>", "</color>") },

            // add additional codes here
        };

        /// <summary>
        /// Style for previewing the rich text.
        /// </summary>
        private static GUIStyle preview;

        /// <summary>
        /// Initialize Rich Editor window.
        /// </summary>
        [MenuItem("Viking/Rich")]
        private static void Init()
        {
            preview = new GUIStyle(EditorStyles.helpBox);
            preview.richText = true;

            window = GetWindow<VikingRichEditor>(true, "Viking Rich Editor");

            window.Show();
        }

        /// <summary>
        /// Render window.
        /// </summary>
        private void OnGUI()
        {
            Toolbar();

            int half = (int)window.position.height / 2;

            text = GUI.TextArea(new Rect(2, 20, window.position.width - 4, half - 22), text, EditorStyles.textArea);

            GUILayout.BeginArea(new Rect(2, half, window.position.width - 4, half - 2));
            EditorGUILayout.LabelField(text, preview, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndArea();

            editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
        }

        /// <summary>
        /// Apply desired rich text.
        /// </summary>
        /// <param name="code">Name of the rich text code.</param>
        /// <param name="value">Optional value. ie. Color Hex [ff0000ff]</param>
        private void Rich(RichCode code, string value = "")
        {
            string selected = editor.SelectedText;

            string a = code.prefix;
            string b = code.suffix;

            // set if there is an additional value
            if (value != "")
            {
                a = a.Replace("x", value);
            }

            // remove prefix from selected text if it contains one 
            if (selected.Contains(a))
            {
                selected = selected.Replace(a, "");
            }

            // remove suffix from selected text if it contains one 
            if (selected.Contains(b))
            {
                selected = selected.Replace(b, "");
            }

            int index = text.IndexOf(editor.SelectedText);

            string before = text.Substring(0, index);
            string after = text.Substring(index);

            // add prefix if there isn't one before the selection
            if (!before.Contains(a))// || before.Contains(b))
            {
                selected = selected.Insert(0, a);
            }

            // add suffix if there isn't one after the selection
            if (!after.Contains(b))// || after.Contains(a))
            {
                selected = selected.Insert(selected.Length, b);
            }

            text = text.Replace(editor.SelectedText, selected);
        }

        /// <summary>
        /// Toolbar for manipulating rich text.
        /// </summary>
        private void Toolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));

            // bold
            if (GUILayout.Button("Bold", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
            {
                Rich(codes["bold"]);
            }

            // italic
            if (GUILayout.Button("Italic", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
            {
                Rich(codes["italic"]);
            }

            EditorGUILayout.Separator();

            // size
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Size", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
            {
                Rich(codes["size"], size.ToString());
            }
            size = EditorGUILayout.IntField(size, GUILayout.Width(48));
            size = Mathf.Clamp(size, 2, 128);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            // color
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Color", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
            {
                Rich(codes["color"], ColorUtility.ToHtmlStringRGBA(color));
            }
            color = EditorGUILayout.ColorField(color, GUILayout.Width(64));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            GUILayout.FlexibleSpace();

            // clipboard
            if (GUILayout.Button("Clipboard", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
            {
                CopyToClipboard();
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Copy the text to the clipboard.
        /// </summary>
        private void CopyToClipboard()
        {
            TextEditor textEditor = new TextEditor();

            textEditor.text = text;

            textEditor.SelectAll();

            textEditor.Copy();

            Debug.Log("Coppied to clipboard!");
        }
    }
}
