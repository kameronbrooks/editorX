using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorX
{
    public abstract class StandardWindow : Window
    {
        protected abstract string EXMLFilePath { get; }

        public override void OnLoadWindow()
        {
            LoadFromFile(EXMLFilePath);
        }
    }
}