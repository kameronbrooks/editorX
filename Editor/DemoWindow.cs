using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorX;
using System;

public class DemoWindow : EditorX.Window {

    Element floatField;
    Img _img;
    ImageButton _imageButton;
    Texture _texture;

    [MenuItem("Tools/Test")]
    public static void Main()
    {      
        DemoWindow window = EditorWindow.GetWindow<DemoWindow>();     
    }


    protected override void OnOpen()
    {
        Debug.Log("Starting");
        this.reloadOnAssemblyReload = true;
    }

    protected override void OnClose()
    {
        
    }

    protected override void PreGUI()
    {
        base.PreGUI();
        if(GUILayout.Button("Refresh"))
        {
            Unload();
            Repaint();
        }
    }

    public override void OnLoadWindow()
    {
        
        string src = @"
<div layout-type='vertical'>
    <intfield change='Change_Test'></intfield>
    <div>
        <intfield></intfield>
        <floatfield></floatfield>
    </div>
    <img width='300' height='300' texture='Assets/EditorX/Demo/ccl.jpg'></img>
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
