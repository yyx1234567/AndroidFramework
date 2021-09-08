using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ETModel
{
    public static class ResourcesHelper
    {
        public static UnityEngine.Object Load(string path)
        {
            return Resources.Load(path);
        }

        public static T Load<T>(string bundleName, string prefabName) where T : UnityEngine.Object
        {
            var resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>() ?? Game.Scene.AddComponent<ResourcesComponent>();

            resourcesComponent.LoadBundle(bundleName);

            var result = resourcesComponent.GetAsset<T>(bundleName, prefabName);

            //resourcesComponent.UnloadBundle(assetbundleInfo.BundleName);
             
            return result;
        }

        public static async ETTask<T> LoadAsync<T>(string bundleName, string prefabName) where T : UnityEngine.Object
        {
            var resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>() ?? Game.Scene.AddComponent<ResourcesComponent>();
  
            await resourcesComponent.LoadBundleAsync(bundleName);

            var result = resourcesComponent.GetAsset<T>(bundleName, prefabName);

            //resourcesComponent.UnloadBundle(assetbundleInfo.BundleName);

            return result;
        }

        public static async ETTask LoadScene(string bundle,string scene)
        {
            var resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>() ?? Game.Scene.AddComponent<ResourcesComponent>();

           await resourcesComponent.LoadBundleAsync(bundle);

            await new AsyncOperationTask(SceneManager.LoadSceneAsync(scene));
        }


        public static string StringToAsset(this string value)
        {
            string[] temp = value.Split('_');

            return temp[temp.Length - 1];
        }
    }
}
