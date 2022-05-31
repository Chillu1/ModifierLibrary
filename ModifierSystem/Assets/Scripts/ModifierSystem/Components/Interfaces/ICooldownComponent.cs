namespace ModifierSystem
{
    public interface ICooldownComponent
    {
        void Update(float deltaTime);
        bool IsReady();
        void ResetTimer();
    }
}