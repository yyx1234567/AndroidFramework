using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ActorMessageHandler(AppType.Map)]
	public class Frame_ClickMapHandler : AMActorLocationHandler<Unit, Frame_ClickMap>
	{
		protected override async ETTask Run(Unit unit, Frame_ClickMap message)
		{
			//Vector3 target = new Vector3(message.X, message.Y, message.Z);
			//unit.GetComponent<UnitPathComponent>().MoveTo(target).Coroutine();
			//await ETTask.CompletedTask;

            M2C_PathfindingResult m2CPathfindingResult = new M2C_PathfindingResult();
            m2CPathfindingResult.X = message.X;
            m2CPathfindingResult.Y = message.Y;
            m2CPathfindingResult.Z = message.Z;
            m2CPathfindingResult.Id = unit.Id;
			var serverunit= Game.Scene.GetComponent<UnitComponent>().Get(unit.Id);
			serverunit.Position = new Vector3(message.X, message.Y, message.Z);
			MessageHelper.Broadcast(m2CPathfindingResult);
            await ETTask.CompletedTask;

		}
	}
}