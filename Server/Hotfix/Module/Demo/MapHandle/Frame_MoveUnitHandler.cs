using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ActorMessageHandler(AppType.Map)]
	public class Frame_MoveUnitHandler : AMActorLocationHandler<Unit, Frame_MoveUnit>
	{
		protected override async ETTask Run(Unit unit, Frame_MoveUnit message)
		{
			Vector3 target = new Vector3(message.X, message.Y, message.Z);
			unit.Position = target;
			await ETTask.CompletedTask;
		}
	}
}