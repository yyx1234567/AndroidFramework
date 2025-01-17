﻿using System;
using ETModel;

namespace ETHotfix
{
    public static class Init
    {
        public static void Start()
        {
#if ILRuntime
            if (!Define.IsILRuntime)
            {
                Log.Error("mono层是mono模式, 但是Hotfix层是ILRuntime模式");
            }
#else
			if (Define.IsILRuntime)
			{
				Log.Error("mono层是ILRuntime模式, Hotfix层是mono模式");
			}
#endif

            try
            {
                // 注册热更层回调
                ETModel.Game.Hotfix.Update = () => { Update(); };
                ETModel.Game.Hotfix.LateUpdate = () => { LateUpdate(); };
                ETModel.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };
                var EventSystem = Game.EventSystem;

                // 加载热更配置
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
            }
        }

        public static void Update()
        {
            try
            {
                Game.EventSystem.Update();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void LateUpdate()
        {
            try
            {
                Game.EventSystem.LateUpdate();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void OnApplicationQuit()
        {
            Game.Close();
        }
    }
}