using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace ETHotfix
{
	public partial class UIReportWindowComponent : UIWindowComponent
	{
		private void RegisterEvent()
		{
		}

        protected override void Show()
        {
            base.Show();
            //var projectconfigcomponent = ETModel.Game.Scene.GetComponent<ProjectConfigComponent>();
            //var data = projectconfigcomponent.ProjectManger.projects.Where(x => x.ProjectName == projectconfigcomponent.GetProjectData().Name).FirstOrDefault();
            //GetParent<UI>().GameObject.transform.Find(data.ProjectID).gameObject.SetActive(true);
            //ReportItem001.text = UIHelper.GetUI<UIRecordWindowComponent>().GetTime1();
            //ReportItem002.text = UIHelper.GetUI<UIRecordWindowComponent>().GetTime2();
            //UIHelper.CloseUI<UIRecordWindowComponent>();
        }
    }
}

