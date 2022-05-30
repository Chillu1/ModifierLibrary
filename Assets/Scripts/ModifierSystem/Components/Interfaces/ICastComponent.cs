namespace ModifierSystem
{
    public interface ICastComponent
    {
        bool IsAutomaticCasting { get; }
        bool IsReadyToCast { get; }
        bool CanCast();
        void Update(float deltaTime);
    }
}