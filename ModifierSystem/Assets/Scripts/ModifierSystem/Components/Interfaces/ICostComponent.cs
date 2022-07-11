namespace ModifierSystem
{
    public interface ICostComponent : IDisplayable
    {
        bool ContainsCost();
        void ApplyCost();
        void SetupOwner(Unit owner);
    }
}