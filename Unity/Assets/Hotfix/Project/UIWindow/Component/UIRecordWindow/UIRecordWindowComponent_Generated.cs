using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UIRecordWindowComponentAwakeSystem : AwakeSystem<UIRecordWindowComponent>
	{
		public override void Awake(UIRecordWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UIRecordWindowComponent : UIWindowComponent
	{
		private TMPro.TextMeshProUGUI Name1;

		private TMPro.TextMeshProUGUI Name2;

		private Coffee.UIExtensions.UIGradient FirstTimeRecord;

		private Coffee.UIExtensions.UIGradient EndTimeRecord;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			Name1=Collector.GetMonoComponent<TMPro.TextMeshProUGUI>("Name1");

			Name2=Collector.GetMonoComponent<TMPro.TextMeshProUGUI>("Name2");

			FirstTimeRecord=Collector.GetMonoComponent<Coffee.UIExtensions.UIGradient>("FirstTimeRecord");

			EndTimeRecord=Collector.GetMonoComponent<Coffee.UIExtensions.UIGradient>("EndTimeRecord");

			this.RegisterEvent();
		}
	}
}
