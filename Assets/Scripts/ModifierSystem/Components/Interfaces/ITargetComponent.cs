namespace ModifierSystem
{
    public interface ITargetComponent
    {
        IBeing GetTarget();
        void SetTarget(IBeing target);
        IBeing GetOwner();
    }
}