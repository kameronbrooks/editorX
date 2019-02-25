using EditorX;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DemoWindow : EditorX.Window
{
    private Element floatField;
    private Img _img;
    private ImageButton _imageButton;
    private Texture _texture;

    [System.Serializable]
    public class MaterialSettings
    {

        [SerializeField]
        string _name;
        [SerializeField]
        int _size;
        [SerializeField]
        Material _mat;
        [SerializeField]
        Texture _tex;

        public MaterialSettings(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return "MaterialSettings: " + _name;
        }

        public void OpenWindow()
        {
            EditorUtility.SaveFilePanel("Title", "", "", "");
        }
    }

    [SerializeField]
    List<MaterialSettings> _matList;

    [MenuItem("Tools/Test")]
    public static void Main()
    {
        DemoWindow window = EditorWindow.GetWindow<DemoWindow>();
        window.Open();
    }

    protected override void OnOpen()
    {
        Debug.Log("Starting");
    }

    protected override void OnClose()
    {
    }

    protected override void PreGUI()
    {
        base.PreGUI();
        if (GUILayout.Button("Refresh"))
        {
            Unload();
            Repaint();
        }
    }

    protected override void PostGUI()
    {
        base.PostGUI();
    }

    public override void OnLoadWindow()
    {

        _matList = new List<MaterialSettings>();
        _matList.Add(new MaterialSettings("1"));
        _matList.Add(new MaterialSettings("2"));
        _matList.Add(new MaterialSettings("3"));
        LoadFromFile("EditorX/Demo/Editor/New Window.exml");

        GetElementByID("matlist").SetProperty("value", _matList);
    }

    private void Change_Test(Element elem, Event evnt)
    {
        ValueElement valElem = (ValueElement)elem;
        Debug.Log("My val is " + valElem.GetValue());
    }

    private void Callback(Element elem, Event evnt)
    {
        ValueElement valElem = (ValueElement)elem;
        ((Img)_img).texture = valElem.GetValue<Texture>();

        
    }

    int iter = 0;
    protected override void EditorUpdate()
    {
        if(iter < 10)
        {
            iter++;
            return;
        }

        iter = 0;
    }

    private void MouseMove_Callback(Element elem, Event evnt)
    {
        
    }

    private void MouseClick(Element elem, Event evnt)
    {
        Debug.Log(elem.name + " was clicked");
    }

    protected override void OnSerialize()
    {
    
    }

    protected override void OnDeserialize()
    {
        

    }

    protected override void OnAssemblyReload()
    {
        if(_matList == null)
        {
            _matList = new List<MaterialSettings>();
            _matList.Add(new MaterialSettings("1"));
            _matList.Add(new MaterialSettings("2"));
            _matList.Add(new MaterialSettings("3"));
        }
        
        GetElementByID("matlist").SetProperty("value", _matList);
    }
}