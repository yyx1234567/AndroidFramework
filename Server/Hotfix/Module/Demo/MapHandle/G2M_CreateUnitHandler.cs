using System;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[MessageHandler(AppType.Map)]
	public class G2M_CreateUnitHandler : AMRpcHandler<G2M_CreateUnit, M2G_CreateUnit>
	{
		protected override async ETTask RunAsync(Session session, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
		{
			var player = Game.Scene.GetComponent<PlayerComponent>().Get(request.PlayerId);
			if (!player.HasCreateUnit)
			{
				Unit unit = ComponentFactory.CreateWithId<Unit>(IdGenerater.GenerateId());
				unit.UnitType = (UnitType)Enum.Parse(typeof(UnitType), request.role);
				unit.AddComponent<MoveComponent>();
				unit.AddComponent<UnitPathComponent>();
				unit.Position = new Vector3(-10, 0, -10);

				await unit.AddComponent<MailBoxComponent>().AddLocation();
				unit.AddComponent<UnitGateComponent, long>(request.GateSessionId);
				Game.Scene.GetComponent<UnitComponent>().Add(unit);
				response.UnitId = unit.Id;
			}
			else
			{
			    var unit=Game.Scene.GetComponent<UnitComponent>().Get(player.UnitId);
				unit.GetComponent<UnitGateComponent>().GateSessionActorId = request.GateSessionId;
				unit.UnitType = (UnitType)Enum.Parse(typeof(UnitType), request.role);
				response.UnitId = player.UnitId;
  			}

			// 广播创建的unit
			M2C_CreateUnits createUnits = new M2C_CreateUnits();
			Unit[] units = Game.Scene.GetComponent<UnitComponent>().GetAll();
 			foreach (Unit u in units)
			{
				UnitInfo unitInfo = new UnitInfo();
				unitInfo.X = u.Position.x;
				unitInfo.Y = u.Position.y;
				unitInfo.Z = u.Position.z;
				unitInfo.UnitId = u.Id;
				unitInfo.Role = u.UnitType.ToString();
				createUnits.Units.Add(unitInfo);
				Console.WriteLine($"{u.Position.x}    {u.Position.y}   {u.Position.z}");

			}
			MessageHelper.Broadcast(createUnits);
			
			reply();
		}
	}
}