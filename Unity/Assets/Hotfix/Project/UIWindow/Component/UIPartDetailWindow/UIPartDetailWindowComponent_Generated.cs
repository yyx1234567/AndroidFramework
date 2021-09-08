using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UIPartDetailWindowComponentAwakeSystem : AwakeSystem<UIPartDetailWindowComponent>
	{
		public override void Awake(UIPartDetailWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UIPartDetailWindowComponent : UIWindowComponent
	{
		private Coffee.UIExtensions.UIGradient ViewItem;

		private UIToggleOdin ViewItem2;

		private UnityEngine.RectTransform Content;

		private UnityEngine.UI.Text InfoText;

		private UIContentsizeFilter Info;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			ViewItem=Collector.GetMonoComponent<Coffee.UIExtensions.UIGradient>("ViewItem");

			ViewItem2=Collector.GetMonoComponent<UIToggleOdin>("ViewItem2");

			Content=Collector.GetMonoComponent<UnityEngine.RectTransform>("Content");

			InfoText=Collector.GetMonoComponent<UnityEngine.UI.Text>("InfoText");

			Info=Collector.GetMonoComponent<UIContentsizeFilter>("Info");

			this.RegisterEvent();
		}
	}
}
