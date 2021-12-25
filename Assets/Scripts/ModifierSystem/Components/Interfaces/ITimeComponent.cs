namespace ModifierSystem
{
    public interface ITimeComponent
    {
        void Init(ModifierController modifierController);
        void Update(float deltaTime, double statusResistance = 1d);
        bool EffectComponentIsOfType<T>(bool checkResetOnFinished = true) where T : IEffectComponent;
    }
}