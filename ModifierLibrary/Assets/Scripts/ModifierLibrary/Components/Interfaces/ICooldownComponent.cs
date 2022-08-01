namespace ModifierLibrary
{
    public interface ICooldownComponent : IDisplayable
    {
        void Update(float deltaTime);
        bool IsReady();
        void ResetTimer();
    }
}