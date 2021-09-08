using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace ETHotfix
{
    public partial class UIMainWindowComponent : UIWindowComponent
    {
        private void RegisterEvent()
        {
            UIFeedBackWindow.Init();
            UIAboutUsWindow.Init();
            //TitleName.text = ProjectConfigComponent.Instance.GetProjectData().Name;
            try
            {
                QuitBtn.ClickEvent += () =>
                {
                    MessageBoxHelper.ShowMessage("是否退出当前软件？", ()=> { Debug.Log("退出"); Application.Quit(); }, ConfirmPanelType.QuitPanel);
                };
                Toggle_01.ToggleEvent +=  (arg) =>
                 {
                     if (arg)
                     {
                         UIHelper.OpenUI<UINoteWindowComponent>();
                     }
                     else
                     {
                         UIHelper.CloseUI<UINoteWindowComponent>();
                     }
                 };
                Toggle_02.ToggleEvent +=  (arg) =>
                {
                    if (arg)
                    {
                        UIHelper.OpenUI<UIPartDetailWindowComponent>();
                    }
                    else
                    {
                        UIHelper.CloseUI<UIPartDetailWindowComponent>();
                    }
                };
                Toggle_03.ToggleEvent += (arg) =>
                {
                    if (arg)
                    {
                        var panel = UIHelper.OpenUI<UIOperateStepWindowComponent>();
                        panel.Init();
                    }
                    else
                    {
                        UIHelper.CloseUI<UIOperateStepWindowComponent>();
                    }
                };
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.StackTrace);
            }
        }
    }
}

