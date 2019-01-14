﻿using System.Collections;
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
<div>
    <intfield change='Change_Test'></intfield>
    <div layout-type='vertical' color='#FFFFFF'>
        This is some text.
        <intfield name='int1'></intfield>
        <floatfield name='float1' hidden='true'></floatfield>
        This is some text.
    </div>
    <img width='255' height='255' texture='Assets/EditorX/Demo/ccl.jpg'></img>
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
