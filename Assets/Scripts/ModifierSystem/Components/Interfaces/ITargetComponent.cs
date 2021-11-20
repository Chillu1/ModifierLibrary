namespace ModifierSystem
{
    public interface ITargetComponent
    {
        IBeing GetTarget();
        bool SetTarget(IBeing target);
        IBeing GetOwner();
    }
}