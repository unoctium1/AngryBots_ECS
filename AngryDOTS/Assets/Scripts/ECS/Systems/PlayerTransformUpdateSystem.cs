using Unity.Entities;
using Unity.Transforms;

[UpdateBefore(typeof(CollisionSystem))]
public class PlayerTransformUpdateSystem : SystemBase
{
	protected override void OnUpdate()
	{
		if (Settings.IsPlayerDead())
			return;

		Entities
			.WithAll<PlayerTag>()
			.ForEach((ref Translation pos) =>
			{
				pos = new Translation { Value = Settings.PlayerPosition };
			})
			.Run();
	}
}