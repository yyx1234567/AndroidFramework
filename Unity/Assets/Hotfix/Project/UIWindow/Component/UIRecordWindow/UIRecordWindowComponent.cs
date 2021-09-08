using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace ETHotfix
{
	public partial class UIRecordWindowComponent : UIWindowComponent
	{
 		private void RegisterEvent()
        {
            Name1.text = string.Empty;
            Name2.text = string.Empty;

            FirstTimeRecord.GetComponent<Button>().onClick.AddListener(()=> 
            {
                UIHelper.OperateUI(FirstTimeRecord.name, () => { 
                    RecrodTime(FirstTimeRecord.transform); 
                });
            });

            EndTimeRecord.GetComponent<Button>().onClick.AddListener(() =>
            {
                UIHelper.OperateUI(EndTimeRecord.name, () => {
                    RecrodTime(EndTimeRecord.transform);
                });
            });
        }


        public void RecrodTime(Transform target)
        {
            var tmptext = target.parent.transform.Find("Image").GetComponentInChildren<TMPro.TMP_Text>();
            tmptext.text = UIHelper.GetUI<UITimeCountWindowComponent>().GetCurrentTime();
        }

        public string  GetTime1()
        {
            return Name1.text;
        }
        public string  GetTime2()
        {
            return Name2.text;
        }
        public override void ResetUI(List<string> itemlist)
        {
            foreach (var item in itemlist)
            {
                switch (item)
                {
                    case "FirstTimeRecord":
                        Name1.text = string.Empty;
                        break;
                    case "EndTimeRecord":
                        Name2.text = string.Empty;
                        break;
                }
            }
        }
    }
}

