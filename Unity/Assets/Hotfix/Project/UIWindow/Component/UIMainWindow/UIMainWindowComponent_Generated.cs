using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UIMainWindowComponentAwakeSystem : AwakeSystem<UIMainWindowComponent>
	{
		public override void Awake(UIMainWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UIMainWindowComponent : UIWindowComponent
	{
		private ETModel.UIWindow UIAboutUsWindow;

		private ETModel.UIWindow UIFeedBackWindow;

		private UIToggleOdin Toggle_01;

		private UIToggleOdin Toggle_02;

		private UIToggleOdin Toggle_03;

		private TMPro.TextMeshProUGUI TitleName;

		private UIButtonOdin QuitBtn;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			UIAboutUsWindow=Collector.GetMonoComponent<ETModel.UIWindow>("UIAboutUsWindow");

			UIFeedBackWindow=Collector.GetMonoComponent<ETModel.UIWindow>("UIFeedBackWindow");

			Toggle_01=Collector.GetMonoComponent<UIToggleOdin>("Toggle_01");

			Toggle_02=Collector.GetMonoComponent<UIToggleOdin>("Toggle_02");

			Toggle_03=Collector.GetMonoComponent<UIToggleOdin>("Toggle_03");

			TitleName=Collector.GetMonoComponent<TMPro.TextMeshProUGUI>("TitleName");

			QuitBtn=Collector.GetMonoComponent<UIButtonOdin>("QuitBtn");

			this.RegisterEvent();
		}
	}
}
