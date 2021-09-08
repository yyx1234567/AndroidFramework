using ETModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ObjectExtension
{
#if UNITY_EDITOR
    public static TextAsset GetScirptFormDirectory(this object self, string path)
    {
        if (Directory.Exists(path))
        {
            foreach (var item in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
            {
                var target = item.Split('\\').LastOrDefault();

                if (target.Replace(".cs", "") == self.GetType().Name)
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(item);
                }
            }
        }
        return null;
    }
#endif
}
