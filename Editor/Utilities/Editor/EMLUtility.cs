using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace EditorX
{
    public static class EMLUtility
    {
        [MenuItem("NewEMLFile", menuItem = "Assets/Create/EXML File")]
        public static void NewEMLFile()
        {
            Object ob = Selection.activeObject;
            if (ob == null)
            {
                Debug.LogError("No Folder Selected");
                return;
            }

            string path = AssetDatabase.GetAssetPath(ob);
            string fullDirPath = Application.dataPath + path.Substring("Assets".Length);

            string fileDirPath = fullDirPath + "/New Window";

            string postFix = "";
            int iter = 0;

            
            while (File.Exists(fileDirPath + postFix + ".exml"))
            {
                iter += 1;
                postFix = iter.ToString();
            }
            File.WriteAllText(fileDirPath + postFix + ".exml", "<head />");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}