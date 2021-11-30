using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;


[UpdateAfter(typeof(MoveForwardSystem))]
public class TimedDestroySystem : SystemBase
{
	EndSimulationEntityCommandBufferSystem buffer;

	protected override void OnCreate()
	{
		base.OnCreate();
		buffer = World
			.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
	}

    protected override void OnUpdate()
    {
		float dt = Time.DeltaTime;
		var ecb = buffer.CreateCommandBuffer().AsParallelWriter();

		Entities
			.ForEach(
			(Entity e, int entityInQueryIndex, ref TimeToLive ttl) =>
			{
				ttl.Value -= dt;
				if (ttl.Value <= 0)
				{
					ecb.DestroyEntity(entityInQueryIndex, e);
				}
			})
			.ScheduleParallel();

		buffer.AddJobHandleForProducer(this.Dependency);
    }
}

