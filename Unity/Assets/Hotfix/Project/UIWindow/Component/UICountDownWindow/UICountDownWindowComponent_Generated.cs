using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UICountDownWindowComponentAwakeSystem : AwakeSystem<UICountDownWindowComponent>
	{
		public override void Awake(UICountDownWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UICountDownWindowComponent : UIWindowComponent
	{
		private UnityEngine.UI.Button FillAmount;

		private TMPro.TextMeshProUGUI TextTarget;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			FillAmount=Collector.GetMonoComponent<UnityEngine.UI.Button>("FillAmount");

			TextTarget=Collector.GetMonoComponent<TMPro.TextMeshProUGUI>("TextTarget");

			this.RegisterEvent();
		}
	}
}
