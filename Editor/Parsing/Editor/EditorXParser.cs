using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

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

        private Dictionary<string, System.Type> _elementTypes;

        public void Initialize()
        {
            _elementTypes = new Dictionary<string, System.Type>();
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i += 1)
            {
                System.Type[] types = assemblies[i].GetTypes();

                for (int j = 0; j < types.Length; j += 1)
                {
                    if (types[j].IsSubclassOf(typeof(Element)))
                    {
                        _elementTypes.Add(types[j].Name.ToLower(), types[j]);
                    }
                }
            }
        }

        private Stack<Tag> _tagStack;
        private string _data;
        private int _index;

        public EditorXParser(Window window)
        {
            _windowTarget = window;
        }

        protected Tag[] Parse(string data)
        {
            _tagStack = new Stack<Tag>();
            _index = 0;
            _data = data;
            List<Tag> tagList = new List<Tag>();
            bool isTagOpen = false;
            while (!isEOF())
            {
                CullWhiteSpace();
                if (Peek() == '<')
                {
                    if (LookAhead("</"))
                    {
                        EndTag();
                        Tag last = _tagStack.Pop();
                        if (_tagStack.Count < 1)
                        {
                            tagList.Add(last);
                        }
                        isTagOpen = false;
                        CullWhiteSpace();
                    }
                    else
                    {
                        bool isClosed = false;
                        Tag next = ParseTag(ref isClosed);
                        if (_tagStack.Count > 0) _tagStack.Peek().AddChild(next);
                        else
                        {
                            if (isClosed) tagList.Add(next);
                        }
                        if (!isClosed)
                        {
                            _tagStack.Push(next);
                            isTagOpen = true;
                        }               
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

        private bool isEOF()
        {
            return _index >= _data.Length;
        }

        private char Step()
        {
            char output = _data[_index];
            _index++;
            return output;
        }

        private char Peek()
        {
            return _data[_index];
        }

        private bool LookAhead(string pattern)
        {
            int index = _index;
            for (int i = 0; i < pattern.Length; i += 1)
            {
                if (isEOF()) return false;
                if (_data[_index + i] != pattern[i]) return false;
            }
            return true;
        }

        private bool Require(char c)
        {
            if (_data[_index] != c) return false;

            Step();
            return true;
        }

        protected string ParseIdentifier(string allowedChars = "-_")
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            while (char.IsLetterOrDigit(Peek()) || allowedChars.IndexOf(Peek()) >= 0)
            {
                builder.Append(Step());
            }
            return builder.ToString();
        }

        protected void CullWhiteSpace()
        {
            while (!isEOF() && char.IsWhiteSpace(Peek()))
            {
                Step();
            }
        }

        private Window _windowTarget;
        private UnityEngine.Object _callbackTarget;

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

        protected Tag ParseTag(ref bool isClosed)
        {
            Require('<');
            CullWhiteSpace();
            string tagType = ParseIdentifier();
            Tag tag = new Tag() { type = tagType };
            CullWhiteSpace();
            while (char.IsLetter(Peek()))
            {
                tag.attributes.Add(ParseKVP());
                CullWhiteSpace();
            }
            CullWhiteSpace();
            if (Peek() == '/')
            {
                isClosed = true;
                Step();
            } else
            {
                isClosed = false;
            }
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
            for (int i = 0; i < tag.children.Count; i += 1)
            {
                root.AddChild(CreateTree(tag.children[i]));
            }
            return root;
        }

        protected void ApplyAttributes(List<Tag.Attribute> attributes, Element elem)
        {
            for (int i = 0; i < attributes.Count; i += 1)
            {
                ApplyAttribute(attributes[i], elem);
            }
        }

        protected void ApplyAttribute(Tag.Attribute attribute, Element elem)
        {
            string name = attribute.name.ToLower();
            switch (name)
            {
                case "name":
                    elem.name = attribute.data;
                    break;
                case "font-style":
                case "alignment":
                case "position":
                    elem.style[name] = attribute.data;
                    break;

                case "width":
                case "height":
                case "top":
                case "bottom":
                case "right":
                case "left":
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

                case "padding":
                case "margin":
                    elem.style[name] = attribute.data;
                    break;
                case "background":
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(attribute.data);
                    if (texture == null)
                    {
                        Debug.LogWarning("There was no Texture found at path: " + attribute.data);
                    }
                    else
                    {
                        elem.style[attribute.name] = texture;
                    }
                    break;
                case "font":
                    Font font = AssetDatabase.LoadAssetAtPath<Font>(attribute.data);
                    if (font == null)
                    {
                        Debug.LogWarning("There was no Font found at path: " + attribute.data);
                    }
                    else
                    {
                        elem.style[attribute.name] = font;
                    }
                    break;
                case "load":
                case "click":
                case "change":
                case "mousedown":
                case "mouseup":
                case "mouseenter":
                case "mouseleave":
                case "mousedrag":
                case "mousemove":
                case "keyup":
                case "keydown":
                    EventCallback callback = CreateEventCallback(attribute.data);
                    if (callback != null) elem.AddEventListener(name, callback);
                    break;

                default:
                    if (!elem.SetProperty(attribute.name, attribute.data))
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
            MethodInfo method = _callbackTarget.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            if (method == null)
            {
                Debug.LogWarning("Method " + name + " was not found on " + _callbackTarget.GetType().Name);
                return null;
            }
            if (method.IsStatic)
            {
                return (EventCallback)System.Delegate.CreateDelegate(typeof(EventCallback), method);
            }
            else
            {
                return (EventCallback)System.Delegate.CreateDelegate(typeof(EventCallback), _callbackTarget, method);
            }
        }

        public T LoadUnityObject<T>(string path) where T : UnityEngine.Object
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
            int i = 0;
            if (tags.Length > 0)
            {
                if (tags[0].type == "head")
                {
                    ParseHead(tags[0]);
                    i += 1;
                }
            }
            for (; i < tags.Length; i += 1)
            {
                elements.Add(CreateTree(tags[i]));
            }
            return elements.ToArray();
        }

        protected void ParseHead(Tag tag)
        {
            for (int i = 0; i < tag.attributes.Count; i += 1)
            {
                Tag.Attribute attribute = tag.attributes[i];
                switch (attribute.name)
                {
                    case "skin":
                        GUISkin skin = AssetDatabase.LoadAssetAtPath<GUISkin>(attribute.data);
                        if(skin == null)
                        {
                            Debug.LogWarning("There was no GUISkin found at path: " + attribute.data);
                        } else
                        {
                            _windowTarget.skin = skin;
                        }
                        
                        break;
                    case "repaint-on-scene-change":
                    case "auto-repaint":
                    case "autorepaint":
                    case "auto-repaint-on-scene-change":
                        _windowTarget.autoRepaintOnSceneChange = bool.Parse(attribute.data);
                        break;
                    case "wantsmousemove":
                    case "wants-mouse-move":
                        _windowTarget.wantsMouseMove = bool.Parse(attribute.data);
                        break;
                    case "background-color":
                        _windowTarget.body.style["background-color"] = ColorUtility.ReadColor(attribute.data);
                        Debug.Log("bg detected" + _windowTarget.body.style[attribute.name]);
                        break;
                    case "background":
                        Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(attribute.data);
                        if (texture == null)
                        {
                            Debug.LogWarning("There was no Texture found at path: " + attribute.data);
                        }
                        else
                        {
                            _windowTarget.body.style[attribute.name] = texture;
                        }
                        break;
                    case "font":
                        Font font = AssetDatabase.LoadAssetAtPath<Font>(attribute.data);
                        if (font == null)
                        {
                            Debug.LogWarning("There was no Font found at path: " + attribute.data);
                        }
                        else
                        {
                            _windowTarget.body.style[attribute.name] = font;
                        }

                        break;
                    default:
                        break;
                }

            }
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
            }
            else
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
            string identifier = ParseIdentifier("-_.");
            CullWhiteSpace();
            Require('=');
            CullWhiteSpace();
            string data = ParseString();
            return new Tag.Attribute() { name = identifier, data = data };
        }
    }
}