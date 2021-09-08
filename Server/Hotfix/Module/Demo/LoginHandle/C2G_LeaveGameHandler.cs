 
using System;
using ETModel;
using System.Linq;
using System.Collections.Generic;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_LeaveGameHandler : AMRpcHandler<C2G_LeaveGame, G2C_LeaveGame>
	{
		protected override async ETTask RunAsync(Session session, C2G_LeaveGame request, G2C_LeaveGame response, Action reply)
		{
			try
			{
				var playerComponent = Game.Scene.GetComponent<PlayerComponent>();
				var player = playerComponent.Get(request.PlayerID);
				var room = Game.Scene.GetComponent<RoomComponent>().GetRoom(player.m_Room.Id);
				room.Remove(player);
				Game.Scene.GetComponent<PlayerComponent>().Remove(player.Id);
			}
			catch (Exception ex)
			{
				session.Error = ErrorCode.ERR_LeaveGameError;
			}
			reply();
			await ETTask.CompletedTask;
		}
	}
}