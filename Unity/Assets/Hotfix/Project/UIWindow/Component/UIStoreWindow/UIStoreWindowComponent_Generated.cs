using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UIStoreWindowComponentAwakeSystem : AwakeSystem<UIStoreWindowComponent>
	{
		public override void Awake(UIStoreWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UIStoreWindowComponent : UIWindowComponent
	{
		private UIToggleGroupOdin AidPanel;

		private UnityEngine.UI.Button ShowBtn;

		private UIButtonOdin LeftBtn;

		private UIButtonOdin RightBtn;

		private UIToggleGroupOdin TaskContent;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			AidPanel=Collector.GetMonoComponent<UIToggleGroupOdin>("AidPanel");

			ShowBtn=Collector.GetMonoComponent<UnityEngine.UI.Button>("ShowBtn");

			LeftBtn=Collector.GetMonoComponent<UIButtonOdin>("LeftBtn");

			RightBtn=Collector.GetMonoComponent<UIButtonOdin>("RightBtn");

			TaskContent=Collector.GetMonoComponent<UIToggleGroupOdin>("TaskContent");

			this.RegisterEvent();
		}
	}
}
