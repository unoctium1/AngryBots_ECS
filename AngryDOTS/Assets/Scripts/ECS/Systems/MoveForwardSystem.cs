using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Unity.Transforms
{
	public class MoveForwardSystem : SystemBase
	{
        protected override void OnUpdate()
        {
			float dt = Time.DeltaTime;

			Entities
				.WithAll<MoveForward>()
				.ForEach((ref Translation pos, in Rotation rot, in MoveSpeed speed) =>
				{
					pos.Value = pos.Value + (dt * speed.Value * math.forward(rot.Value));
				})
				.ScheduleParallel();
        }
    }
}