namespace ModifierSystem
{
    public interface ICastComponent
    {
        bool TryCast();
        void Update(float deltaTime);
    }
}