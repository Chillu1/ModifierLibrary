namespace ModifierSystem
{
    public interface ICostComponent
    {
        bool ContainsCost();
        void ApplyCost();
        void SetupOwner(Being owner);
    }
}