namespace ModifierSystem
{
    public interface ITargetComponent
    {
        Being Target { get; }
        Being Owner { get; }
        bool SetTarget(Being target);
    }
}