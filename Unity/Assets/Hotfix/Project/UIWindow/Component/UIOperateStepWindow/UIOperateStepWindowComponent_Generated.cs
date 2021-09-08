using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UIOperateStepWindowComponentAwakeSystem : AwakeSystem<UIOperateStepWindowComponent>
	{
		public override void Awake(UIOperateStepWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UIOperateStepWindowComponent : UIWindowComponent
	{
		private Coffee.UIExtensions.UIGradient ViewItem;

		private UIToggleOdin ViewItem2;

		private UIToggleGroupOdin Content;

		public UnityEngine.UI.Text InfoText;

		public UIContentsizeFilter Info;

		private UnityEngine.UI.Outline LastStep;

		private UnityEngine.UI.Outline Reset;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			ViewItem=Collector.GetMonoComponent<Coffee.UIExtensions.UIGradient>("ViewItem");

			ViewItem2=Collector.GetMonoComponent<UIToggleOdin>("ViewItem2");

			Content=Collector.GetMonoComponent<UIToggleGroupOdin>("Content");

			InfoText=Collector.GetMonoComponent<UnityEngine.UI.Text>("InfoText");

			Info=Collector.GetMonoComponent<UIContentsizeFilter>("Info");

			LastStep=Collector.GetMonoComponent<UnityEngine.UI.Outline>("LastStep");

			Reset=Collector.GetMonoComponent<UnityEngine.UI.Outline>("Reset");

			this.RegisterEvent();
		}
	}
}
