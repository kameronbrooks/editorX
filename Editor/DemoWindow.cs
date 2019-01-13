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

    public void OnEnable()
    {

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
        if(GUILayout.Button("Refresh"))
        {
            Unload();
            Repaint();
        }
    }

    public override void OnLoadWindow()
    {
        floatField = ObjectField.Create("object", typeof(Texture2D));
        //
        body.AddChild(floatField);
        body.AddChild(NewElement<ColorField>("colors"));
        _img = NewElement<Img>("image");
        _img.texture = Texture2D.whiteTexture;
        _img.style["width"] = 300.0f;
        _img.style["height"] = 300.0f;

        body.GetChildById("colors").style["width"] = 300;
        body.AddChild(_img);

        floatField.AddEventListener("change", Callback);

    }

    private void Callback(Element elem, Event evnt)
    {
        ValueElement valElem = (ValueElement)elem;
        ((Img)_img).texture = valElem.GetValue<Texture>();
    }
}
