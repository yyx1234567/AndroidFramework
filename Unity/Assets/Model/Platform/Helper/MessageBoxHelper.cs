using ETHotfix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETModel
{
    public class MessageBoxHelper
    {
        public static void ShowMessage(string message)
        {
            var messagewindow = GameObject.FindObjectOfType<UIMessageWindowMono>();
            messagewindow.ShowPanel(null, message);
        }

        public static void ShowMessage(string message, System.Action action)
        {
            var messagewindow = GameObject.FindObjectOfType<UIMessageWindowMono>();
            messagewindow.ShowPanel(action, message);
        }

        public static void ShowMessage(string message, System.Action confrim, System.Action cancel)
        {
            var messagewindow = GameObject.FindObjectOfType<UIMessageWindowMono>();
            messagewindow.ShowPanel(confrim, cancel, message);
        }
    }
}