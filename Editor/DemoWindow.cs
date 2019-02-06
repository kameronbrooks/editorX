using EditorX;
using UnityEditor;
using UnityEngine;

public class DemoWindow : EditorX.Window
{
    private Element floatField;
    private Img _img;
    private ImageButton _imageButton;
    private Texture _texture;

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


        LoadFromFile("EditorX/Demo/Editor/New Window.exml");
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
}