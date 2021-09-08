using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETHotfix
{
    public class MessageBoxHelper
    {
        public static void ShowMessage(string message, ConfirmPanelType confirmPanelType = ConfirmPanelType.EventType)
        {
            var messagewindow = UIHelper.OpenUI<UIMessageWindowComponent>();
            messagewindow.ShowPanel(null, message, confirmPanelType);
        }

        public static void ShowMessage(string message, System.Action action, ConfirmPanelType confirmPanelType = ConfirmPanelType.EventType)
        {
            var messagewindow = UIHelper.OpenUI<UIMessageWindowComponent>();
            messagewindow.ShowPanel(action, message, confirmPanelType);
        }

        public static void ShowMessage(string message, System.Action confrim, System.Action cancel, ConfirmPanelType confirmPanelType = ConfirmPanelType.EventType)
        {
            var messagewindow = UIHelper.OpenUI<UIMessageWindowComponent>();
            messagewindow.ShowPanel(confrim, cancel, message, confirmPanelType);
        }
    }
}
