namespace ModifierSystem
{
    public interface ITargetComponent
    {
        IBeing Target { get; }
        IBeing Owner { get; }
        bool SetTarget(IBeing target);
    }
}