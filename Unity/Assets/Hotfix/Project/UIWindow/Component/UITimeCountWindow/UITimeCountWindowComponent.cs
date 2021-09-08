using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace ETHotfix
{
	public partial class UITimeCountWindowComponent : UIWindowComponent
	{
		public void UpdateText(string text)
		{
			TimeText.text = text;
		}

		public string GetCurrentTime()
		{
			return TimeText.text;
		}
		private void RegisterEvent()
		{

		}
	}
}

