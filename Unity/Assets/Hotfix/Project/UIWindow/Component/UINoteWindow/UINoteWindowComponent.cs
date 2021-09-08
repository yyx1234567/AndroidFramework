using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ETModel;
using System.Linq;

namespace ETHotfix
{
	public partial class UINoteWindowComponent : UIWindowComponent
	{
		private void RegisterEvent()
		{
			//Content.Show(ProjectConfigComponent.Instance.TryGetNote().Select(x=>x.NoteInfo).ToList());
		}
	}
}

