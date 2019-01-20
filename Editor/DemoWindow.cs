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
        string src = @"
<head 
    skin='Assets/EditorX/Demo/Editor/DemoSkin.guiskin' 
    wants-mouse-move='true'
    background-color='#313131'

></head>

<div color='#eeeeee'>
    <fadegroup background-color='#777777' label='show'>
        <div layout-type='vertical' background-color='#777777' padding='5 5 5 5' >
            <intfield change='Change_Test'/>
            <toggle label='toggle 1'/>
            <enumpopup type='UnityEngine.BatteryStatus' value='Charging' />
            <curvefield />
        </div>
    </fadegroup>
    
    <div layout-type='vertical' color='red' background-color='#00000055' padding='10 10 10 10'>
        This is some text.
        <intfield name='int1' margin='10 10 10 10' />
        <floatfield name='float1' hidden='true' />
        This is some text.
        <colorfield value='blue' />
    </div>
    <scrollview>
        <img width='255' height='255' texture='Assets/EditorX/Demo/ccl.jpg' mousedown='MouseClick' mousemove='MouseMove_Callback' />
    </scrollview>
    

</div>
<hr color='#EEEEEE66' />
<div>
    <scrollview>
        <objecteditor type='UnityEngine.Texture' />
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

    int iter = 0;
    protected override void EditorUpdate()
    {
        if(iter < 10)
        {
            iter++;
            return;
        }
        Debug.Log("Updating");
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