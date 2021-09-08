using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UITimeCountWindowComponentAwakeSystem : AwakeSystem<UITimeCountWindowComponent>
	{
		public override void Awake(UITimeCountWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UITimeCountWindowComponent : UIWindowComponent
	{
		private TMPro.TextMeshProUGUI TimeText;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			TimeText=Collector.GetMonoComponent<TMPro.TextMeshProUGUI>("TimeText");

			this.RegisterEvent();
		}
	}
}
