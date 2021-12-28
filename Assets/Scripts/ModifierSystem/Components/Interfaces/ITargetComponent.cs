namespace ModifierSystem
{
    public interface ITargetComponent
    {
        ConditionalTarget ConditionalTarget { get; }
        Being Target { get; }
        Being Owner { get; }
        bool SetTarget(Being target);
    }
}