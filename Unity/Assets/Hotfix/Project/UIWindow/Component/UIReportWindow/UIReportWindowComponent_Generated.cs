using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UIReportWindowComponentAwakeSystem : AwakeSystem<UIReportWindowComponent>
	{
		public override void Awake(UIReportWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UIReportWindowComponent : UIWindowComponent
	{
		private UnityEngine.RectTransform Report_01;

		private UnityEngine.RectTransform Report_02;

		private TMPro.TextMeshProUGUI ReportItem001;

		private TMPro.TextMeshProUGUI ReportItem002;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			Report_01=Collector.GetMonoComponent<UnityEngine.RectTransform>("Report_01");

			Report_02=Collector.GetMonoComponent<UnityEngine.RectTransform>("Report_02");

			ReportItem001=Collector.GetMonoComponent<TMPro.TextMeshProUGUI>("ReportItem001");

			ReportItem002=Collector.GetMonoComponent<TMPro.TextMeshProUGUI>("ReportItem002");

			this.RegisterEvent();
		}
	}
}
