using System;
using System.Linq;
using System.Net;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_EnterMapHandler : AMRpcHandler<C2G_EnterMap, G2C_EnterMap>
	{
		protected override async ETTask RunAsync(Session session, C2G_EnterMap request, G2C_EnterMap response, Action reply)
		{
 			Player player = session.GetComponent<SessionPlayerComponent>().Player;
 			// 在map服务器上创建战斗Unit
			IPEndPoint mapAddress = StartConfigComponent.Instance.MapConfigs[0].GetComponent<InnerConfig>().IPEndPoint;
			Session mapSession = Game.Scene.GetComponent<NetInnerComponent>().Get(mapAddress);
			M2G_CreateUnit createUnit = (M2G_CreateUnit)await mapSession.Call(new G2M_CreateUnit() {role=request.Role, PlayerId = player.Id, GateSessionId = session.InstanceId });
			player.UnitId = createUnit.UnitId;
			player.HasCreateUnit = true;
			player.Role = request.Role;
			response.UnitId = createUnit.UnitId;
			reply();
		}
	}
}