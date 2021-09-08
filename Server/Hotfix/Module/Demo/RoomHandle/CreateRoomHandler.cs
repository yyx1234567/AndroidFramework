using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class CreateRoomHandler : AMRpcHandler<C2R_CreateRoom, M2G_CreateRoom>
    {
        protected override async ETTask RunAsync(Session session, C2R_CreateRoom request, M2G_CreateRoom response, Action reply)
        {
             Player player = session.GetComponent<SessionPlayerComponent>().Player;
            var roomcomponent = Game.Scene.GetComponent<RoomComponent>();
            if (!string.IsNullOrEmpty(request.Message))
            {
                Room room = new Room() { RoomName = request.Message };
                room.Add(player);
                roomcomponent.Add(new Room() { RoomName=request.Message});
            }
             //roomcomponent.Add(new Room());
             var roomComponent=  Game.Scene.GetComponent<RoomComponent>();
            M2C_CreateRooms m2C_Create = new M2C_CreateRooms();
             foreach (var item in roomComponent.GetAllRoom())
            {
               ETModel.roominfo roominfo = new ETModel.roominfo()
                {
                    RoomName = item.RoomName,
                    Roomid = item.Id,
                    PlayerNumber=item.Count
               };
                m2C_Create.Units.Add(roominfo);
            }
            MessageHelper.BroadcastRoom(m2C_Create);


            reply();
            await ETTask.CompletedTask;
        }
    }
}
