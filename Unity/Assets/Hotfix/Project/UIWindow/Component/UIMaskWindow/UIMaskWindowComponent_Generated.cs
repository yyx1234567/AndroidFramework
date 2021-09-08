using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UIMaskWindowComponentAwakeSystem : AwakeSystem<UIMaskWindowComponent>
	{
		public override void Awake(UIMaskWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UIMaskWindowComponent : UIWindowComponent
	{
		public void Awake()
		{
			var go = GetParent<UI>().GameObject;

			this.RegisterEvent();
		}
	}
}
