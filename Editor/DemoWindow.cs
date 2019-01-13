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
        Element div = Element.Create<Div>("main");
        div.style["background-color"] = new Color(0, 0, 0, 0.2f);
        div.style["padding"] = new RectOffset(40, 40, 40, 40);

        div.AddChild(floatField);
        ValueElement color = (ValueElement)div.AddChild(NewElement<ColorField>("colors"));
        color.SetLabel("Color", "tool tip");
        _img = NewElement<Img>("image");
        _img.texture = Texture2D.whiteTexture;

        _img.style["width"] = 300.0f;
        _img.style["height"] = 300.0f;

        color.style["width"] = 300;
        div.AddChild(_img);

        floatField.AddEventListener("change", Callback);

        Element node = body.AddText("This is some text");
        node.style["color"] = Color.red;
        //node.style["font-size"] = 40;

        body.AddChild(div);

    }

    private void Callback(Element elem, Event evnt)
    {
        ValueElement valElem = (ValueElement)elem;
        ((Img)_img).texture = valElem.GetValue<Texture>();
    }
}
