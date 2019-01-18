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
        this.reloadOnAssemblyReload = true;
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
        string src = @"
<head skin='Assets/EditorX/Demo/Editor/DemoSkin.guiskin'>
</head>
<div>
    <div layout-type='vertical'>
        <intfield change='Change_Test'></intfield>
        <toggle label='toggle 1'></toggle>
        <enumpopup type='UnityEngine.BatteryStatus' value='Charging' ></enumpopup>
    </div>
    
    <div layout-type='vertical' color='red' background-color='#00000055' padding='10 10 10 10'>
        This is some text.
        <intfield name='int1' margin='10 10 10 10'></intfield>
        <floatfield name='float1' hidden='true'></floatfield>
        This is some text.
        <colorfield value='blue'></colorfield>
    </div>
    <scrollview>
        <img width='255' height='255' texture='Assets/EditorX/Demo/ccl.jpg'></img>
    </scrollview>
    
</div>
";
        LoadFromMarkup(src);
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
}