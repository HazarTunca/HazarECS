namespace ECS
{
    public abstract class ECSSystem
    {
        public World world;
        
        public EntityFilter Filter() => new EntityFilter(world);
    }
}