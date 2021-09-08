using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

namespace ETHotfix
{
	public partial class UILoadingWindowComponent : UIWindowComponent
	{
		private void RegisterEvent()
		{
		}

        protected override  void Show()
        {
             //base.Show();
            LoadingUI.DOFade(1,0.5f);
         }

        protected override void Hide()
        {
            LoadingUI.DOFade(0,0.5F);
           // base.Hide();
        }
    }
}

