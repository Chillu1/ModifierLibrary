namespace ModifierSystem
{
    public interface ITimeComponent
    {
        void Init(ModifierController modifierController);
        void Update(float deltaTime, double statusResistance);
    }
}