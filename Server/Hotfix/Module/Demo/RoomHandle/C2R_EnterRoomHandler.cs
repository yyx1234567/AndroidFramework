using ETModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2R_EnterRoomHandler : AMRpcHandler<C2R_EnterRoom, R2C_EnterRoom>
    {
        protected override async ETTask RunAsync(Session session, C2R_EnterRoom request, R2C_EnterRoom response, Action reply)
        {
            var room= Game.Scene.GetComponent<RoomComponent>().GetRoom(request.Roomid);
            Player player = session.GetComponent<SessionPlayerComponent>().Player;
            room.Add(player);

            M2C_CreateRooms m2C_Create = new M2C_CreateRooms();
            foreach (var item in Game.Scene.GetComponent<RoomComponent>().GetAllRoom())
            {
                ETModel.roominfo roominfo = new ETModel.roominfo()
                {
                    RoomName = item.RoomName,
                    Roomid = item.Id,
                    PlayerNumber = item.Count
                };
                m2C_Create.Units.Add(roominfo);
            }
            MessageHelper.BroadcastRoom(m2C_Create);
            reply();
            await ETTask.CompletedTask;
        }
    }
}
