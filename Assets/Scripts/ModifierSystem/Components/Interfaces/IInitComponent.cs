namespace ModifierSystem
{
    public interface IInitComponent
    {
        void Init();
        bool EffectComponentIsOfType<T>() where T : IEffectComponent;
    }
}