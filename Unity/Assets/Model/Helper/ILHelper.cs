using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

namespace ETModel
{
	public static class ILHelper
	{
		public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
		{
			// 注册重定向函数

			// 注册委托
			AdditionalDelegate( appdomain);
			appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
			appdomain.DelegateManager.RegisterMethodDelegate<AChannel, System.Net.Sockets.SocketError>();
			appdomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
			appdomain.DelegateManager.RegisterMethodDelegate<IResponse>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session, object>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session, ushort, MemoryStream>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session>();
			appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
			appdomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();
			appdomain.DelegateManager.RegisterMethodDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();
            appdomain.DelegateManager.RegisterMethodDelegate<UIButtonOdin>();
            appdomain.DelegateManager.RegisterMethodDelegate<UIToggleOdin>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.UI.Toggle, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.UI.Toggle, System.String>();
			appdomain.DelegateManager.RegisterFunctionDelegate<ETModel.UIConfig, System.Boolean>();
			appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.AsyncOperation>();

			appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int64, System.String>, System.String>();

			appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<bool>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<bool>((x) =>
                {
                    ((Action<bool>)act)(x);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<string>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<string>((x) =>
                {
                    ((Action<string>)act)(x);
                });
            });
            CLRBindings.Initialize(appdomain);

			// 注册适配器
			Assembly assembly = typeof(Init).Assembly;
			foreach (Type type in assembly.GetTypes())
			{
				object[] attrs = type.GetCustomAttributes(typeof(ILAdapterAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}
				object obj = Activator.CreateInstance(type);
				CrossBindingAdaptor adaptor = obj as CrossBindingAdaptor;
				if (adaptor == null)
				{
					continue;
				}
				appdomain.RegisterCrossBindingAdaptor(adaptor);
			}

			LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
		}


		/// <summary>
		/// 生成常用方法委托
		/// </summary>
        private static void AdditionalDelegate(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
			appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<ETModel.UIConfig>>((act) =>
			{
				return new System.Predicate<ETModel.UIConfig>((obj) =>
				{
					return ((Func<ETModel.UIConfig, System.Boolean>)act)(obj);
				});
			});
			appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
			{
				return new UnityEngine.Events.UnityAction(() =>
				{
					((Action)act)();
				});
			});
		}
    }
}