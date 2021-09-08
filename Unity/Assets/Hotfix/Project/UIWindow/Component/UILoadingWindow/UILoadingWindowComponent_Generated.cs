using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UILoadingWindowComponentAwakeSystem : AwakeSystem<UILoadingWindowComponent>
	{
		public override void Awake(UILoadingWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UILoadingWindowComponent : UIWindowComponent
	{
		private UnityEngine.CanvasGroup LoadingUI;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			LoadingUI=Collector.GetMonoComponent<UnityEngine.CanvasGroup>("LoadingUI");

			this.RegisterEvent();
		}
	}
}
