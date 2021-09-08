using ETModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2R_EnterRoomSelectHandler : AMRpcHandler<C2R_EnterRoomSelect, R2C_EnterRoomSelect>
    {
        protected async override ETTask RunAsync(Session session, C2R_EnterRoomSelect request, R2C_EnterRoomSelect response, Action reply)
        {
            IPEndPoint mapAddress = StartConfigComponent.Instance.MapConfigs[0].GetComponent<InnerConfig>().IPEndPoint;
            Session mapSession = Game.Scene.GetComponent<NetInnerComponent>().Get(mapAddress);
            M2G_CreateRoom createUnit = (M2G_CreateRoom)await mapSession.Call(new C2R_CreateRoom());

            reply();
            await ETTask.CompletedTask;
        }
     }
}
