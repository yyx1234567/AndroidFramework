using ETModel;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UITaskPanelHelper
{
    public static async ETTask<T> GetPanelResult<T>(IPanelSelectHandle<T> panelSelectHandle)
    {
        PanelSelectTask<T> panelSelectTask = new PanelSelectTask<T>();
        Regeister(panelSelectHandle, panelSelectTask);
        await panelSelectTask;
        return panelSelectTask.Reslut;
     }

    private static async void Regeister<T>(IPanelSelectHandle<T> panelSelectHandle,PanelSelectTask<T> panelSelectTask)
    {
        await Task.Delay(100);
        panelSelectHandle.ConfirmAction += panelSelectTask.onComplete;
      }
}
