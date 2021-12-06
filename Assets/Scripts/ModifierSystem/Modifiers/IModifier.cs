namespace ModifierSystem
{
    public interface IModifier
    {
        object Clone();
        bool ValidatePrototypeSetup();
    }
}