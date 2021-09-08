using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace ETHotfix
{
	public partial class UICountDownWindowComponent : UIWindowComponent
	{
		private void RegisterEvent()
		{
		}

		public void UpdateValue(string value,float progress)
		{
			TextTarget.text = value;
			FillAmount.GetComponent<Image>().fillAmount = progress;
		}
	}
}

