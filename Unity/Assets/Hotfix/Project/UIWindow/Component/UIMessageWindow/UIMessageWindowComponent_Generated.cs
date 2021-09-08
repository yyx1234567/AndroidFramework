using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
	[ETModel.ObjectSystem]
	public class UIMessageWindowComponentAwakeSystem : AwakeSystem<UIMessageWindowComponent>
	{
		public override void Awake(UIMessageWindowComponent self)
		{
			self.Awake();
		}
	}
	public partial class UIMessageWindowComponent : UIWindowComponent
	{
 		public void Awake()
		{
 			this.RegisterEvent();
		}
	}
}
