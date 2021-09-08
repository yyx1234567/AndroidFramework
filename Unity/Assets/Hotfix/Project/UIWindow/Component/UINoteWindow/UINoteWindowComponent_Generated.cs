using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UINoteWindowComponentAwakeSystem : AwakeSystem<UINoteWindowComponent>
	{
		public override void Awake(UINoteWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UINoteWindowComponent : UIWindowComponent
	{
		private UIGrid Content;

		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			Content=Collector.GetMonoComponent<UIGrid>("Content");

			this.RegisterEvent();
		}
	}
}
