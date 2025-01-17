﻿using PF;
using UnityEngine;

namespace ETModel
{
	public enum UnitType
	{
		Hero,
		Npc,
		Meet,
		EngineeringEmergency,
		EmergencyRescue,
		Defend,
		Skeleton
	}

	[ObjectSystem]
	public class UnitAwakeSystem : AwakeSystem<Unit, UnitType>
	{
		public override void Awake(Unit self, UnitType a)
		{
			self.Awake(a);
		}
	}

	public sealed class Unit: Entity
	{
		public UnitType UnitType { get;  set; }
		
		public Vector3 Position { get; set; }
		
		public void Awake(UnitType unitType)
		{
			this.UnitType = unitType;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
		}
	}
}