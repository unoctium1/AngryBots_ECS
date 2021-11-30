using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(MoveForwardSystem))]
public class TurnTowardsPlayerSystem : SystemBase
{
	protected override void OnUpdate()
	{
		if (Settings.IsPlayerDead())
			return;

		float3 playerPosition = Settings.PlayerPosition;

		Entities
			.WithAll<EnemyTag>()
			.ForEach((ref Rotation rot, in Translation pos) =>
			{
				RotateTowards(playerPosition, ref rot, in pos);
			})
			.ScheduleParallel();
	}

	static void RotateTowards(float3 position, ref Rotation rot, in Translation pos)
    {
		float3 heading = position - pos.Value;
		heading.y = 0f;
		rot.Value = quaternion.LookRotation(heading, math.up());
	}
}

