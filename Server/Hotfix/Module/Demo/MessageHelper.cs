using ETModel;
using System;

namespace ETHotfix
{
	public static class MessageHelper
	{
		public static void Broadcast(IActorMessage message)
		{
			Unit[] units = Game.Scene.GetComponent<UnitComponent>().GetAll();
 			ActorMessageSenderComponent actorLocationSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
			foreach (Unit unit in units)
			{
				UnitGateComponent unitGateComponent = unit.GetComponent<UnitGateComponent>();
				if (unitGateComponent.IsDisconnect)
				{
					continue;
				}
 				ActorMessageSender actorMessageSender = actorLocationSenderComponent.Get(unitGateComponent.GateSessionActorId);
				actorMessageSender.Send(message);
			}
		}
		public static void BroadcastRoom(IActorMessage message)
		{
			Player[] units = Game.Scene.GetComponent<PlayerComponent>().GetAll();

 			ActorMessageSenderComponent actorLocationSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
			foreach (Player unit in units)
			{
				UnitGateComponent unitGateComponent = unit.GetComponent<UnitGateComponent>();
  
				if (unitGateComponent.IsDisconnect)
				{
					continue;
				}
 				ActorMessageSender actorMessageSender = actorLocationSenderComponent.Get(unitGateComponent.GateSessionActorId);
				actorMessageSender.Send(message);
			}
		}
	}
}
