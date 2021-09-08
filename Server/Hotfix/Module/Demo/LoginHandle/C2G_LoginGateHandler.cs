using System;
using ETModel;
using System.Linq;
using System.Collections.Generic;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override async ETTask RunAsync(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
		{
			string account = Game.Scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
 			if (account == null)
			{
				response.Error = ErrorCode.ERR_ConnectGateKeyError;
				response.Message = "Gate key验证失败!";
				reply();
				return;
			}
			var player= Game.Scene.GetComponent<PlayerComponent>().GetAll().Where(x => x.Account == account).FirstOrDefault();
			if (player == null)
			{
				player = ComponentFactory.Create<Player, string>(account);
				var component = player.AddComponent<UnitGateComponent, long>(player.Id);
				Game.Scene.GetComponent<PlayerComponent>().Add(player);
			}
			else
			{
				if (!string.IsNullOrEmpty(player.Role))
				{
 					session.Send(new G2C_Reconnect() { Role = player.Role });
				}
			}
			player.GetComponent<UnitGateComponent>().IsDisconnect=false;
			player.GetComponent<UnitGateComponent>().GateSessionActorId = session.InstanceId;
			var unit = Game.Scene.GetComponent<UnitComponent>().Get(player.UnitId);
			if (unit != null)
			{
				unit.GetComponent<UnitGateComponent>().IsDisconnect = false;
			}
			session.AddComponent<SessionPlayerComponent>().Player = player;
			session.AddComponent<MailBoxComponent, string>(MailboxType.GateSession);
			response.PlayerId = player.Id;
			reply();
  			await ETTask.CompletedTask;
		}
 	}
  }