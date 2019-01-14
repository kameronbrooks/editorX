using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EditorX
{
    public class EditorXParser
    {
        public class Tag
        {
            public class Attribute
            {
                public string name;
                public string data;
            }
            public string type;
            public List<Attribute> attributes;

            public Tag parent;
            public List<Tag> children;

            public Tag()
            {
                attributes = new List<Attribute>();
                children = new List<Tag>();
            }

            public void AddChild(Tag tag)
            {
                children.Add(tag);
            }
            public void AddAttribute(string name, string data)
            {
                attributes.Add(new Attribute() { name = name, data = data });
            }
        }
        Dictionary<string, System.Type> _elementTypes;

        public void Initialize()
        {
            _elementTypes = new Dictionary<string, System.Type>();
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for(int i = 0; i < assemblies.Length; i += 1)
            {
                System.Type[] types = assemblies[i].GetTypes();

                for(int j = 0; j < types.Length; j += 1)
                {
                    if(types[j].IsSubclassOf(typeof(Element)))
                    {

                        _elementTypes.Add(types[j].Name.ToLower(), types[j]);
                    }
                }
            }
        }

        Stack<Tag> _tagStack;
        string _data;
        int _index;

        protected Tag[] Parse(string data)
        {
            _tagStack = new Stack<Tag>();
            _index = 0;
            _data = data;
            List<Tag> tagList = new List<Tag>();
            bool isTagOpen = false;
            while(!isEOF())
            {
                CullWhiteSpace();
                if (Peek() == '<')
                {
                    if(LookAhead("</"))
                    {
                        EndTag();
                        Tag last = _tagStack.Pop();
                        if(_tagStack.Count < 1)
                        {
                            tagList.Add(last);
                        }
                        isTagOpen = false;
                        CullWhiteSpace();
                    }
                    else
                    {
                        Tag next = ParseTag();
                        if (_tagStack.Count > 0) _tagStack.Peek().AddChild(next);
                        _tagStack.Push(next);
                        isTagOpen = true;
                        CullWhiteSpace();
                    }
                }
                else
                {
                    Tag textNode = ParseMiscText();
                    if (_tagStack.Count > 0)
                    {
                        _tagStack.Peek().AddChild(textNode);
                    }
                    else
                    {
                        tagList.Add(textNode);
                    }
                    
                }
            }
            return tagList.ToArray();
        }

        bool isEOF()
        {
            return _index >= _data.Length;
        }
        char Step()
        {
            char output = _data[_index];
            _index++;
            return output;
        }
        char Peek()
        {
            return _data[_index];
        }
        bool LookAhead(string pattern)
        {
            int index = _index;
            for(int i = 0; i < pattern.Length; i += 1)
            {
                if (isEOF()) return false;
                if (_data[_index + i] != pattern[i]) return false;
            }
            return true;
        }
        bool Require(char c)
        {
            if (_data[_index] != c) return false;

            Step();
            return true;
        }
        protected string ParseIdentifier()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            while(char.IsLetterOrDigit(Peek()) || Peek() == '-' || Peek() == '_')
            {
                builder.Append(Step());
            }
            return builder.ToString();
        }
        protected void CullWhiteSpace()
        {
            while(!isEOF() && char.IsWhiteSpace(Peek()))
            {
                Step();
            }
        }

        UnityEngine.Object _callbackTarget;

        public Object callbackTarget
        {
            get
            {
                return _callbackTarget;
            }

            set
            {
                _callbackTarget = value;
            }
        }

        protected Tag ParseTag()
        {
            Require('<');
            CullWhiteSpace();
            string tagType = ParseIdentifier();
            Tag tag = new Tag() { type = tagType };
            CullWhiteSpace();
            while(char.IsLetter(Peek()))
            {
                tag.attributes.Add(ParseKVP());
                CullWhiteSpace();
            }
            CullWhiteSpace();
            Require('>');
            return tag;
        }

        protected Tag ParseMiscText()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            while (Peek() != '<')
            {
                builder.Append(Step());
            }
            string res = builder.ToString();
            Regex stripWhiteSpace = new Regex(@"\s+");
            res = stripWhiteSpace.Replace(res, " ");
            Tag tag = new Tag() { type = "textnode" };
            tag.AddAttribute("innerText", res);
            return tag;
            
        }

        protected Element CreateTree(Tag tag)
        {
            Element root = Element.Create(_elementTypes[tag.type]);
            ApplyAttributes(tag.attributes, root);
            for(int i = 0; i < tag.children.Count; i += 1)
            {
                root.AddChild(CreateTree(tag.children[i]));
            }
            return root;
        }
        protected void ApplyAttributes(List<Tag.Attribute> attributes, Element elem)
        {
            Debug.Log("Applying Attributes" + attributes.Count);
            for(int i = 0; i < attributes.Count; i += 1)
            {
                ApplyAttribute(attributes[i], elem);
            }
        }
        protected void ApplyAttribute(Tag.Attribute attribute, Element elem)
        {
            string name = attribute.name.ToLower();
            switch(name)
            {
                case "name":
                    elem.name = attribute.data;
                    break;
                case "width":
                case "height":
                case "min-width":
                case "min-height":
                case "max-width":
                case "max-height":
                    elem.style[name] = System.Single.Parse(attribute.data);
                    break;
                case "expand-width":
                case "expand-height":
                    elem.style[name] = (attribute.data.ToLower() == "false") ? false : true;
                    break;
                case "background-color":
                case "color":
                    elem.style[name] = ColorUtility.ReadColor(attribute.data);
                    break;
                case "click":
                case "change":
                case "keyup":
                case "keydown":
                    EventCallback callback = CreateEventCallback(attribute.data);
                    Debug.Log(attribute.data + " " + attribute.name);
                    if(callback != null) elem.AddEventListener(name, callback);
                    break;
                default:
                    if(!elem.SetProperty(attribute.name, attribute.data))
                    {
                        Debug.LogWarning("Cannot apply attribute " + name + " to tag of type " + elem.GetType().Name);
                    }
                        
                    break;

            }
        }

        public EventCallback CreateEventCallback(string name)
        {
            if (_callbackTarget == null)
            {
                Debug.LogError("There is no callback target specified to EditorX Parser");
                return null;
            }
            MethodInfo method = _callbackTarget.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static );

            if (method == null)
            {
                Debug.LogWarning("Method " + name + " was not found on " + _callbackTarget.GetType().Name);
                return null;
            }
            if(method.IsStatic)
            {
                return (EventCallback)System.Delegate.CreateDelegate(typeof(EventCallback), method);
            }
            else
            {
                return (EventCallback)System.Delegate.CreateDelegate(typeof(EventCallback), _callbackTarget,  method);
            }
        }

        public T LoadUnityObject<T>(string path) where T:UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        
        public Element[] BuildUI(string markup)
        {
            Tag[] tags = null;
            List<Element> elements = new List<Element>();
            try
            {
                tags = Parse(markup);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
                return new Element[0];
            }

            for(int i = 0; i < tags.Length; i += 1)
            {
                elements.Add(CreateTree(tags[i]));
            }
            return elements.ToArray();
        }
        protected void EndTag()
        {
            Require('<');
            Require('/');
            CullWhiteSpace();
            string tagType = ParseIdentifier();
            CullWhiteSpace();
            Require('>');
        }
        protected string ParseString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            char literalChar;
            if (Peek() == '"' || Peek() == '\'')
            {
                literalChar = Step();
            } else
            {
                throw new System.Exception("This is not a literal");
            }

            while (Peek() != literalChar)
            {
                builder.Append(Step());
            }

            Require(literalChar);

            return builder.ToString();
        }
        protected Tag.Attribute ParseKVP()
        {
            string identifier = ParseIdentifier();
            CullWhiteSpace();
            Require('=');
            CullWhiteSpace();
            string data = ParseString();
            return new Tag.Attribute() { name = identifier, data = data };

        }
        
    }
}
