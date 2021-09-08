using ETModel;
using System.Linq;

namespace ETHotfix
{
	[ObjectSystem]
	public class SessionPlayerComponentDestroySystem : DestroySystem<SessionPlayerComponent>
	{
		public override void Destroy(SessionPlayerComponent self)
		{
			DestroyAsync(self).Coroutine();
		}

		private static async ETVoid DestroyAsync(SessionPlayerComponent self)
        {
 			// 发送断线消息
			var player= Game.Scene.GetComponent<PlayerComponent>()?.Get(self.Player.Id);
			player.DisConnect = true;
			var room=	Game.Scene.GetComponent<RoomComponent>().GetAllRoom().
				Where(x => x.playersSeets.Where(y => y.Id == self.Player.Id).FirstOrDefault() != null).FirstOrDefault();
			if (room != null)
			{
				room.Remove(self.Player);
				//if (room.playersSeets.Count == 0)
				//{
				//	room.Dispose();
				//}
			}
			ActorLocationSender actorLocationSender = await Game.Scene.GetComponent<ActorLocationSenderComponent>().Get(self.Player.UnitId);
			actorLocationSender.Send(new G2M_SessionDisconnect()).Coroutine();

			M2C_Disconnect m2C_Create = new M2C_Disconnect();
		 
			MessageHelper.BroadcastRoom(m2C_Create);

		}
	}
}