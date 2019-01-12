using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorX;

public class DemoWindow : EditorX.Window {

    TextBox _textbox;  
    IElement _div;
    IElement _button;
    IElement _intField;
    Img _img;
    ImageButton _imageButton;
    Texture _texture;

    

    private void Obfield_onChange(IElement elem, Event e)
    {
        IValueElement valueElem = elem as IValueElement;
        _imageButton.img = valueElem.GetValue<Texture>();
        _img.texture = valueElem.GetValue<Texture>();
    }

    private void Button_onMouseEvent(IElement elem, Event e)
    {
        Debug.Log("Haha");
        
    }

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

    public override void OnLoadWindow()
    {
        _intField = new IntField("int", new Style(
            "width", 100
            ));

        body.AddChild(_intField);

    }

}
