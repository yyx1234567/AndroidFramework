using ETModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace ETModel
{
    public class DataHelper
    {
        public static async ETTask<T> TryGet<T>(Predicate<T> predicate) where T : ETModel.ScriptObjectBaseConfig
        {
            var component = Game.Scene.GetComponent<ScriptObjectConfigComponent>();
            if (component == null)
            {
                component = Game.Scene.AddComponent<ScriptObjectConfigComponent>();
                await component.LoadAsync();
            }
            return component.TryGet(predicate);
        }
        public static async ETTask<T> TryGetDynamicData<T>(Predicate<T> predicate) where T : ETModel.ScriptObjectBaseConfig
        {
            var component = Game.Scene.GetComponent<ScriptObjectConfigComponent>();
            if (component == null)
            {
                component = Game.Scene.AddComponent<ScriptObjectConfigComponent>();
                await component.LoadAsync();
            }
            return component.TryGet(predicate);
        }

        public static async ETTask<T[]> TryGetAll<T>(Predicate<T> predicate) where T : ETModel.ScriptObjectBaseConfig
        {
            var component = Game.Scene.GetComponent<ScriptObjectConfigComponent>();
            if (component == null)
            {
                component = Game.Scene.AddComponent<ScriptObjectConfigComponent>();
                await component.LoadAsync();
            }
            return component.TryGetAll(predicate);
        }

        public static async ETTask<IEnumerable<T>> GetAllDynamicData<T>() where T : ETModel.ScriptObjectBaseConfig
        {
            var component = Game.Scene.GetComponent<ScriptObjectConfigComponent>();
            if (component == null)
            {
                component = Game.Scene.AddComponent<ScriptObjectConfigComponent>();
                await component.LoadAsync();
            }
            return component.GetAllDynamicData<T>();
        }
    }
}