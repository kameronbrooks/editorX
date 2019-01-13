using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class TextNode : Element
    {
        [SerializeField]
        protected string _text;

        public static TextNode Create(string text)
        {
            TextNode node = ScriptableObject.CreateInstance<TextNode>();
            node._text = text;

            return node;
        }

        public string text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        public override string tag
        {
            get
            {
                return "span";
            }
        }


        protected override void OnGUI()
        {         
            EditorGUI.LabelField(_rect, _text, style.guistyle);
        }

    }
}
