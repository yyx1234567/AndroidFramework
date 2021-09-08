using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using UnityEditor.Animations;
using System.Linq;
[CreateAssetMenu(menuName = "DataManager/Editor/ModelAniEditor")]

public class ModelAniEditor : Sirenix.OdinInspector.SerializedScriptableObject
{
    [Sirenix.OdinInspector.LabelText("工具名称")]
    [Sirenix.OdinInspector.FilePath]
    public string ToolName;

    [Sirenix.OdinInspector.LabelText("导入模型路径")]
    [Sirenix.OdinInspector.FilePath]
    public string ModelLoadPath;

    [Sirenix.OdinInspector.LabelText("保存预制件路径")]
    [Sirenix.OdinInspector.FolderPath]
    public string SavePath;

    [Sirenix.OdinInspector.LabelText("预制件名称")]
    public string PrefabName;

    [HideInInspector]
    public ModelImporter assetImporter;

    public Dictionary<string, Vector2> AnimationClips = new Dictionary<string, Vector2>();

    [Sirenix.OdinInspector.Button("生成", Sirenix.OdinInspector.ButtonSizes.Large)]
    public void Process()
    {
        if (!File.Exists(ModelLoadPath))
        {
            EditorUtility.DisplayDialog("提示", "模型读取目录不存在！", "确定");
        }

        assetImporter = AssetImporter.GetAtPath(ModelLoadPath) as ModelImporter;
        if (assetImporter == null) return;
        if (string.IsNullOrEmpty(PrefabName))
        {
            assetImporter.name = ModelLoadPath.Split('/').Last().Split('.').First();
        }
        else
        {
            assetImporter.name = PrefabName;
         }
        CreateAnimationClip();

        var prefab = CreatePrefab();

        CreateAnimatior(prefab);

        EditorUtility.DisplayDialog("提示", "创建成功！", "确定");
     }

    private void CreateAnimatior(GameObject prefab)
    {
        var animatorController = AnimatorController.CreateAnimatorControllerAtPath($"{SavePath}/{assetImporter.name}/{PrefabName}_AniController.controller");
        prefab.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        AnimatorControllerLayer layer = animatorController.layers[0];
        AnimatorStateMachine stateMachine = layer.stateMachine;

        var objs = AssetDatabase.LoadAllAssetsAtPath(ModelLoadPath);

        var initstate = stateMachine.AddState("Idle");
        initstate.motion = null;

        foreach (UnityEngine.Object o in objs)
        {
            if (o is AnimationClip&&o.name.StartsWith("__preview"))
            {
                if (AnimationClips.Count != 0 && AnimationClips.ContainsKey(o.name))
                {
                    AnimationClip clip = o as AnimationClip;
                    AnimatorState state = stateMachine.AddState(clip.name);
                    state.motion = clip;
                }
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void CreateAnimationClip()
    {
        var clips = new List<ModelImporterClipAnimation>();
        foreach (var item in AnimationClips)
        {
            var clip = new ModelImporterClipAnimation();
            clip.name = item.Key;
            clip.firstFrame = item.Value.x;
            clip.lastFrame = item.Value.y;
            clips.Add(clip);
        }
        assetImporter.clipAnimations = clips.ToArray();
        assetImporter.SaveAndReimport();
    }

    public GameObject CreatePrefab()
    {
        if (!Directory.Exists(SavePath + "/" + assetImporter.name))
        {
            Directory.CreateDirectory(SavePath + "/" + assetImporter.name);
        }
        var go = AssetDatabase.LoadAssetAtPath<GameObject>(ModelLoadPath);
        var instance = GameObject.Instantiate(go);
        var result = PrefabUtility.SaveAsPrefabAsset(instance, $"{SavePath}/{ assetImporter.name}/{assetImporter.name}.prefab");
        DestroyImmediate(instance);
        AssetDatabase.Refresh();
        return result;
    }

}


