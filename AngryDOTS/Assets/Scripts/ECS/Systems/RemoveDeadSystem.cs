using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class RemoveDeadSystem : SystemBase
{
	EndSimulationEntityCommandBufferSystem buffer;

	protected override void OnCreate()
    {
		buffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
	{
		var ecb = buffer.CreateCommandBuffer();

		Entities
			.WithAll<PlayerTag>()
			.ForEach((ref Health health) =>
			{
				if (health.Value <= 0)
				{
					Settings.PlayerDied();
				}
			}).Run();

		Entities
			.WithAll<EnemyTag>()
			.ForEach((Entity e, int entityInQueryIndex, ref Health health, in Translation pos) =>
			{
				if (health.Value <= 0)
				{
					ecb.DestroyEntity(e);
					BulletImpactPool.PlayBulletImpact(pos.Value);
				}
			})
			.Run();
	}
}