namespace ModifierSystem
{
    public interface ICooldownComponent : IDisplay
    {
        void Update(float deltaTime);
        bool IsReady();
        void ResetTimer();
    }
}