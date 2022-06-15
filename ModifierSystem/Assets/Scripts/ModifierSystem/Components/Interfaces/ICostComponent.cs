namespace ModifierSystem
{
    public interface ICostComponent : IDisplay
    {
        bool ContainsCost();
        void ApplyCost();
        void SetupOwner(Being owner);
    }
}